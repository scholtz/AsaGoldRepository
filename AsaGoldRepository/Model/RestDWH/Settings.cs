using RestDWH.Base.Attributes;

namespace AsaGoldRepository.Model.DWH
{
    /// <summary>
    /// Settings
    /// 
    /// Id is Account address
    /// </summary>
    [RestDWHEntity(
        name: "Settings", 
        endpointGet: "api/v1/user-setting",
        endpointUpsert: "api/v1/user-setting/{id}",
        endpointPatch: "api/v1/user-setting/{id}"
        )]
    public class Settings
    {
        public string Language { get; set; }
    }
}
