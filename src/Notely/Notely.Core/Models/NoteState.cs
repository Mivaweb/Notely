using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement the NoteState DTO
    /// </summary>
    [TableName("notelyNoteStates")]
    [PrimaryKey("id", autoIncrement = true)]
    public class NoteState
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}
