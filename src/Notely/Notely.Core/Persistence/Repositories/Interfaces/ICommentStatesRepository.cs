using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a ICommentStatesRepository
    /// </summary>
    public interface ICommentStatesRepository : IRepository<int, CommentState>
    {
    }
}
