using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a CommentTypeViewModel
    /// </summary>
    public class CommentTypeViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("canAssign")]
        public bool CanAssign { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentTypeViewModel()
        {
            Id = -1;
            Title = Icon = "";
            CanAssign = false;
        }
    }
}