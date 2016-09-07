using System.Collections.Generic;

using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a BackOfficeProperty
    /// </summary>
    public class BackOfficeProperty
    {
        /// <summary>
        /// Property type Id
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Property type alias
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Property name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// List of comments
        /// </summary>
        [JsonProperty("comments")]
        public List<BackOfficeComment> Comments { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackOfficeProperty()
        {
            Id = -1;
            Alias = "";
            Name = "";
            Comments = new List<BackOfficeComment>();
        }
    }
}