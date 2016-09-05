using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a CommentStateViewModel
    /// </summary>
    public class CommentStateViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        public CommentStateViewModel()
        {
            Id = -1;
            Title = Color = "";
        }
    }
}