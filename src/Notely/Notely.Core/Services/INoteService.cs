using System.Collections.Generic;

using Notely.Core.Models;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a INoteService
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Get note by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Note GetById(int id);

        /// <summary>
        /// Get notes
        /// </summary>
        /// <returns></returns>
        IEnumerable<Note> GetAll();

        /// <summary>
        /// Get notes by content id and propertytype id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAll(int contentId, int propertyTypeId);

        /// <summary>
        /// Get notes by content id and propertytype id and user id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAll(int contentId, int propertyTypeId, int userId);

        /// <summary>
        /// Get notes assigned to user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByAssignee(int userId);

        /// <summary>
        /// Saves a note
        /// </summary>
        /// <param name="note"></param>
        int Save(Note note);

        /// <summary>
        /// Deletes a note
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Deletes a note
        /// </summary>
        /// <param name="note"></param>
        void Delete(Note note);

        /// <summary>
        /// Get unique content node id's for my tasks
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<int> GetUniqueContentNodes(int userId);

        /// <summary>
        /// Get notes by note type
        /// </summary>
        /// <param name="noteTypeId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByType(int noteTypeId);

        /// <summary>
        /// Get notes by content node
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        IEnumerable<Note> GetAllByContent(int contentId);
    }
}
