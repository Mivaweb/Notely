using System;
using System.Collections.Generic;

using Notely.Core.Enum;
using Notely.Core.Models;
using Notely.Core.Persistence.Repositories;

namespace Notely.Core.Services
{
    /// <summary>
    /// Defines a NoteCommentService
    /// </summary>
    public class NoteCommentService : INoteCommentService
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public NoteCommentService() { }

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects of a given note
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public IEnumerable<NoteComment> GetByNoteId(int noteId)
        {
            using (var repo = new NoteCommentRepository())
            {
                return repo.GetAllByNote(noteId);
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<NoteComment> GetAll()
        {
            using (var repo = new NoteCommentRepository())
            {
                return repo.GetAll();
            }
        }

        /// <summary>
        /// Get a list of <see cref="NoteComment"/> objects based on logtype
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        public IEnumerable<NoteComment> GetAll(string logType)
        {
            using (var repo = new NoteCommentRepository())
            {
                return repo.GetAll(logType);
            }
        }

        /// <summary>
        /// Saves a single <see cref="NoteComment"/>
        /// </summary>
        /// <param name="noteComment"></param>
        public void Save(NoteComment noteComment)
        {
            using (var repo = new NoteCommentRepository())
            {
                repo.AddOrUpdate(noteComment);
            }
        }

        /// <summary>
        /// Deletes a single <see cref="NoteComment"/>
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            using (var repo = new NoteCommentRepository())
            {
                repo.Delete(id);
            }
        }

        /// <summary>
        /// Add a new <see cref="NoteComment"/>
        /// </summary>
        /// <param name="comment"></param>
        public void Add(int note, int userId, NoteCommentType type, string description)
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
        /// Deletes all <see cref="NoteComment"/> objects for a given note
        /// </summary>
        /// <param name="noteId"></param>
        public void DeleteByNoteId(int noteId)
        {
            using (var repo = new NoteCommentRepository())
            {
                repo.DeleteByNote(noteId);
            }
        }
    }
}
