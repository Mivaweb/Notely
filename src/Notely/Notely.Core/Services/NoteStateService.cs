using System.Collections.Generic;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteStateService
    /// </summary>
    public class NoteStateService : INoteStateService
    {
        /// <summary>
        /// Get a list of <see cref="NoteState"/> objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NoteState> GetAll()
        {
            using (var repo = new NoteStatesRepository())
            {
                return repo.GetAll();
            }
        }
    }
}
