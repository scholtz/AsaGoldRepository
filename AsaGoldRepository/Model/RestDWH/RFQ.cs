using Microsoft.AspNetCore.JsonPatch;
using RestDWH.Base.Attributes;
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
        name: "RFQ",
        events: typeof(RFQEvent),
        endpointGetById: "api/v1/rfq/{id}",
        endpointUpsert: "api/v1/rfq/{id}",
        endpointPatch: "api/v1/rfq/{id}"
        )]
    public class RFQ
    {
        /// <summary>
        /// User address to fund account when money arrive
        /// </summary>
        public string User { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Quote { get; set; }
        public bool Confirmed { get; set; } = false;
        public bool Funded { get; set; } = false;

    }

    public class RFQEvent : RestDWH.Base.Model.RestDWHEvents<RFQ>
    {
        public override Task<string> BeforeGetByIdAsync(string id, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (!Program.Admins.Contains(user.Identity.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforeGetByIdAsync(id, user);
        }
        public override Task<(string id, RFQ data)> BeforeUpsertAsync(string id, RFQ data, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (!Program.Admins.Contains(user.Identity.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforeUpsertAsync(id, data, user);
        }
        public override Task<(string id, JsonPatchDocument<RFQ> data)> BeforePatchAsync(string id, JsonPatchDocument<RFQ> data, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (!Program.Admins.Contains(user.Identity.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforePatchAsync(id, data, user);
        }
    }
}
