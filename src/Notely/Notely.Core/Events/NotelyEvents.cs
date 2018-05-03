using System;
using System.Linq;
using Semver;

using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web;

using Notely.Core.Models;


namespace Notely.Core.Events
{
    /// <summary>
    /// Implement custom application events
    /// </summary>
    public class NotelyEvents: ApplicationEventHandler
    {
        // Fired when application is started
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Install notely table
            InstallTables(applicationContext);            

            // Handle migrations
            HandleV20Migrations();

            // Add events to cleanup notely notes
            ContentService.Deleted += ContentService_Deleted;
            ContentService.EmptiedRecycleBin += ContentService_EmptiedRecycleBin;
        }

        // Install notely database tables
        private static void InstallTables(ApplicationContext applicationContext)
        {
            // Get database context
            var dbContext = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, applicationContext.ProfilingLogger.Logger, dbContext.SqlSyntax);

            // Check if table 'notelyNoteStates' exists
            // If it doesn't exists create the new table and add the default records
            if (!db.TableExist("notelyNoteStates"))
            {
                db.CreateTable<NoteState>(false);
                dbContext.Database.Insert(new NoteState() { Id = 1, Title = "Open" });
                dbContext.Database.Insert(new NoteState() { Id = 2, Title = "Approved" });
                dbContext.Database.Insert(new NoteState() { Id = 3, Title = "Pending" });
                dbContext.Database.Insert(new NoteState() { Id = 4, Title = "Done" });
            }

            // Check if table 'notelyNoteTypes' exits
            // If it doesn't exists create the new table and add the default records
            if (!db.TableExist("notelyNoteTypes"))
            {
                db.CreateTable<NoteType>(false);
                dbContext.Database.Insert(new NoteType() { CanAssign = false, Icon = "icon-info", Id = 1, Title = "Info" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-pushpin", Id = 2, Title = "Todo" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-brackets", Id = 3, Title = "Development" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-split-alt", Id = 4, Title = "Test" });
            }

            // Check if table 'notelyNoteComments' exists
            // If it doesn't exists create the new table
            if (!db.TableExist("notelyNoteComments"))
                db.CreateTable<NoteComment>(false);

            // Check if table 'notelyNotes' exits
            // If it not exists create the new table
            if (!db.TableExist("notelyNotes"))
                db.CreateTable<Note>(false);
        }

        // Handle migrations to version 2.0
        private static void HandleV20Migrations()
        {
            const string productName = "Notely";
            var currentVersion = new SemVersion(0, 0, 0);

            // Get all migrations
            var migrations = ApplicationContext.Current.Services.MigrationEntryService.GetAll(productName);

            // Get the latest migration
            var latestMigration = migrations.OrderByDescending(x => x.Version).FirstOrDefault();

            if (latestMigration != null)
                currentVersion = latestMigration.Version;

            var targetVersion = new SemVersion(2, 0);

            if (targetVersion == currentVersion)
                return;

            var migrationsRunner = new MigrationRunner(
                    ApplicationContext.Current.Services.MigrationEntryService,
                    ApplicationContext.Current.ProfilingLogger.Logger,
                    currentVersion,
                    targetVersion,
                    productName
                );

            try
            {
                migrationsRunner.Execute(UmbracoContext.Current.Application.DatabaseContext.Database);
            }
            catch (Exception e)
            {
                LogHelper.Error<MigrationRunner>("Error running Notely migrations", e);
            }
        }

        // Event fires when clicking on Empty recycle bin
        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            Notely.Core.Services.ServiceContext Services = new Core.Services.ServiceContext();

            // Check if we are in the content recycle bin
            if(e.IsContentRecycleBin)
            {
                foreach(var node in e.AllPropertyData)
                {
                    var notes = Services.NoteService.GetAllByContent(node.Key);

                    foreach (var note in notes)
                    {
                        // Delete comments
                        Services.NoteCommentService.DeleteByNoteId(note.Id);

                        // Delete note
                        Services.NoteService.Delete(note.Id);
                    }
                }
            }
        }

        // Events fires when deleting a node from the recycle bin
        private void ContentService_Deleted(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            Notely.Core.Services.ServiceContext Services = new Core.Services.ServiceContext();

            foreach (var node in e.DeletedEntities)
            {
                var notes = Services.NoteService.GetAllByContent(node.Id);

                foreach (var note in notes)
                {
                    // Delete comments
                    Services.NoteCommentService.DeleteByNoteId(note.Id);

                    // Delete note
                    Services.NoteService.Delete(note.Id);
                }
            }
        }
    }
}
