using Microsoft.AspNetCore.JsonPatch;
using RestDWH.Base.Attributes;
using RestDWH.Base.Model;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AsaGoldRepository.Model.DWH
{
    /// <summary>
    /// Account
    /// 
    /// Id is user account
    /// </summary>
    [RestDWHEntity(
        name: "Account",
        events: typeof(AccountEvents),
        endpointGetById: "api/v1/user-account/{id}",
        endpointUpsert: "api/v1/user-account/{id}",
        endpointPatch: "api/v1/user-account/{id}",
        endpointDelete: "api/v1/user-account"
        )]
    public class Account
    {
        /// <summary>
        /// Email to be validated
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Version of terms and conditions
        /// </summary>
        public string TermsAndConditions { get; set; }
        /// <summary>
        /// Version of GDPR consent
        /// </summary>
        public string Gdpr { get; set; }
        /// <summary>
        /// Agrees to receive marketing promotions
        /// </summary>
        public bool MarketingConsent { get; set; } = false;
        /// <summary>
        /// Last email validation time
        /// </summary>
        public DateTimeOffset? LastEmailValidationTime { get; set; } = null;
        /// <summary>
        /// Funded amount
        /// </summary>
        public ulong Funded { get; set; } = 0;
        /// <summary>
        /// Last approved KYC request
        /// </summary>
        public string? ApprovedKYCRequestId { get; set; }
        /// <summary>
        /// Last KYC change request
        /// </summary>
        public string? LastKYCRequestId { get; set; }
    }
    public class AccountEvents : RestDWH.Base.Model.RestDWHEvents<RFQ>
    {
        public override Task<string> BeforeGetByIdAsync(string id, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.BeforeGetByIdAsync(id, user); // ok
            if (id != user.Identity.Name) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforeGetByIdAsync(id, user); // ok
        }
        public override Task<(string id, RFQ data)> BeforeUpsertAsync(string id, RFQ data, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.BeforeUpsertAsync(id, data, user);
            if (id != user.Identity.Name) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforeUpsertAsync(id, data, user);
        }
        public override Task<(string id, JsonPatchDocument<RFQ> data)> BeforePatchAsync(string id, JsonPatchDocument<RFQ> data, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.BeforePatchAsync(id, data, user);
            if (id != user.Identity.Name) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            return base.BeforePatchAsync(id, data, user);
        }
        public override Task<string> BeforeDeleteAsync(string id, ClaimsPrincipal? user = null)
        {
            if (string.IsNullOrEmpty(user?.Identity?.Name)) throw new UnauthorizedAccessException("You are not allowed to perform this action");
            if (Program.Admins.Contains(user.Identity.Name)) return base.BeforeDeleteAsync(id, user);
            throw new UnauthorizedAccessException("You are not allowed to perform this action");
        }
    }
}
