using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

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
            // Get database context
            var dbContext = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, applicationContext.ProfilingLogger.Logger, dbContext.SqlSyntax);

            // Check if table 'notelyNoteStates' exists
            // If it not exists create the new table and add the default records
            if (!db.TableExist("notelyNoteStates"))
            {
                db.CreateTable<NoteState>(false);
                dbContext.Database.Insert(new NoteState() { Id = 1, Title = "Open" });
                dbContext.Database.Insert(new NoteState() { Id = 2, Title = "Approved" });
                dbContext.Database.Insert(new NoteState() { Id = 3, Title = "Pending" });
                dbContext.Database.Insert(new NoteState() { Id = 4, Title = "Done" });
            }

            // Check if table 'notelyNoteTypes' exits
            // If it not exists create the new table and add the default records
            if (!db.TableExist("notelyNoteTypes"))
            {
                db.CreateTable<NoteType>(false);
                dbContext.Database.Insert(new NoteType() { CanAssign = false, Icon = "icon-info", Id = 1, Title = "Info" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-pushpin", Id = 2, Title = "Todo" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-brackets", Id = 3, Title = "Development" });
                dbContext.Database.Insert(new NoteType() { CanAssign = true, Icon = "icon-split-alt", Id = 4, Title = "Test" });
            }

            // Check if table 'notelyNotes' exits
            // If it not exists create the new table
            if (!db.TableExist("notelyNotes"))
                db.CreateTable<Note>(false);
            
            // Add events to cleanup notely comments
            ContentService.Deleted += ContentService_Deleted;
            ContentService.EmptiedRecycleBin += ContentService_EmptiedRecycleBin;
        }

        // Event fires when clicking on Empty recycle bin
        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            // Check if we are in the content recycle bin
            if(e.IsContentRecycleBin)
            {
                using (NotesRepository repo = new NotesRepository())
                {
                    foreach (var node in e.AllPropertyData)
                    {
                        repo.DeleteByContent(node.Key);
                    }
                }
            }
        }

        // Events fires when deleting a node from the recycle bin
        private void ContentService_Deleted(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            using (NotesRepository repo = new NotesRepository())
            {
                foreach (var node in e.DeletedEntities)
                {
                    repo.DeleteByContent(node.Id);
                }
            }
        }
    }
}
