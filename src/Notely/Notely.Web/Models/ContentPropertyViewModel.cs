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
        [JsonProperty("nodeId")]
        public int NodeId { get; set; }

        [JsonProperty("propertyDataId")]
        public int PropertyDataId { get; set; }

        [JsonProperty("propertyTypeAlias")]
        public string PropertyTypeAlias { get; set; }
    }
}