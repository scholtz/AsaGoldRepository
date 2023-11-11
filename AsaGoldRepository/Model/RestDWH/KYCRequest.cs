using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.RestDWH
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
}
