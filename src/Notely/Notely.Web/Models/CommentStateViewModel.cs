using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a CommentStateViewModel
    /// </summary>
    public class CommentStateViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentStateViewModel()
        {
            Id = -1;
            Title = "";
        }
    }
}