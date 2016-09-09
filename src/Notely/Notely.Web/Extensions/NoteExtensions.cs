using System;
using System.Linq;

using Umbraco.Web;

using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="Note"/> and <see cref="NoteViewModel"/> classes
    /// </summary>
    public static class NoteExtensions
    {
        /// <summary>
        /// Convert a <see cref="NoteViewModel"/> to a <see cref="Note"/> object
        /// </summary>
        /// <param name="note"></param>
        /// <param name="noteVm"></param>
        /// <returns></returns>
        public static Note Convert(this Note note, NoteViewModel noteVm)
        {
            /* 
             * Check if the ContentProperty is defined => 
             * without it will be hard to know for which content node and property
             * we are talking about.
             */
            if (noteVm.ContentProperty == null)
                throw new ArgumentNullException("ContentProperty of the note is not defined.");

            var _content = UmbracoContext.Current.Application.Services.ContentService.GetById(
                noteVm.ContentProperty.ContentId);

            /* 
             * 1) We first check if we can find the property type based on the property data id.
             *    If we can't find the type then there is a new version published of the property value.
             * 2) Next step is to find the type based on the alias of the property.
             * 
             */
            var _property = _content.Properties.FirstOrDefault(
                    p => p.Id == noteVm.ContentProperty.PropertyDataId || p.Alias.Equals(
                        noteVm.ContentProperty.PropertyTypeAlias)
                );

            // Create a new note object
            var _note = new Note()
            {
                CreateDate = noteVm.CreateDate,
                Description = noteVm.Description,
                Id = noteVm.Id,
                Title = noteVm.Title,
                Type = noteVm.Type.Id,
                ContentId = noteVm.ContentProperty.ContentId,
                PropertyTypeId = _property != null ? _property.PropertyType.Id : -1
            };

            // Only if it has a assignee and the note type can be assigned to
            if (noteVm.AssignedTo != null && noteVm.Type.CanAssign)
            {
                _note.AssignedTo = noteVm.AssignedTo.Id;
            }

            // If the note type can be assigned set the state else leave null
            if(noteVm.Type.CanAssign)
            {
                _note.State = noteVm.State.Id;
            }

            return _note;
        }

        /// <summary>
        /// Convert a <see cref="Note"/> to a <see cref="NoteViewModel"/> object
        /// </summary>
        /// <param name="noteVm"></param>
        /// <param name="note"></param>
        /// <returns></returns>
        public static NoteViewModel Convert(this NoteViewModel noteVm, Note note)
        {
            var _content = UmbracoContext.Current.Application.Services.ContentService.GetById(note.ContentId);

            /* 
             * 1) We first check if we can find the property type based on the property data id.
             *    If we can't find the type then there is a new version published of the property value.
             * 2) Next step is to find the type based on the alias of the property.
             * 
             */
            var _property = _content.Properties.FirstOrDefault(
                    p => p.PropertyType.Id == note.PropertyTypeId
                );

            var userVm = new UserViewModel();
            var noteTypeVm = new NoteTypeViewModel();
            var noteStateVm = new NoteStateViewModel();

            var contentProperty = new ContentPropertyViewModel() {
                ContentId = note.ContentId,
                ContentName = _content.Name,
                PropertyTypeAlias = _property.Alias
            };

            var result = new NoteViewModel()
            {
                AssignedTo = note.AssignedTo.HasValue ? 
                    userVm.Convert(
                        UmbracoContext.Current.Application.Services.UserService.GetUserById(note.AssignedTo.Value)
                    ) : null,
                CreateDate = note.CreateDate,
                Description = note.Description,
                Id = note.Id,
                State = noteStateVm.Convert(note.NoteState),
                Title = note.Title,
                Type = noteTypeVm.Convert(note.NoteType),
                ContentProperty = contentProperty
            };

            return result;
        }
    }
}