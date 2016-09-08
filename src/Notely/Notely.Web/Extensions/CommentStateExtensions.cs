using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension method for the <see cref="CommentStateViewModel"/> object
    /// </summary>
    public static class CommentStateExtensions
    {
        /// <summary>
        /// Converts a <see cref="CommentState"/> object to a <see cref="CommentStateViewModel"/> object
        /// </summary>
        /// <param name="commentStateVm"></param>
        /// <param name="commentState"></param>
        /// <returns></returns>
        public static CommentStateViewModel Convert(this CommentStateViewModel commentStateVm, CommentState commentState)
        {
            if (commentState == null)
                return null;

            return new CommentStateViewModel()
            {
                Id = commentState.Id,
                Title = commentState.Title
            };
        }
    }
}