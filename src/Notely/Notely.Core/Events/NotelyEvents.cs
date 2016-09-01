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
        }

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
