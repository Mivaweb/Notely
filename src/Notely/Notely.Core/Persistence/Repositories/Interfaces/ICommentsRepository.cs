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
        void Delete(int id);
        void DeleteByContent(int contentId);
        IEnumerable<Comment> GetAllByContentProp(int contentId, int propertyTypeId);
        IEnumerable<Comment> GetAllByAssignee(int assignee);
    }
}
