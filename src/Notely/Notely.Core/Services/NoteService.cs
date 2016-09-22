using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteService
    /// </summary>
    public static class NoteService
    {
        /// <summary>
        /// Get <see cref="Note"/> by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Note GetById(int id)
        {
            using (var repo = new NotesRepository())
            {
                return repo.Get(id);
            }
        }
    }
}
