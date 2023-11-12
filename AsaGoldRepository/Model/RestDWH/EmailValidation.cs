using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.DWH
{
    /// <summary>
    /// Email validation form
    /// </summary>
    [RestDWHEntity(
        name: "EmailValidation",
        endpointGetById: "api/v1/email-validation/{id}",
        endpointUpsert: "api/v1/email-validation",
        endpointPatch: "api/v1/email-validation"
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
        /// Indicates if this email validation code has benn already used
        /// </summary>
        public bool Used { get; set; } = false;
        /// <summary>
        /// Indicates the transaction id of the funding tx
        /// </summary>
        public string? FundTransaction { get; set; } = null;
    }
}
