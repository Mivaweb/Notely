using System.Collections.Generic;

using Notely.Core.Models;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a INoteStateService
    /// </summary>
    public interface INoteStateService
    {
        /// <summary>
        /// Get note states
        /// </summary>
        /// <returns></returns>
        IEnumerable<NoteState> GetAll();
    }
}
