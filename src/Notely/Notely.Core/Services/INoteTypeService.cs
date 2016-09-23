using System.Collections.Generic;

using Notely.Core.Models;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a INoteTypeService
    /// </summary>
    public interface INoteTypeService
    {
        /// <summary>
        /// Get note types
        /// </summary>
        /// <returns></returns>
        IEnumerable<NoteType> GetAll();

        /// <summary>
        /// Get note type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        NoteType GetById(int id);

        /// <summary>
        /// Saves a note type
        /// </summary>
        /// <param name="noteType"></param>
        void Save(NoteType noteType);

        /// <summary>
        /// Deletes a note type
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}
