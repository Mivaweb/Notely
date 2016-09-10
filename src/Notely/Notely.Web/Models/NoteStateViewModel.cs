using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a NoteStateViewModel
    /// </summary>
    public class NoteStateViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteStateViewModel()
        {
            Id = -1;
            Title = "";
        }
    }
}