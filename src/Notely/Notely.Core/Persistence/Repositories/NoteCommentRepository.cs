using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories.Interfaces;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a NoteCommentsRepository
    /// </summary>
    public class NoteCommentRepository : INoteCommentsRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteCommentRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Adds or updates a <see cref="NoteComment"/> object
        /// </summary>
        /// <param name="entity"></param>
        public void AddOrUpdate(NoteComment entity)
        {
            if(entity.Id > 0)
            {
                _dbContext.Database.Update(entity);
            } else
            {
                _dbContext.Database.Insert(entity);
            }
        }

        /// <summary>
        /// Delete <see cref="NoteComment"/> object
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(NoteComment entity)
        {
            _dbContext.Database.Delete(entity);
        }

        /// <summary>
        /// Delete comments by note
        /// </summary>
        /// <param name="noteId"></param>
        public void DeleteByNote(int noteId)
        {
            _dbContext.Database.Delete<NoteComment>("WHERE noteId = @p1", new { p1 = noteId });
        }

        /// <summary>
        /// Delete <see cref="NoteComment"/> base on id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Delete(Get(id));
        }

        /// <summary>
        /// Check if <see cref="NoteComment"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return _dbContext.Database.Exists<NoteComment>(id);
        }

        /// <summary>
        /// Get <see cref="NoteComment"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NoteComment Get(int id)
        {
            return _dbContext.Database.Single<NoteComment>(id);
        }

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<NoteComment> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<NoteComment>("SELECT * FROM notelyNoteComments ORDER BY datestamp DESC");
        }

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects for a given note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public IEnumerable<NoteComment> GetAllByNote(int noteId)
        {
            return _dbContext.Database.Fetch<NoteComment>("SELECT * FROM notelyNoteComments WHERE noteId = @p1 ORDER BY datestamp DESC", new { p1 = noteId });
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
    }
}
