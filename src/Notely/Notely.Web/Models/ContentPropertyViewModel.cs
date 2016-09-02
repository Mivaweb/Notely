using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a ContentPropertyViewModel
    /// </summary>
    public class ContentPropertyViewModel
    {
        [JsonProperty("contentId")]
        public int ContentId { get; set; }

        [JsonProperty("propertyDataId")]
        public int PropertyDataId { get; set; }

        [JsonProperty("propertyTypeAlias")]
        public string PropertyTypeAlias { get; set; }

        public ContentPropertyViewModel()
        {
            ContentId = -1;
            PropertyDataId = -1;
            PropertyTypeAlias = "";
        }
    }
}