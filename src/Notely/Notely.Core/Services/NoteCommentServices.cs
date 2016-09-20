using System;

using Notely.Core.Enum;
using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteCommentServices
    /// </summary>
    public static class NoteCommentServices
    {
        /// <summary>
        /// Add a new NoteComment
        /// </summary>
        /// <param name="comment"></param>
        public static void Add(int note, int userId, NoteCommentType type, string description)
        {
            using (var repo = new NoteCommentRepository())
            {
                repo.AddOrUpdate(new NoteComment() {
                    LogType = type.ToString(),
                    LogComment = description,
                    Datestamp = DateTime.Now,
                    NoteId = note,
                    UserId = userId
                });
            }
        }

        /// <summary>
        /// Delete comments by note
        /// </summary>
        /// <param name="note"></param>
        public static void DeleteByNote(int note)
        {
            using (var repo = new NoteCommentRepository())
            {
                repo.DeleteByNote(note);
            }
        }
    }
}
