using Notely.Core.Models;
using Notely.Web.Models;

namespace Notely.Web.Extensions
{
    /// <summary>
    /// Implement extension methods for the <see cref="NoteTypeViewModel"/>
    /// </summary>
    public static class NoteTypeExtensions
    {
        /// <summary>
        /// Converts a <see cref="NoteType"/> object to <see cref="NoteTypeViewModel"/> object
        /// </summary>
        /// <param name="noteTypeVm"></param>
        /// <param name="noteType"></param>
        /// <returns></returns>
        public static NoteTypeViewModel Convert(this NoteTypeViewModel noteTypeVm, NoteType noteType)
        {
            if (noteType == null)
                return null;

            return new NoteTypeViewModel()
            {
                Id = noteType.Id,
                Title = noteType.Title,
                Icon = noteType.Icon,
                CanAssign = noteType.CanAssign
            };
        }

        /// <summary>
        /// Converts a <see cref="NoteTypeViewModel"/> object to a <see cref="NoteType"/> object
        /// </summary>
        /// <param name="noteType"></param>
        /// <param name="noteTypeVm"></param>
        /// <returns></returns>
        public static NoteType Convert(this NoteType noteType, NoteTypeViewModel noteTypeVm)
        {
            return new NoteType() {

                Id = noteTypeVm.Id,
                Title = noteTypeVm.Title,
                Icon = noteTypeVm.Icon,
                CanAssign = noteTypeVm.CanAssign
            };
        }
    }
}