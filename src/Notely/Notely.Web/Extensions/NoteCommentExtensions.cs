using System;
using System.Linq;

using Umbraco.Web;

using Notely.Core.Models;
using Notely.Web.Models;
using Notely.Core.Services;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="NoteComment"/> and <see cref="NoteCommentViewModel"/> classes
    /// </summary>
    public static class NoteCommentExtensions
    {
        /// <summary>
        /// Converts a <see cref="NoteCommentViewModel"/> object to a <see cref="NoteComment"/> object
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="commentVm"></param>
        /// <returns></returns>
        public static NoteComment Convert(this NoteComment comment, NoteCommentViewModel commentVm)
        {
            var result = new NoteComment()
            {
                Id = commentVm.Id,
                Datestamp = DateTime.Now,
                LogComment = commentVm.LogComment,
                LogType = commentVm.LogType,
                NoteId = commentVm.NoteId,
                UserId = commentVm.User.Id
            };

            return result;
        }

        /// <summary>
        /// Converts a <see cref="NoteComment"/> object to a <see cref="NoteCommentViewModel"/> object
        /// </summary>
        /// <param name="commentVm"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static NoteCommentViewModel Convert(this NoteCommentViewModel commentVm, NoteComment comment)
        {
            var userVm = new UserViewModel();

            var _note = NotelyContext.Current.Services.NoteService.GetById(comment.NoteId);

            var result = new NoteCommentViewModel() {
                Id = comment.Id,
                Datestamp = umbraco.library.FormatDateTime(comment.Datestamp.ToString(), "dd MMM yyyy HH:mm:ss"),
                LogComment = comment.LogComment,
                LogType = comment.LogType,
                NoteId = comment.NoteId,
                NoteName = _note.Title,
                User = userVm.Convert(
                        UmbracoContext.Current.Application.Services.UserService.GetUserById(comment.UserId)
                    )
            };

            return result;
        }
    }
}