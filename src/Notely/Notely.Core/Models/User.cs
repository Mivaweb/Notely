using Newtonsoft.Json;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement a user object ( Umbraco backoffice user )
    /// </summary>
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
