using Newtonsoft.Json;
using System;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a NoteViewModel
    /// </summary>
    public class NoteViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public NoteTypeViewModel Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("assignedTo")]
        public UserViewModel AssignedTo { get; set; }

        [JsonProperty("state")]
        public NoteStateViewModel State { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("contentProperty")]
        public ContentPropertyViewModel ContentProperty { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteViewModel()
        {
            Id = -1;
            Type = null;
            Title = Description = "";
            AssignedTo = null;
            State = null;
            CreateDate = DateTime.Now;
            ContentProperty = null;
            Priority = (int)Notely.Core.Enum.NotePriority.Low;
        }
    }
}