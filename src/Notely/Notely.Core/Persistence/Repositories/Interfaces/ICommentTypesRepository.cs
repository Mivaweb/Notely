using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a ICommentTypesRepository
    /// </summary>
    public interface ICommentTypesRepository : IRepository<int, CommentType>
    {
        /// <summary>
        /// Delete comment type by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
