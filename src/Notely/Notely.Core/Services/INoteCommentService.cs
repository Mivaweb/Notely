using System.Collections.Generic;

using Notely.Core.Models;
using Notely.Core.Enum;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines the INoteCommentService
    /// </summary>
    public interface INoteCommentService
    {
        /// <summary>
        ///  Get comments by note id
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        IEnumerable<NoteComment> GetByNoteId(int noteId);

        /// <summary>
        /// Get all comments
        /// </summary>
        /// <returns></returns>
        IEnumerable<NoteComment> GetAll();

        /// <summary>
        /// Get all comments based on logtype
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        IEnumerable<NoteComment> GetAll(string logType);

        /// <summary>
        /// Saves a notecomment object
        /// </summary>
        /// <param name="noteComment"></param>
        void Save(NoteComment noteComment);

        /// <summary>
        /// Deletes a noteComment object
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes all comments for a given note
        /// </summary>
        /// <param name="noteId"></param>
        void DeleteByNoteId(int noteId);

        /// <summary>
        /// Add a new note comment
        /// </summary>
        /// <param name="note"></param>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="description"></param>
        void Add(int note, int userId, NoteCommentType type, string description);
    }
}
