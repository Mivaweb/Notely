using System;
using System.Linq;

using Umbraco.Web;

using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="Comment"/> and <see cref="CommentViewModel"/> classes
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
             * Check if the ContentProperty is defined => 
             * without it will be hard to know for which content node and property
             * we are talking about.
             */
            if (commentVm.ContentProperty == null)
                throw new ArgumentNullException("ContentProperty of the comment is not defined.");

            var _content = UmbracoContext.Current.Application.Services.ContentService.GetById(commentVm.ContentProperty.ContentId);

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
                Title = commentVm.Title,
                Type = commentVm.Type.Id,
                ContentId = commentVm.ContentProperty.ContentId,
                PropertyTypeId = _property != null ? _property.PropertyType.Id : -1
            };

            // Only if it has a assignee and its not an info note
            if (commentVm.AssignedTo != null && commentVm.Type.CanAssign)
            {
                _comment.AssignedTo = commentVm.AssignedTo.Id;
            }

            // If the comment type can not have an assignee make sure its set to null
            if(commentVm.Type.CanAssign)
            {
                _comment.State = commentVm.State.Id;
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
            var _content = UmbracoContext.Current.Application.Services.ContentService.GetById(comment.ContentId);

            /* 
             * 1) We first check if we can find the property type based on the property data id.
             *    If we can't find the type then there is a new version published of the property value.
             * 2) Next step is to find the type based on the alias of the property.
             * 
             */
            var _property = _content.Properties.FirstOrDefault(
                    p => p.PropertyType.Id == comment.PropertyTypeId
                );

            var userVm = new UserViewModel();
            var commentTypeVm = new CommentTypeViewModel();
            var commentStateVm = new CommentStateViewModel();

            var contentProperty = new ContentPropertyViewModel() {
                ContentId = comment.ContentId,
                ContentName = _content.Name,
                PropertyTypeAlias = _property.Alias
            };

            // Create a new CommentViewModel
            var result = new CommentViewModel()
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
                Type = commentTypeVm.Convert(comment.CommentType),
                ContentProperty = contentProperty,
                Closed = comment.Closed
            };

            return result;
        }
    }
}