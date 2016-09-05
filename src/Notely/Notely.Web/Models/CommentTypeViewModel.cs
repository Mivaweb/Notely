using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notely.Web.Models
{
    /// <summary>
    /// Defines a CommentTypeViewModel
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

        public CommentTypeViewModel()
        {
            Id = -1;
            Title = Icon = "";
            CanAssign = false;
        }
    }
}