using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a UserViewModel
    /// </summary>
    public class UserViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserViewModel()
        {
            Id = -1;
            Name = "";
            AvatarUrl = "";
        }
    }
}