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
        public string GDPR { get; set; }
        /// <summary>
        /// Agrees to receive marketing promotions
        /// </summary>
        public bool MarketingConsent { get; set; } = false;
        /// <summary>
        /// Last email validation time
        /// </summary>
        public DateTimeOffset? LastEmailValidationTime { get; set; } = null;
    }
}
