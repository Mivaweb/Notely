using Newtonsoft.Json;
using System;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a CommentViewModel
    /// </summary>
    public class CommentViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignedTo")]
        public UserViewModel AssignedTo { get; set; }

        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("contentProperty")]
        public ContentPropertyViewModel ContentProperty { get; set; }

        public CommentViewModel()
        {
            Id = -1;
            Type = 0;
            Title = Description = "";
            AssignedTo = null;
            State = false;
            CreateDate = DateTime.Now;
            ContentProperty = null;
        }
    }
}