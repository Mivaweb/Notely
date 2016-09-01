using Newtonsoft.Json;

namespace Notely.Core.Models
{
    /// <summary>
    /// Defines a user object ( Umbraco backoffice user )
    /// </summary>
    public class User
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
