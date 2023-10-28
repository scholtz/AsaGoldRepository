﻿using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.RestDWH
{
    /// <summary>
    /// Email validation form
    /// </summary>
    [RestDWHEntity(
        name: "AccountEmail",
        endpointGet: "api/v1/account-email",
        endpointGetById: "api/v1/account-email/{id}",
        endpointPut: "api/v1/account-email",
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
        /// Version of GDPR consent
        /// </summary>
        public string Consent { get; set; }
        /// <summary>
        /// Agrees to receive marketing promotions
        /// </summary>
        public bool MarketingConsent { get; set; } = false;
    }
}
