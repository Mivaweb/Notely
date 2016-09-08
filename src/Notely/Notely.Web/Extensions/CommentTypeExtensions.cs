using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="CommentTypeViewModel"/>
    /// </summary>
    public static class CommentTypeExtensions
    {
        /// <summary>
        /// Converts a <see cref="CommentType"/> object to <see cref="CommentTypeViewModel"/> object
        /// </summary>
        /// <param name="commentTypeVm"></param>
        /// <param name="commentType"></param>
        /// <returns></returns>
        public static CommentTypeViewModel Convert(this CommentTypeViewModel commentTypeVm, CommentType commentType)
        {
            if (commentType == null)
                return null;

            return new CommentTypeViewModel()
            {
                Id = commentType.Id,
                Title = commentType.Title,
                Icon = commentType.Icon,
                CanAssign = commentType.CanAssign
            };
        }

        /// <summary>
        /// Converts a <see cref="CommentTypeViewModel"/> object to a <see cref="CommentType"/> object
        /// </summary>
        /// <param name="commentType"></param>
        /// <param name="commentTypeVm"></param>
        /// <returns></returns>
        public static CommentType Convert(this CommentType commentType, CommentTypeViewModel commentTypeVm)
        {
            return new CommentType() {

                Id = commentTypeVm.Id,
                Title = commentTypeVm.Title,
                Icon = commentTypeVm.Icon,
                CanAssign = commentTypeVm.CanAssign

            };
        }
    }
}