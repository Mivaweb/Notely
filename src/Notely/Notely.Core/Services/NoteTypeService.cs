using System.Collections.Generic;

using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteTypeService
    /// </summary>
    public class NoteTypeService : INoteTypeService
    {
        /// <summary>
        /// Get a list of <see cref="NoteType"/> objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NoteType> GetAll()
        {
            using (var repo = new NoteTypesRepository())
            {
                return repo.GetAll();
            }
        }

        /// <summary>
        /// Get a single <see cref="NoteType"/> object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NoteType GetById(int id)
        {
            using (var repo = new NoteTypesRepository())
            {
                return repo.Get(id);
            }
        }

        /// <summary>
        /// Saves a <see cref="NoteType"/> object
        /// </summary>
        /// <param name="noteType"></param>
        public void Save(NoteType noteType)
        {
            using (var repo = new NoteTypesRepository())
            {
                repo.AddOrUpdate(noteType);
            }
        }

        /// <summary>
        /// Deletes a <see cref="NoteType"/> object
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var repo = new NoteTypesRepository())
            {
                repo.Delete(id);
            }
        }
    }
}
