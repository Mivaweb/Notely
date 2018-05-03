using System;

using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

using Notely.Core.Models;

namespace Notely.Core.Migrations
{
    /// <summary>
    /// Define migrations from version 1.x to 2.0 for Notely
    /// </summary>
    [Migration("2.0", 1, "Notely")]
    public class MigrateV20 : MigrationBase
    {
        private readonly UmbracoDatabase _database = ApplicationContext.Current.DatabaseContext.Database;
        private readonly DatabaseSchemaHelper _schemaHelper;

        public MigrateV20(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
            _schemaHelper = new DatabaseSchemaHelper(_database, logger, sqlSyntax);
        }

        /// <summary>
        /// Add changes
        /// </summary>
        public override void Up()
        {
            // Check if the table notelyNotes exits
            if(_schemaHelper.TableExist("notelyNotes"))
            {
                Alter.Table("notelyNotes").AddColumn("priority").AsInt32().NotNullable().WithDefaultValue(1);
            }
        }

        /// <summary>
        /// Remove changes
        /// </summary>
        public override void Down()
        {
            // Check if the table notelyNotes exits
            if (_schemaHelper.TableExist("notelyNotes"))
            {
                Delete.Column("priority").FromTable("notelyNotes");
            }
        }
    }
}
