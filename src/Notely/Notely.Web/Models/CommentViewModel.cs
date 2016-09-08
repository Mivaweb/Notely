using Newtonsoft.Json;
using System;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a CommentViewModel
    /// </summary>
    public class CommentViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public CommentTypeViewModel Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignedTo")]
        public UserViewModel AssignedTo { get; set; }

        [JsonProperty("state")]
        public CommentStateViewModel State { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("contentProperty")]
        public ContentPropertyViewModel ContentProperty { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentViewModel()
        {
            Id = -1;
            Type = null;
            Title = Description = "";
            AssignedTo = null;
            State = null;
            CreateDate = DateTime.Now;
            ContentProperty = null;
            Closed = false;
        }
    }
}