using System;
using System.Collections.Generic;

using Umbraco.Core;

using Notely.Core.Persistence.Repositories.Interfaces;
using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories
{
    /// <summary>
    /// Defines a CommentsRepository
    /// </summary>
    public class CommentsRepository : ICommentsRepository
    {
        // Database context
        private DatabaseContext _dbContext;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentsRepository()
        {
            _dbContext = ApplicationContext.Current.DatabaseContext;
        }

        /// <summary>
        /// Add or update a <see cref="Comment"/> object
        /// </summary>
        /// <param name="entity"></param>
        public void AddOrUpdate(Comment entity)
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
        /// Delete <see cref="Comment"/> object
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(Comment entity)
        {
            _dbContext.Database.Delete(entity);
        }

        /// <summary>
        /// Delete comment by id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Delete(Get(id));
        }

        /// <summary>
        /// Delete comments of a content node
        /// </summary>
        /// <param name="contentId"></param>
        public void DeleteByContent(int contentId)
        {
            _dbContext.Database.Delete<Comment>("DELETE FROM commentorComments WHERE contentId = @p1", new { p1 = contentId });
        }

        /// <summary>
        /// Check if <see cref="Comment"/> exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exists(int id)
        {
            return _dbContext.Database.Exists<Comment>(id);
        }

        /// <summary>
        /// Get <see cref="Comment"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment Get(int id)
        {
            return _dbContext.Database.Single<Comment>(id);
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<Comment>("SELECT * FROM notelyComments ORDER BY type, createDate");
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects of a content node and property
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId)
        {
            return _dbContext.Database.Fetch<Comment>("SELECT * FROM notelyComments WHERE contentId = @p1 AND propertyTypeId = @p2 ORDER BY type, createDate", new { p1 = contentId, p2 = propertyTypeId });
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
    }
}
