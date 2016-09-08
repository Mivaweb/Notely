using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a ContentPropertyViewModel
    /// </summary>
    public class ContentPropertyViewModel
    {
        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        [JsonProperty("contentName")]
        public string ContentName { get; set; }

        [JsonProperty("propertyDataId")]
        public int PropertyDataId { get; set; }

        [JsonProperty("propertyTypeAlias")]
        public string PropertyTypeAlias { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContentPropertyViewModel()
        {
            ContentId = -1;
            ContentName = "";
            PropertyDataId = -1;
            PropertyTypeAlias = "";
        }
    }
}