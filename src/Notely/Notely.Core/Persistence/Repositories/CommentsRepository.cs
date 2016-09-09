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
            _dbContext.Database.Delete<Comment>("DELETE FROM notelyNotes WHERE contentId = @p1", new { p1 = contentId });
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
            return _dbContext.Database.Fetch<Comment, CommentType, CommentState>("SELECT nc.*, ncs.*, nct.* FROM notelyNotes AS nc LEFT JOIN notelyNoteStates AS ncs ON ncs.id = nc.state JOIN notelyNoteTypes AS nct ON nct.id = nc.type WHERE nc.id = @p1", new { p1 = id })[0];
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAll(params int[] ids)
        {
            return _dbContext.Database.Fetch<Comment, CommentState, CommentType>("SELECT nc.*, ncs.*, nct.* FROM notelyNotes AS nc JOIN notelyNoteStates AS ncs ON ncs.id = nc.state JOIN notelyNoteTypes AS nct ON nct.id = nc.type ORDER BY nc.contentId, nc.propertyTypeId, nc.type, nc.createDate");
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects of a content node and property
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId)
        {
            return _dbContext.Database.Fetch<Comment, CommentState, CommentType>("SELECT nc.*, ncs.*, nct.* FROM notelyNotes AS nc LEFT JOIN notelyNoteStates AS ncs ON ncs.id = nc.state JOIN notelyNoteTypes AS nct ON nct.id = nc.type WHERE nc.contentId = @p1 AND nc.propertyTypeId = @p2 ORDER BY nc.type, nc.createDate", new { p1 = contentId, p2 = propertyTypeId });
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects of a content node and property and user
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId, int userId)
        {
            return _dbContext.Database.Fetch<Comment, CommentState, CommentType>("SELECT nc.*, ncs.*, nct.* FROM notelyNotes AS nc JOIN notelyNoteStates AS ncs ON ncs.id = nc.state JOIN notelyNoteTypes AS nct ON nct.id = nc.type WHERE nc.contentId = @p1 AND nc.propertyTypeId = @p2 AND nc.assignedTo = @p3 ORDER BY nc.type, nc.createDate", new { p1 = contentId, p2 = propertyTypeId, p3 = userId });
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects of a assignee
        /// </summary>
        /// <param name="assignee"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByAssignee(int assignee)
        {
            return _dbContext.Database.Fetch<Comment, CommentState, CommentType>("SELECT nc.*, ncs.*, nct.* FROM notelyNotes AS nc JOIN notelyNoteStates AS ncs ON ncs.id = nc.state JOIN notelyNoteTypes AS nct ON nct.id = nc.type WHERE assignedTo = @p1 ORDER BY type, createDate", new { p1 = assignee });
        }

        /// <summary>
        /// Get a list of unique content nodes with comments
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetUniqueContentNodes(int userId)
        {
            if(userId >= 0)
                return _dbContext.Database.Fetch<int>("SELECT DISTINCT contentId FROM notelyNotes WHERE assignedTo = @p1", new { p1 = userId });
            else
                return _dbContext.Database.Fetch<int>("SELECT DISTINCT contentId FROM notelyNotes");
        }

        /// <summary>
        /// Get a list of <see cref="Comment"/> objects based on a comment type
        /// </summary>
        /// <param name="commentTypeId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByType(int commentTypeId)
        {
            return _dbContext.Database.Fetch<Comment>("SELECT * FROM notelyNotes WHERE type = @p1", new { p1 = commentTypeId });
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() { }
    }
}
