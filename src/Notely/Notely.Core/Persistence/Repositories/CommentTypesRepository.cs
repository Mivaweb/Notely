using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories.Interfaces;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a CommentTypeRepository
    /// </summary>
    public class CommentTypesRepository : ICommentTypesRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentTypesRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Get a list of <see cref="CommentType"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<CommentType> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<CommentType>("SELECT * FROM notelyCommentTypes ORDER BY ID");
        }

        public void AddOrUpdate(CommentType entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(CommentType entity)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Check if <see cref="CommentType"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return _dbContext.Database.Exists<CommentType>(id);
        }

        /// <summary>
        /// Get a <see cref="CommentType"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CommentType Get(int id)
        {
            return _dbContext.Database.Single<CommentType>(id);
        }

        /// <summary>
        ///  Dispose
        /// </summary>
        public void Dispose()
        {

        }
    }
}
