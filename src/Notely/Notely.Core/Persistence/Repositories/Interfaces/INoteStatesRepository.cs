using Umbraco.Core.Persistence.Repositories;

using Notely.Core.Models;

namespace Notely.Core.Persistence.Repositories.Interfaces
{
    /// <summary>
    /// Defines a INoteStatesRepository
    /// </summary>
    public interface INoteStatesRepository : IRepository<int, NoteState>
    {
    }
}
