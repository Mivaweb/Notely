using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Events
{
    /// <summary>
    /// Define custom application events
    /// </summary>
    public class NotelyEvents: ApplicationEventHandler
    {
        // Fire when application is started
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // Get database context
            var dbContext = applicationContext.DatabaseContext;
            var db = new DatabaseSchemaHelper(dbContext.Database, applicationContext.ProfilingLogger.Logger, dbContext.SqlSyntax);

            // Check if table exists
            if (!db.TableExist("notelyComments"))
                db.CreateTable<Comment>(false); // Create table

            // Add events to cleanup commentor comments
            ContentService.Deleted += ContentService_Deleted;
            ContentService.EmptiedRecycleBin += ContentService_EmptiedRecycleBin;
        }

        // Event fires when clicking on Empty recycle bin
        private void ContentService_EmptiedRecycleBin(IContentService sender, Umbraco.Core.Events.RecycleBinEventArgs e)
        {
            if(e.IsContentRecycleBin)
            {
                using (CommentsRepository repo = new CommentsRepository())
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
            using (CommentsRepository repo = new CommentsRepository())
            {
                foreach (var node in e.DeletedEntities)
                {
                    repo.DeleteByContent(node.Id);
                }
            }
        }
    }
}
