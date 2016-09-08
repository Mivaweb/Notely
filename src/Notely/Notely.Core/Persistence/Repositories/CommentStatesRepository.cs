using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories.Interfaces;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a CommentStatesRepository
    /// </summary>
    public class CommentStatesRepository : ICommentStatesRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentStatesRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Get a list of <see cref="CommentState"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<CommentState> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<CommentState>("SELECT * FROM notelyCommentStates ORDER BY Id");
        }

        public void AddOrUpdate(CommentState entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(CommentState entity)
        {
            throw new NotImplementedException();
        }
        
        public bool Exists(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get a <see cref="CommentState"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommentState Get(int id)
        {
            return _dbContext.Database.Single<CommentState>(id);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
    }
}
