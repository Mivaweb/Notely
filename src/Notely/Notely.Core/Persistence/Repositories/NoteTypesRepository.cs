using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories.Interfaces;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a NoteTypesRepository
    /// </summary>
    public class NoteTypesRepository : INoteTypesRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteTypesRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Get a list of <see cref="NoteType"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<NoteType> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<NoteType>("SELECT * FROM notelyNoteTypes ORDER BY ID");
        }

        /// <summary>
        /// Adds or updates the <see cref="NoteType"/> object
        /// </summary>
        /// <param name="entity"></param>
        public void AddOrUpdate(NoteType entity)
        {
            if (entity.Id > 0)
            {
                // Update entity
                _dbContext.Database.Update(entity);
            }
            else
            {
                // Add entity
                _dbContext.Database.Insert(entity);
            }
        }

        /// <summary>
        /// Delete note type
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(NoteType entity)
        {
            _dbContext.Database.Delete(entity);
        }

        /// <summary>
        /// Delete a note type by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Delete(Get(id));
        }
        
        /// <summary>
        /// Check if <see cref="NoteType"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return _dbContext.Database.Exists<NoteType>(id);
        }

        /// <summary>
        /// Get a <see cref="NoteType"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NoteType Get(int id)
        {
            return _dbContext.Database.Single<NoteType>(id);
        }

        /// <summary>
        ///  Dispose
        /// </summary>
        public void Dispose() { }
    }
}
