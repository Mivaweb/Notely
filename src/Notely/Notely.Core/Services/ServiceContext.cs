using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notely.Core.Services
{
    /// <summary>
    /// Contain the available Notely services
    /// </summary>
    public class ServiceContext
    {
        /// <summary>
        /// Services
        /// </summary>
        private Lazy<INoteCommentService> _noteCommentService;

        public INoteCommentService NoteCommentService
        {
            get
            {
                return _noteCommentService.Value;
            }
        }

        private Lazy<INoteService> _noteService;

        public INoteService NoteService
        {
            get
            {
                return _noteService.Value;
            }
        }

        private Lazy<INoteTypeService> _noteTypeService;

        public INoteTypeService NoteTypeService
        {
            get
            {
                return _noteTypeService.Value;
            }
        }

        private Lazy<INoteStateService> _noteStateService;

        public INoteStateService NoteStateService
        {
            get
            {
                return _noteStateService.Value;
            }
        }

        /// <summary>
        /// Get the current singleton ServiceContext object
        /// </summary>
        public static ServiceContext Current { get; internal set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public ServiceContext() {
            ConfigureServices();
        }

        /// <summary>
        /// Configure the services
        /// </summary>
        public void ConfigureServices()
        {
            if (_noteCommentService == null)
                _noteCommentService = new Lazy<INoteCommentService>(() => new NoteCommentService());

            if (_noteService == null)
                _noteService = new Lazy<INoteService>(() => new NoteService());

            if (_noteTypeService == null)
                _noteTypeService = new Lazy<INoteTypeService>(() => new NoteTypeService());

            if (_noteStateService == null)
                _noteStateService = new Lazy<INoteStateService>(() => new NoteStateService());
        }
    }
}
