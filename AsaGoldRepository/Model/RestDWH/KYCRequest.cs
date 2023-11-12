using Microsoft.AspNetCore.JsonPatch;
using Nest;
using RestDWH.Base.Attributes;
using RestDWH.Base.Model;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace AsaGoldRepository.Model.DWH
{
    /// <summary>
    /// KYC data
    /// 
    /// Id is Account address
    /// </summary>
    [RestDWHEntity(
        name: "KYCRequest",
        endpointGet: "api/v1/kyc",
        endpointGetById: "api/v1/kyc/{id}",
        endpointUpsert: "api/v1/kyc/{id}",
        endpointPatch: "api/v1/kyc/{id}"
        )]
    public class KYCRequest
    {
        public class Address
        {
            public string Street { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }
        }
        public string LegalEntity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Company { get; set; }
        public string? TaxId { get; set; }
        public Address? DeliveryAddress { get; set; }
        public Address? ResidentialAddress { get; set; }
        public Address? CompanyAddress { get; set; }
    }
    public class KYCEvents : RestDWH.Base.Model.RestDWHEvents<RFQ>
    {
        public override Task<(int from, int size, string query, string sort)> BeforeGetAsync(int from = 0, int size = 10, string query = "*", string sort = "", ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.BeforeGetAsync(from, size, query, sort, user);
            throw new UnauthorizedAccessException("You are not allowed to perform this action");
        }
        public override Task<DBBase<RFQ>> AfterGetByIdAsync(DBBase<RFQ> result, string id, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.AfterGetByIdAsync(result, id, user);
            if (result.CreatedBy != user.Identity.Name) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.AfterGetByIdAsync(result, id, user);
        }
        public override Task<(DBBase<RFQ>, DBBaseLog<RFQ>)> ToUpdate(DBBase<RFQ> item, DBBaseLog<RFQ> logItem, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.ToUpdate(item, logItem, user);
            if(item.CreatedBy != user.Identity.Name) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.ToUpdate(item, logItem, user);
        }

    }
}
