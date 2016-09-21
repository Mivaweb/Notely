using Newtonsoft.Json;
using System;

namespace Notely.Web.Models
{
    /// <summary>
    /// Implements a NoteCommentViewModel
    /// </summary>
    public class NoteCommentViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("user")]
        public UserViewModel User { get; set; }

        [JsonProperty("noteId")]
        public int NoteId { get; set; }

        [JsonProperty("datestamp")]
        public string Datestamp { get; set; }

        [JsonProperty("logType")]
        public string LogType { get; set; }

        [JsonProperty("logComment")]
        public string LogComment { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteCommentViewModel()
        {
            Id = -1;
            User = null;
            NoteId = -1;
            Datestamp = umbraco.library.FormatDateTime(DateTime.Now.ToString(), "dd MMM yyyy HH:mm:ss");
            LogType = "";
            LogComment = "";
        }
    }
}