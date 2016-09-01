using System;

using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace Notely.Core.Models
{
    /// <summary>
    /// Defins the Comment DTO
    /// </summary>
    [TableName("notelyComments")]
    [PrimaryKey("Id", autoIncrement = true)]
    [ExplicitColumns]
    public class Comment
    {
        [Column("id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column("contentId")]
        public int ContentId { get; set; }

        [Column("propertyTypeId")]
        public int PropertyTypeId { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("assignedTo")]
        [NullSetting(NullSetting = NullSettings.Null)]
        public int? AssignedTo { get; set; }

        [Column("state")]
        public bool State { get; set; }

        [Column("createDate")]
        public DateTime CreateDate { get; set; }
    }
}
