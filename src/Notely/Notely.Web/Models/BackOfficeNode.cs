using System.Collections.Generic;

using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a BackOfficeNode
    /// </summary>
    public class BackOfficeNode
    {
        /// <summary>
        /// Content Id
        /// </summary>
        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        /// <summary>
        /// Content Name
        /// </summary>
        [JsonProperty("contentName")]
        public string ContentName { get; set; }

        /// <summary>
        /// List of properties
        /// </summary>
        [JsonProperty("properties")]
        public List<BackOfficeProperty> Properties { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackOfficeNode()
        {
            ContentId = -1;
            ContentName = "";
            Properties = new List<BackOfficeProperty>();
        }
    }
}