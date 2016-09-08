using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement the DataType object
    /// </summary>
    public class DataType
    {
        [JsonProperty("guid")]
        public Guid Guid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("propertyTypeAlias")]
        public string PropertyTypeAlias { get; set; }

        [JsonProperty("prevalues")]
        public IDictionary<string, object> PreValues { get; set; }

        [JsonProperty("view")]
        public string View { get; set; }
    }
}
