namespace AsaGoldRepository.Model.Config
{
    public class RepositoryOptions
    {
        /// <summary>
        /// ARC76 Account
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Elastic search URI
        /// </summary>
        public string ElasticURI { get; internal set; }
        /// <summary>
        /// Elastic search Api Key
        /// </summary>
        public string ApiKey { get; internal set; }
    }
}
