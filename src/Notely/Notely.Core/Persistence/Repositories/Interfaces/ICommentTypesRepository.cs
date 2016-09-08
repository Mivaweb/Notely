using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a ICommentTypesRepository
    /// </summary>
    public interface ICommentTypesRepository : IRepository<int, CommentType>
    {
    }
}
