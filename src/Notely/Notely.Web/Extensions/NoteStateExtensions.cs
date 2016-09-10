using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension method for the <see cref="NoteStateViewModel"/> object
    /// </summary>
    public static class NoteStateExtensions
    {
        /// <summary>
        /// Converts a <see cref="NoteState"/> object to a <see cref="NoteStateViewModel"/> object
        /// </summary>
        /// <param name="noteStateVm"></param>
        /// <param name="noteState"></param>
        /// <returns></returns>
        public static NoteStateViewModel Convert(this NoteStateViewModel noteStateVm, NoteState noteState)
        {
            if (noteState == null)
                return null;

            return new NoteStateViewModel()
            {
                Id = noteState.Id,
                Title = noteState.Title
            };
        }
    }
}