using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Defines the CommentType DTO
    /// </summary>
    [TableName("notelyCommentTypes")]
    [PrimaryKey("Id", autoIncrement = true)]
    public class CommentType
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
