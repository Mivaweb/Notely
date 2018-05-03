using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Implement the Note DTO
    /// </summary>
    [TableName("notelyNotes")]
    [PrimaryKey("id", autoIncrement = true)]
    [ExplicitColumns]
    public class Note
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("contentId")]
        public int ContentId { get; set; }

        [Column("propertyTypeId")]
        public int PropertyTypeId { get; set; }

        [Column("type")]
        [ForeignKey(typeof(NoteType), Column = "id", Name = "FK_Note_NoteType")]
        public int Type { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("assignedTo")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? AssignedTo { get; set; }

        [Column("state")]
        [ForeignKey(typeof(NoteState), Column = "id", Name = "FK_Note_NoteState")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? State { get; set; }

        [Column("priority")]
        public int Priority { get; set; }

        [Column("createDate")]
        public DateTime CreateDate { get; set; }

        [Ignore]
        public virtual NoteType NoteType { get; set; }

        [Ignore]
        public virtual NoteState NoteState { get; set; }
    }
}
