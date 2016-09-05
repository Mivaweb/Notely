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
            if (!db.TableExist("notelyCommentStates"))
            {
                db.CreateTable<CommentState>(false);
                dbContext.Database.Insert(new CommentState() { Id = 1, Title = "Open", Color = "" });
                dbContext.Database.Insert(new CommentState() { Id = 2, Title = "Approved", Color = "" });
                dbContext.Database.Insert(new CommentState() { Id = 3, Title = "Pending", Color = "" });
                dbContext.Database.Insert(new CommentState() { Id = 4, Title = "Done", Color = "" });
            }
              
            if (!db.TableExist("notelyCommentTypes"))
            {
                db.CreateTable<CommentType>(false);
                dbContext.Database.Insert(new CommentType() { CanAssign = false, Icon = "icon-info", Id = 1, Title = "Info" });
                dbContext.Database.Insert(new CommentType() { CanAssign = true, Icon = "icon-pushpin", Id = 2, Title = "Todo" });
                dbContext.Database.Insert(new CommentType() { CanAssign = true, Icon = "icon-brackets", Id = 3, Title = "Development" });
                dbContext.Database.Insert(new CommentType() { CanAssign = true, Icon = "icon-split-alt", Id = 4, Title = "Test" });
            }
                
            if (!db.TableExist("notelyComments"))
                db.CreateTable<Comment>(false);

            
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
