using System.Collections.Generic;

using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a INotesRepository
    /// </summary>
    public interface INotesRepository : IRepository<int, Note>
    {
        /// <summary>
        /// Delete note by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Delete all notes by content id
        /// </summary>
        /// <param name="contentId"></param>
        void DeleteByContent(int contentId);

        /// <summary>
        /// Get all notes by content id and property type id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByContentProp(int contentId, int propertyTypeId);

        /// <summary>
        /// Get all notes by content id, property id and user id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByContentProp(int contentId, int propertyTypeId, int userId);

        /// <summary>
        /// Get all notes by user id
        /// </summary>
        /// <param name="assignee"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByAssignee(int assignee);

        /// <summary>
        /// Get unique content node id's with notes
        /// </summary>
        /// <param name="userId">If a user id is given ( id >= 0 )</param>
        /// <returns></returns>
        IEnumerable<int> GetUniqueContentNodes(int userId);

        /// <summary>
        /// Get all notes based on the note type id
        /// </summary>
        /// <param name="noteTypeId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByType(int noteTypeId);

        /// <summary>
        /// Add or update note and return the id of the note
        /// </summary>
        /// <param name="note"></param>
        /// <param name="noteId"></param>
        void AddOrUpdate(Note note, out int noteId);
    }
}
