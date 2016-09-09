using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement the CommentState DTO
    /// </summary>
    [TableName("notelyNoteStates")]
    [PrimaryKey("id", autoIncrement = true)]
    public class CommentState
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }
    }
}
