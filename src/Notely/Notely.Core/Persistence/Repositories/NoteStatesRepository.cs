using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories.Interfaces;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a NoteStatesRepository
    /// </summary>
    public class NoteStatesRepository : INoteStatesRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteStatesRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Get a list of <see cref="NoteState"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<NoteState> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<NoteState>("SELECT * FROM notelyNoteStates ORDER BY Id");
        }

        public void AddOrUpdate(NoteState entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(NoteState entity)
        {
            throw new NotImplementedException();
        }
        
        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a <see cref="NoteState"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NoteState Get(int id)
        {
            return _dbContext.Database.Single<NoteState>(id);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
    }
}
