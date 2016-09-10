using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implment the NoteType DTO
    /// </summary>
    [TableName("notelyNoteTypes")]
    [PrimaryKey("id", autoIncrement = true)]
    public class NoteType
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("icon")]
        public string Icon { get; set; }

        [Column("canAssign")]
        public bool CanAssign { get; set; }
    }
}
