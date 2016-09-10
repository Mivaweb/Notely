using System.Collections.Generic;

using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a BackOfficeProperty
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
        /// Holds the limit prevalue of the property editor
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// List of notes
        /// </summary>
        [JsonProperty("notes")]
        public List<NoteViewModel> Notes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BackOfficeProperty()
        {
            Id = -1;
            Limit = 1;
            Alias = "";
            Name = "";
            Notes = new List<NoteViewModel>();
        }
    }
}