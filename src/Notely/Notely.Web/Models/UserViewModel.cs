using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a UserViewModel
    /// </summary>
    public class UserViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public UserViewModel()
        {
            Id = -1;
            Name = "";
        }
    }
}