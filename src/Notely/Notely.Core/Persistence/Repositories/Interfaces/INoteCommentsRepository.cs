using System.Collections.Generic;

using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a INoteCommentsRepository
    /// </summary>
    public interface INoteCommentsRepository : IRepository<int, NoteComment>
    {
        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects for a given note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        IEnumerable<NoteComment> GetAllByNote(int noteId);

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects for a given logtype
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        IEnumerable<NoteComment> GetAll(string logType);

        /// <summary>
        /// Delete <see cref="NoteComment"/> object based on id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Delete all comment by note
        /// </summary>
        /// <param name="noteId"></param>
        void DeleteByNote(int noteId);
    }
}
