using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a INoteTypesRepository
    /// </summary>
    public interface INoteTypesRepository : IRepository<int, NoteType>
    {
        /// <summary>
        /// Delete note type by id
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
