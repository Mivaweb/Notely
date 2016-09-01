using System;
using System.Linq;

using Umbraco.Web;

using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Defines extension methods for the <see cref="Comment"/> and <see cref="CommentViewModel"/> classes
    /// </summary>
    public static class CommentExtensions
    {
        /// <summary>
        /// Convert a <see cref="CommentViewModel"/> to a <see cref="Comment"/> object
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="commentVm"></param>
        /// <returns></returns>
        public static Comment Convert(this Comment comment, CommentViewModel commentVm)
        {
            /* 
             * Check if the ConentProperty is defined => 
             * without it will be hard to know for which content node and property
             * we are talking about.
             */
            if (commentVm.ContentProperty == null)
                throw new ArgumentNullException("ContentProperty of the comment is not defined.");

            var _content = UmbracoContext.Current.Application.Services.ContentService.GetById(commentVm.ContentProperty.NodeId);

            /* 
             * 1) We first check if we can find the property type based on the property data id.
             *    If we can't find the type then there is a new version published of the property value.
             * 2) Next step is to find the type based on the alias of the property.
             * 
             */
            var _property = _content.Properties.FirstOrDefault(
                    p => p.Id == commentVm.ContentProperty.PropertyDataId || p.Alias.Equals(commentVm.ContentProperty.PropertyTypeAlias)
                );

            // Create a new Comment object
            var _comment = new Comment()
            {
                CreateDate = commentVm.CreateDate,
                Description = commentVm.Description,
                Id = commentVm.Id,
                State = commentVm.State,
                Title = commentVm.Title,
                Type = commentVm.Type,
                ContentId = commentVm.ContentProperty.NodeId,
                PropertyTypeId = _property != null ? _property.PropertyType.Id : -1
            };

            // Only if it has a assignee and its not an info note
            if (commentVm.AssignedTo != null && commentVm.Type > 0)
            {
                _comment.AssignedTo = commentVm.AssignedTo.Id;
            }

            return _comment;
        }

        /// <summary>
        /// Convert a <see cref="Comment"/> to a <see cref="CommentViewModel"/> object
        /// </summary>
        /// <param name="commentVm"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static CommentViewModel Convert(this CommentViewModel commentVm, Comment comment)
        {
            var userVm = new UserViewModel();

            // Create a new CommentViewModel
            return new CommentViewModel()
            {
                AssignedTo =
                    userVm.Convert(
                        comment.AssignedTo.HasValue ?
                        UmbracoContext.Current.Application.Services.UserService.GetUserById(comment.AssignedTo.Value) :
                        null),
                CreateDate = comment.CreateDate,
                Description = comment.Description,
                Id = comment.Id,
                State = comment.State,
                Title = comment.Title,
                Type = comment.Type
            };
        }
    }
}