using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a UserViewModel
    /// </summary>
    public sealed class UserViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}