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
using Umbraco.Web.Editors;

using Notely.Core.Models;
using Notely.Web.Models;
using Notely.Web.Extensions;
using Notely.Core.Persistence.Repositories;
using Notely.Core.Services;
using Notely.Core.Enum;

namespace Notely.Web.Controllers
{
    /// <summary>
    /// Implements a NotelyApiController
    /// </summary>
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
        /// Get note
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="NoteViewModel"/> object</returns>
        [HttpGet]
        public NoteViewModel GetNote(int id)
        {
            using (var repo = new NotesRepository())
            {
                var noteVm = new NoteViewModel();
                return noteVm.Convert(repo.Get(id));
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteViewModel"/> objects of a content node property
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyDataId"></param>
        /// <param name="propertyTypeAlias"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<NoteViewModel> GetNotes(int contentId, int propertyDataId, string propertyTypeAlias)
        {
            var _propertyVM = new ContentPropertyViewModel()
            {
                ContentId = contentId,
                PropertyDataId = propertyDataId,
                PropertyTypeAlias = propertyTypeAlias
            };

            var noteVm = new NoteViewModel();

            if (_propertyVM.ContentId > 0 && _propertyVM.PropertyDataId > 0)
            {
                using (var repo = new NotesRepository())
                {
                    var _content = Services.ContentService.GetById(_propertyVM.ContentId);
                    var _property = _content.Properties.FirstOrDefault(
                            p => p.Id == _propertyVM.PropertyDataId || p.Alias.Equals(_propertyVM.PropertyTypeAlias)
                        );

                    return repo.GetAllByContentProp(
                            _propertyVM.ContentId, _property != null ? _property.PropertyType.Id : -1)
                            .Select(c => noteVm.Convert(c)
                        );
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteViewModel"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<NoteViewModel> GetAllNotes()
        {
            var noteVm = new NoteViewModel();

            using (var repo = new NotesRepository())
            {
                return repo.GetAll().Select(c => noteVm.Convert(c));
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteViewModel"/> objects
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<NoteViewModel> GetMyTasks(int userId)
        {
            var noteVm = new NoteViewModel();

            using (var repo = new NotesRepository())
            {
                return repo.GetAllByAssignee(userId).Select(c => noteVm.Convert(c));
            }
        }

        /// <summary>
        /// Add a new note
        /// </summary>
        /// <param name="noteVm">A <see cref="NoteViewModel"/> object</param>
        [HttpPost]
        public void AddNote(object noteVm)
        {
            var note = new Note();

            NoteViewModel noteDto = JsonConvert.DeserializeObject<NoteViewModel>(noteVm.ToString());
            noteDto.CreateDate = DateTime.Now;

            int noteId = DoAddOrUpdate(note.Convert(noteDto));

            // Add log comment
            NoteCommentServices.Add(
                noteId, 
                GetCurrentUserId(), 
                NoteCommentType.New, 
                "Note %" + noteDto.Title + "% was created.");
        }

        /// <summary>
        /// Update a note
        /// </summary>
        /// <param name="noteVm">A <see cref="NoteViewModel"/> object</param>
        [HttpPut]
        public void UpdateNote(object noteVm)
        {
            var note = new Note();

            NoteViewModel noteDto = JsonConvert.DeserializeObject<NoteViewModel>(noteVm.ToString());

            NoteViewModel _oldNote = GetNote(noteDto.Id);

            int noteId = DoAddOrUpdate(note.Convert(noteDto));

            // Add log comment
            NoteCommentServices.Add(
                noteId,
                GetCurrentUserId(),
                NoteCommentType.Save,
                "Note %" + noteDto.Title + "% was saved.");

            // Check if type is changed
            if (_oldNote.Type.Id != noteDto.Type.Id)
            {
                NoteCommentServices.Add(
                    noteId,
                    GetCurrentUserId(),
                    NoteCommentType.Save,
                    "Note %" + noteDto.Title + "% type changed from " + _oldNote.Type.Title + " to " + noteDto.Type.Title + ".");
            }

            // Check if state is changed
            if (_oldNote.State.Id > 0 && noteDto.State.Id > 0 && (_oldNote.State.Id != noteDto.State.Id))
            {
                NoteCommentServices.Add(
                    noteId,
                    GetCurrentUserId(),
                    NoteCommentType.Save,
                    "Note %" + noteDto.Title + "% state changed from " + _oldNote.State.Title + " to " + noteDto.State.Title + ".");
            }
        }

        /// <summary>
        /// Delete a note
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void DeleteNote(int id)
        {
            DeleteNoteCommentsByNote(id);

            using (var repo = new NotesRepository())
            {
                repo.Delete(id);
            }
        }

        /// <summary>
        /// Cleanup notes
        /// </summary>
        /// <returns>Count of notes that were deleted</returns>
        [HttpDelete]
        public int CleanupNotes()
        {
            int result = 0;

            using (var repo = new NotesRepository())
            {
                var notes = repo.GetAll();
                foreach(var note in notes)
                {
                    bool delete = false;

                    // Check if the content exists
                    var _content = Services.ContentService.GetById(note.ContentId);

                    if(_content != null)
                    {
                        // Check if property exists
                        var _property = _content.Properties.FirstOrDefault(
                            p => p.PropertyType.Id == note.PropertyTypeId
                        );

                        if (_property == null) delete = true;
                    }
                    else
                    {
                        delete = true;
                    }

                    if(delete)
                    {
                        repo.Delete(note);
                        result++;
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// Get a list of <see cref="NoteType"/> objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<NoteTypeViewModel> GetNoteTypes()
        {
            var _noteType = new NoteTypeViewModel();

            using (var repo = new NoteTypesRepository())
            {
                return repo.GetAll().Select(c => _noteType.Convert(c));
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteStateViewModel"/> objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<NoteStateViewModel> GetNoteStates()
        {
            var _noteState = new NoteStateViewModel();

            using (var repo = new NoteStatesRepository())
            {
                return repo.GetAll().Select(c => _noteState.Convert(c));
            }
        }
        
        /// <summary>
        /// Get a list of unique content id's that has notes
        /// </summary>
        /// <param name="userId">For a certain user ( assignedTo )</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<int> GetUniqueContentNodes(int userId)
        {
            using(var repo = new NotesRepository())
            {
                var result = repo.GetUniqueContentNodes(userId);
                return result;
            }
        }

        /// <summary>
        /// Get details of the content
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        [HttpGet]
        public BackOfficeNode GetBackOfficeNodeDetails(int contentId, int userId)
        {
            var _result = new BackOfficeNode();
            var _note = new NoteViewModel();

            using (var repo = new NotesRepository())
            {
                // Step 1: Get the content node details
                var _content = Services.ContentService.GetById(contentId);

                if (_content == null)
                    throw new ArgumentNullException("contentId");

                _result.ContentId = contentId;
                _result.ContentName = _content.Name;
                
                foreach(var prop in _content.Properties.Where(p => p.PropertyType.PropertyEditorAlias == "Notely"))
                {
                    var dataTypeDef = Services.DataTypeService.GetDataTypeDefinitionById(
                        prop.PropertyType.DataTypeDefinitionId);
                    var limitValue = int.Parse(GetPreValues(dataTypeDef)["limit"].ToString());

                    // Step 2: Add properties and notes
                    _result.Properties.Add(new BackOfficeProperty() {

                        Alias = prop.Alias,
                        Id = prop.PropertyType.Id,
                        Name = prop.PropertyType.Name,
                        Limit = limitValue,
                        Notes = userId >= 0 ? repo.GetAllByContentProp(
                            _content.Id, prop.PropertyType.Id, userId)
                            .Select(c => _note.Convert(c)).ToList() :
                            repo.GetAllByContentProp(
                            _content.Id, prop.PropertyType.Id)
                            .Select(c => _note.Convert(c)).ToList()

                    });
                }

            }

            return _result;
        }

        /// <summary>
        /// Get note type
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A <see cref="NoteTypeViewModel"/> object</returns>
        [HttpGet]
        public NoteTypeViewModel GetNoteType(int id)
        {
            using (var repo = new NoteTypesRepository())
            {
                var noteTypeVm = new NoteTypeViewModel();
                return noteTypeVm.Convert(repo.Get(id));
            }
        }

        /// <summary>
        /// Add a new note Type
        /// </summary>
        /// <param name="noteType"></param>
        [HttpPost]
        public void AddNoteType(object noteType)
        {
            var _noteType = new NoteType();

            var noteTypeDto = JsonConvert.DeserializeObject<NoteTypeViewModel>(noteType.ToString());

            DoAddOrUpdate(_noteType.Convert(noteTypeDto));
        }

        /// <summary>
        /// Update an existing Note Type
        /// </summary>
        /// <param name="noteType"></param>
        [HttpPut]
        public void UpdateNoteType(object noteType)
        {
            var _noteType = new NoteType();

            var noteTypeDto = JsonConvert.DeserializeObject<NoteTypeViewModel>(noteType.ToString());

            DoAddOrUpdate(_noteType.Convert(noteTypeDto));
        }

        /// <summary>
        /// Delete a note type
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        public void DeleteNoteType(int id)
        {
            DeleteNotesByType(id);

            using (var repo = new NoteTypesRepository())
            {
                repo.Delete(id);
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
        /// Add or update a note
        /// </summary>
        /// <param name="note">A <see cref="Note"/> object</param>
        private int DoAddOrUpdate(Note note)
        {
            int noteId = -1;
            using (var repo = new NotesRepository())
            {
                if (!(note.ContentId > 0)) throw new ArgumentException("Content node not found");
                if (!(note.PropertyTypeId > 0)) throw new ArgumentException("PropertyType not found");

                repo.AddOrUpdate(note, out noteId);

                return noteId;
            }
        }

        /// <summary>
        /// Add or update a note type
        /// </summary>
        /// <param name="noteType">A <see cref="NoteType"/> object</param>
        private void DoAddOrUpdate(NoteType noteType)
        {
            using (var repo = new NoteTypesRepository())
            {
                repo.AddOrUpdate(noteType);
            }
        }

        /// <summary>
        /// Delete all notes based on a note type
        /// </summary>
        /// <param name="noteTypeId"></param>
        private void DeleteNotesByType(int noteTypeId)
        {
            using (var repo = new NotesRepository())
            {
                foreach(var note in repo.GetAllByType(noteTypeId))
                {
                    repo.Delete(note);
                }
            }
        }

        /// <summary>
        /// Get current userid
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            var userService = Services.UserService;
            return userService.GetByUsername(UmbracoContext.Security.CurrentUser.Username).Id;
        }

        /// <summary>
        /// Delete note comments by note
        /// </summary>
        /// <param name="noteId"></param>
        private void DeleteNoteCommentsByNote(int noteId)
        {
            NoteCommentServices.DeleteByNote(noteId);
        }

        #endregion
    }
}