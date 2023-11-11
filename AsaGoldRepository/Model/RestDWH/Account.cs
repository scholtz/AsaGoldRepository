using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.RestDWH
{
    /// <summary>
    /// Account
    /// 
    /// Id is user account
    /// </summary>
    [RestDWHEntity(
        name: "Account",
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
}
