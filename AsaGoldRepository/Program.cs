
using AlgorandAuthentication;
using AsaGoldRepository.Model.Config;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Nest;
using RestDWH.Base.Extensios;
using RestDWH.Base.Model;
using RestDWH.Base.Repository;
using RestDWH.Elastic.Extensions;
using RestDWH.Elastic.Repository;

namespace AsaGoldRepository
{
    public class Program
    {
        public static void Main(string[] args)
        {
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

            builder.Services.AddSingleton<IDWHRepository<Model.RestDWH.Account>, RestDWHElasticSearchRepository<Model.RestDWH.Account>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.RestDWH.Account>>();

            builder.Services.AddSingleton<IDWHRepository<Model.RestDWH.AccountEmail>, RestDWHElasticSearchRepository<Model.RestDWH.AccountEmail>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.RestDWH.AccountEmail>>();

            builder.Services.AddSingleton<IDWHRepository<Model.RestDWH.EmailValidation>, RestDWHElasticSearchRepository<Model.RestDWH.EmailValidation>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.RestDWH.EmailValidation>>();

            builder.Services.AddSingleton<IDWHRepository<Model.RestDWH.Settings>, RestDWHElasticSearchRepository<Model.RestDWH.Settings>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.RestDWH.Settings>>();

            builder.Services.AddSingleton<IDWHRepository<Model.RestDWH.KYCRequest>, RestDWHElasticSearchRepository<Model.RestDWH.KYCRequest>>();
            builder.Services.AddSingleton<RestDWHEvents<Model.RestDWH.KYCRequest>>();


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
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.RestDWH.Account>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.RestDWH.AccountEmail>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.RestDWH.EmailValidation>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.RestDWH.Settings>>());
            app.MapEndpoints(app.Services.GetService<IDWHRepository<Model.RestDWH.KYCRequest>>());

            app.MapControllers();

            app.Run();
        }
    }
}