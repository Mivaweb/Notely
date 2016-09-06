using Umbraco.Web;

using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Defines extension methods for the <see cref="BackOfficeComment"/> object
    /// </summary>
    public static class BackOfficeCommentExtensions
    {
        /// <summary>
        /// Converts a <see cref="Comment"/> object to a <see cref="BackOfficeComment"/> object
        /// </summary>
        /// <param name="boComment"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static BackOfficeComment Convert(this BackOfficeComment boComment, Comment comment)
        {
            var userVm = new UserViewModel();
            var commentTypeVm = new CommentTypeViewModel();
            var commentStateVm = new CommentStateViewModel();

            // Create a new BackOfficeComment
            var result = new BackOfficeComment()
            {
                AssignedTo = comment.AssignedTo.HasValue ?
                    userVm.Convert(
                        UmbracoContext.Current.Application.Services.UserService.GetUserById(comment.AssignedTo.Value)
                    ) : null,
                CreateDate = comment.CreateDate,
                Description = comment.Description,
                Id = comment.Id,
                State = commentStateVm.Convert(comment.CommentState),
                Title = comment.Title,
                Type = commentTypeVm.Convert(comment.CommentType)
            };

            return result;
        }
    }
}