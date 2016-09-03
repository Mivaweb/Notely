using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using Newtonsoft.Json;

using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Models;

using Notely.Core.Models;
using Notely.Web.Models;
using Notely.Web.Extensions;
using Notely.Core.Persistence.Repositories;
using Umbraco.Web.Editors;

namespace Notely.Web.Controllers
{
    [IsBackOffice]
    [PluginController("Notely")]
    public class NotelyApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Get all the DataTypes from the Umbraco backend without the ones of 'Notely'
        /// </summary>
        /// <returns>A list of <see cref="DataType"/> objects</returns>
        [HttpGet]
        public IEnumerable<DataType> GetDataTypes()
        {
            return Services.DataTypeService.GetAllDataTypeDefinitions()
                .Where(d => d.PropertyEditorAlias != "Notely")
                .OrderBy(d => d.SortOrder)
                .Select(d => new DataType()
                {
                    Guid = d.Key,
                    Name = d.Name,
                    PropertyTypeAlias = d.PropertyEditorAlias
                });
        }

        /// <summary>
        /// Get the DataType object
        /// </summary>
        /// <param name="guid">Key</param>
        /// <returns>A <see cref="DataType"/> object</returns>
        [HttpGet]
        public DataType GetDataType(Guid guid)
        {
            var dataTypeDef = Services.DataTypeService.GetDataTypeDefinitionById(guid);

            var propertyEditor = PropertyEditorResolver.Current.GetByAlias(dataTypeDef.PropertyEditorAlias);

            return new DataType()
            {
                Guid = dataTypeDef.Key,
                PropertyTypeAlias = dataTypeDef.PropertyEditorAlias,
                PreValues = GetPreValues(dataTypeDef),
                View = propertyEditor.ValueEditor.View
            };
        }

        /// <summary>
        /// Get first 50 Umbraco users
        /// </summary>
        /// <returns>A list of <see cref="User"/> objects</returns>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            int totalUsers = 0;
            return Services.UserService.GetAll(0, 50, out totalUsers)
                .Where(u => u.IsApproved)
                .OrderBy(u => u.Name)
                .Select(u => new User() {
                    Name = u.Name,
                    Id = u.Id
                });
        }

        /// <summary>
        /// Get comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="CommentViewModel"/> object</returns>
        [HttpGet]
        public CommentViewModel GetComment(int id)
        {
            using (CommentsRepository repo = new CommentsRepository())
            {
                var commentVm = new CommentViewModel();
                return commentVm.Convert(repo.Get(id));
            }
        }

        /// <summary>
        /// Get a list of <see cref="CommentViewModel"/> objects of a content node property
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyDataId"></param>
        /// <param name="propertyTypeAlias"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CommentViewModel> GetComments(int contentId, int propertyDataId, string propertyTypeAlias)
        {
            ContentPropertyViewModel _propertyVM = new ContentPropertyViewModel()
            {
                ContentId = contentId,
                PropertyDataId = propertyDataId,
                PropertyTypeAlias = propertyTypeAlias
            };

            var commenVm = new CommentViewModel();

            if (_propertyVM.ContentId > 0 && _propertyVM.PropertyDataId > 0)
            {
                using (CommentsRepository repo = new CommentsRepository())
                {
                    var _content = Services.ContentService.GetById(_propertyVM.ContentId);
                    var _property = _content.Properties.FirstOrDefault(
                            p => p.Id == _propertyVM.PropertyDataId || p.Alias.Equals(_propertyVM.PropertyTypeAlias)
                        );

                    return repo.GetAllByContentProp(
                            _propertyVM.ContentId, _property != null ? _property.PropertyType.Id : -1)
                            .Select(c => commenVm.Convert(c)
                        );
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a list of <see cref="CommentViewModel"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CommentViewModel> GetAllComments()
        {
            var commenVm = new CommentViewModel();

            using (CommentsRepository repo = new CommentsRepository())
            {
                return repo.GetAll().Select(c => commenVm.Convert(c));
            }
        }

        /// <summary>
        /// Get a list of <see cref="CommentViewModel"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<CommentViewModel> GetMyComments(int userId)
        {
            var commenVm = new CommentViewModel();

            using (CommentsRepository repo = new CommentsRepository())
            {
                return repo.GetAllByAssignee(userId).Select(c => commenVm.Convert(c));
            }
        }

        /// <summary>
        /// Add a new comment
        /// </summary>
        /// <param name="comment">A <see cref="CommentViewModel"/> object</param>
        [HttpPost]
        public void AddComment(object commentVm)
        {
            var comment = new Comment();

            CommentViewModel commentDto = JsonConvert.DeserializeObject<CommentViewModel>(commentVm.ToString());
            commentDto.CreateDate = DateTime.Now;

            DoAddOrUpdate(comment.Convert(commentDto));
        }

        /// <summary>
        /// Update a comment
        /// </summary>
        /// <param name="comment">A <see cref="CommentViewModel"/> object</param>
        [HttpPut]
        public void UpdateComment(object commentVm)
        {
            var comment = new Comment();

            CommentViewModel commentDto = JsonConvert.DeserializeObject<CommentViewModel>(commentVm.ToString());

            DoAddOrUpdate(comment.Convert(commentDto));
        }

        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void DeleteComment(int id)
        {
            using (CommentsRepository repo = new CommentsRepository())
            {
                repo.Delete(id);
            }
        }

        /// <summary>
        /// Cleanup comments
        /// </summary>
        /// <returns>Count of comments that were deleted</returns>
        [HttpDelete]
        public int CleanupComments()
        {
            int result = 0;

            using (CommentsRepository repo = new CommentsRepository())
            {
                var comments = repo.GetAll();
                foreach(var comment in comments)
                {
                    bool delete = false;

                    // Check if the content exists
                    var _content = Services.ContentService.GetById(comment.ContentId);

                    if(_content != null)
                    {
                        // Check if property exists
                        var _property = _content.Properties.FirstOrDefault(
                            p => p.PropertyType.Id == comment.PropertyTypeId
                        );

                        if (_property == null) delete = true;
                    }
                    else
                    {
                        delete = true;
                    }

                    if(delete)
                    {
                        repo.Delete(comment);
                        result++;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Set comment completed
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void TaskComplete(object id)
        {
            using (CommentsRepository repo = new CommentsRepository())
            {
                var comment = repo.Get(Convert.ToInt32(id));
                if (comment.Type > 0 && comment.State == false)
                {
                    comment.State = true;
                    repo.AddOrUpdate(comment);
                }
            }
        }

        #region Private methods

        /// <summary>
        /// Get a dictionary list of prevalues of the property editor
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        private IDictionary<string, object> GetPreValues(IDataTypeDefinition dataType)
        {
            if (dataType == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var propertyEditor = PropertyEditorResolver.Current.GetByAlias(dataType.PropertyEditorAlias);

            var prevalues = Services.DataTypeService.GetPreValuesCollectionByDataTypeId(dataType.Id);
            var converted = propertyEditor.PreValueEditor.ConvertDbToEditor(
                    propertyEditor.DefaultPreValues,
                    prevalues
                );

            return converted;
        }

        /// <summary>
        /// Add or update a comment
        /// </summary>
        /// <param name="comment">A <see cref="Comment"/> object</param>
        private void DoAddOrUpdate(Comment comment)
        {
            using (CommentsRepository repo = new CommentsRepository())
            {
                if (!(comment.ContentId > 0)) throw new ArgumentException("Content node not found");
                if (!(comment.PropertyTypeId > 0)) throw new ArgumentException("PropertyType not found");

                repo.AddOrUpdate(comment);
            }
        }

        #endregion
    }
}