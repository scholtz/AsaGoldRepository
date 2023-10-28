using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.RestDWH
{
    /// <summary>
    /// Email validation form
    /// </summary>
    [RestDWHEntity(
        name: "EmailValidation",
        endpointGetById: "api/v1/email-validation/{id}",
        endpointPut: "api/v1/email-validation"
        )]
    public class EmailValidation
    {
        /// <summary>
        /// User's account address
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Email to be validated
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Version of GDPR consent
        /// </summary>
        public string Consent { get; set; }
        /// <summary>
        /// Agrees to receive marketing promotions
        /// </summary>
        public bool MarketingConsent { get; set; } = false;
    }
}
