using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement the NoteComment DTO
    /// </summary>
    [TableName("notelyNoteComments")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class NoteComment
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("userId")]
        public int UserId { get; set; }

        [Column("noteId")]
        [ForeignKey(typeof(Note), Column = "id", Name = "FK_NoteComment_Note")]
        public int NoteId { get; set; }

        [Column("datestamp")]
        public DateTime Datestamp { get; set; }

        [Column("logType")]
        public string LogType { get; set; }

        [Column("logComment")]
        public string LogComment { get; set; }
    }
}
