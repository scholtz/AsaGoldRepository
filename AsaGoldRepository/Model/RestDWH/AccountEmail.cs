using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.RestDWH
{
    /// <summary>
    /// Email validation form
    /// </summary>
    [RestDWHEntity(
        name: "AccountEmail",
        endpointGet: "api/v1/account-email",
        endpointGetById: "api/v1/account-email/{id}",
        endpointUpsert: "api/v1/account-email",
        endpointDelete: "api/v1/account-email"
        )]
    public class AccountEmail
    {
        /// <summary>
        /// User's account address
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Email to be validated
        /// </summary>
        public string PrimaryEmail { get; set; }
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
    }
}
