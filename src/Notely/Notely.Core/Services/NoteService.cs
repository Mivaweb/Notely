using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

using System.Collections.Generic;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteService
    /// </summary>
    public class NoteService : INoteService
    {
        /// <summary>
        /// Get a single <see cref="Note"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Note GetById(int id)
        {
            using (var repo = new NotesRepository())
            {
                return repo.Get(id);
            }
        }

        /// <summary>
        /// Get a list of <see cref="Note"/> objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Note> GetAll()
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAll();
            }
        }

        /// <summary>
        /// Get a list of <see cref="Note"/> object for a given content id and property id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetAll(int contentId, int propertyTypeId)
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAllByContentProp(contentId, propertyTypeId);
            }
        }

        /// <summary>
        /// Get a list of <see cref="Note"/> object for a given content id and property id and user id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetAll(int contentId, int propertyTypeId, int userId)
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAllByContentProp(contentId, propertyTypeId, userId);
            }
        }

        /// <summary>
        /// Get a list of <see cref="Note"/> objects assigned to specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetAllByAssignee(int userId)
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAllByAssignee(userId);
            }
        }

        /// <summary>
        /// Saves a <see cref="Note"/> object
        /// </summary>
        /// <param name="note"></param>
        public int Save(Note note)
        {
            int noteId = -1;
            using (var repo = new NotesRepository())
            {
                repo.AddOrUpdate(note, out noteId);
            }
            return noteId;
        }

        /// <summary>
        /// Deletes a <see cref="Note"/> object
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var repo = new NotesRepository())
            {
                repo.Delete(id);
            }
        }

        /// <summary>
        /// Deletes a <see cref="Note"/> object
        /// </summary>
        /// <param name="note"></param>
        public void Delete(Note note)
        {
            using (var repo = new NotesRepository())
            {
                repo.Delete(note);
            }
        }

        /// <summary>
        /// Returns a list of unique content node id's for my tasks
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<int> GetUniqueContentNodes(int userId)
        {
            using (var repo = new NotesRepository())
            {
                var result = repo.GetUniqueContentNodes(userId);
                return result;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="Note"/> objects based on note type
        /// </summary>
        /// <param name="noteTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetAllByType(int noteTypeId)
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAllByType(noteTypeId);
            }
        }

        /// <summary>
        /// Get a lis of <see cref="Note"/> objects of a given content node
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public IEnumerable<Note> GetAllByContent(int contentId)
        {
            using (var repo = new NotesRepository())
            {
                return repo.GetAllByContent(contentId);
            }
        }
    }
}
