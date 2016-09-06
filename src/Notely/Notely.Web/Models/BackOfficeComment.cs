using System;

using Newtonsoft.Json;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a BackOfficeComment
    /// </summary>
    public class BackOfficeComment
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

        /// <summary>
        /// Constructor
        /// </summary>
        public BackOfficeComment()
        {
            Id = -1;
            Type = null;
            Title = Description = "";
            AssignedTo = null;
            State = null;
            CreateDate = DateTime.Now;
        }
    }
}