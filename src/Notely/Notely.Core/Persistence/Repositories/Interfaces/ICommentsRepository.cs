using System.Collections.Generic;

using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a ICommentsRepository
    /// </summary>
    public interface ICommentsRepository : IRepository<int, Comment>
    {
        /// <summary>
        /// Delete comment by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Delete all comments by content id
        /// </summary>
        /// <param name="contentId"></param>
        void DeleteByContent(int contentId);

        /// <summary>
        /// Get all comments by content id and property type id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId);

        /// <summary>
        /// Get all comments by content id, property id and user id
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId, int userId);

        /// <summary>
        /// Get all comments by user id
        /// </summary>
        /// <param name="assignee"></param>
        /// <returns></returns>
        IEnumerable<Comment> GetAllByAssignee(int assignee);

        /// <summary>
        /// Get unique content node id's with comments
        /// </summary>
        /// <param name="userId">If a user id is given ( id >= 0 )</param>
        /// <returns></returns>
        IEnumerable<int> GetUniqueContentNodes(int userId);

        /// <summary>
        /// Get all comments based on the comment type id
        /// </summary>
        /// <param name="commentTypeId"></param>
        /// <returns></returns>
        IEnumerable<Comment> GetAllByType(int commentTypeId);
    }
}
