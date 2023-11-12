
using AlgorandAuthentication;
using AsaGoldRepository.Model.Config;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Nest;
using NLog;
using NLog.Web;
using RestDWH.Base.Extensios;
using RestDWH.Base.Model;
using RestDWH.Base.Repository;
using RestDWH.Elastic.Extensions;
using RestDWH.Elastic.Repository;

namespace AsaGoldRepository
{
    public class Program
    {
        public static HashSet<string> Admins = new HashSet<string>();
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Asa Gold Repository API",
                        Version = "v1",
                        Description = File.ReadAllText("doc/readme.md")
                    });
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "ARC-0014 Algorand authentication transaction",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                    });

                    c.OperationFilter<Swashbuckle.AspNetCore.Filters.SecurityRequirementsOperationFilter>();
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"doc/documentation.xml"));
                }
                );

            var repositoryOptions = new RepositoryOptions();
            builder.Configuration.GetSection("Repository").Bind(repositoryOptions);
            repositoryOptions.ApiKey = builder.Configuration["Repository:ApiKey"];
            repositoryOptions.ElasticURI = builder.Configuration["Repository:ElasticURI"];
            builder.Services.AddProblemDetails();
            builder.Services.Configure<Model.Config.RepositoryOptions>(builder.Configuration.GetSection("Repository"));

            var settings =
                new ConnectionSettings(new Uri(repositoryOptions.ElasticURI))
                .ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(repositoryOptions.ApiKey))
                .ExtendElasticConnectionSettings();
            var client = new ElasticClient(settings);
            builder.Services.AddSingleton<IElasticClient>(client);

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.Account>, RestDWHElasticSearchRepository<Model.DWH.Account>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.Account>>();

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.AccountEmail>, RestDWHElasticSearchRepository<Model.DWH.AccountEmail>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.AccountEmail>>();

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.EmailValidation>, RestDWHElasticSearchRepository<Model.DWH.EmailValidation>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.EmailValidation>>();

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.Settings>, RestDWHElasticSearchRepository<Model.DWH.Settings>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.Settings>>();

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.KYCRequest>, RestDWHElasticSearchRepository<Model.DWH.KYCRequest>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.KYCRequest>>();

            builder.Services.AddSingleton<IDWHRepository<Model.DWH.RFQ>, RestDWHElasticSearchRepository<Model.DWH.RFQ>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.DWH.RFQ>>();


            var algorandAuthenticationOptions = new AlgorandAuthenticationOptions();
            builder.Configuration.GetSection("AlgorandAuthentication").Bind(algorandAuthenticationOptions);

            builder.Services
             .AddAuthentication(AlgorandAuthenticationHandler.ID)
             .AddAlgorand(o =>
             {
                 o.CheckExpiration = algorandAuthenticationOptions.CheckExpiration;
                 o.Debug = algorandAuthenticationOptions.Debug;
                 o.AlgodServer = algorandAuthenticationOptions.AlgodServer;
                 o.AlgodServerToken = algorandAuthenticationOptions.AlgodServerToken;
                 o.AlgodServerHeader = algorandAuthenticationOptions.AlgodServerHeader;
                 o.Realm = algorandAuthenticationOptions.Realm;
                 o.NetworkGenesisHash = algorandAuthenticationOptions.NetworkGenesisHash;
                 o.MsPerBlock = algorandAuthenticationOptions.MsPerBlock;
                 o.EmptySuccessOnFailure = algorandAuthenticationOptions.EmptySuccessOnFailure;
                 o.EmptySuccessOnFailure = algorandAuthenticationOptions.EmptySuccessOnFailure;
             });
            var admins = builder.Configuration.GetSection("Admins").AsEnumerable().Select(k => k.Value).Where(k => !string.IsNullOrEmpty(k)).Select(k=>k.ToString()).ToHashSet();
            if (admins?.Any() == true)
            {
                Admins = admins;
                logger.Info($"Admins: {string.Join(",", Admins)}");
            }

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseExceptionHandler(exceptionHandlerApp
                => exceptionHandlerApp.Run(async context
                    =>
                {
                    var exception = context.Features.Get<IExceptionHandlerPathFeature>().Error;
                    await Results.Problem(exception.Message, null, 400, exception.Message).ExecuteAsync(context);
                }));
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.Account>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.AccountEmail>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.EmailValidation>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.Settings>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.KYCRequest>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.DWH.RFQ>>());

            app.MapControllers();

            app.Run();
        }
    }
}