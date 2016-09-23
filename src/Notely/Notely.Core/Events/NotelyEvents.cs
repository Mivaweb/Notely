using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;
using Notely.Core.Services;

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
            
            // Add events to cleanup notely notes
            ContentService.Deleted += ContentService_Deleted;
            ContentService.EmptiedRecycleBin += ContentService_EmptiedRecycleBin;
        }

        // Event fires when clicking on Empty recycle bin
        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            // Check if we are in the content recycle bin
            if(e.IsContentRecycleBin)
            {
                using (var repo = new NotesRepository())
                {
                    foreach (var node in e.AllPropertyData)
                    {
                        var notes = repo.GetAllByContent(node.Key);

                        foreach(var note in notes)
                        {
                            
                            //NoteCommentService.DeleteByNote(note.Id);

                            //repo.Delete(note.Id);
                        }
                    }
                }
            }
        }

        // Events fires when deleting a node from the recycle bin
        private void ContentService_Deleted(IContentService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IContent> e)
        {
            using (var repo = new NotesRepository())
            {
                foreach (var node in e.DeletedEntities)
                {
                    var notes = repo.GetAllByContent(node.Id);

                    foreach (var note in notes)
                    {
                        //NoteCommentService.DeleteByNote(note.Id);

                        //repo.Delete(note.Id);
                    }
                }
            }
        }
    }
}
