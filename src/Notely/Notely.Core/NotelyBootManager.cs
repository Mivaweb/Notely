using System;

using Notely.Core.Services;

namespace Notely.Core
{
    public class NotelyBootManager : INotelyBootManager
    {
        private bool _initialized = false;

        protected ServiceContext ServiceContext { get; private set; }

        public NotelyBootManager() { }

        /// <summary>
        /// Initialize boot manager
        /// </summary>
        /// <returns></returns>
        public INotelyBootManager Initialize()
        {
            if (_initialized)
                throw new InvalidOperationException("The boot manager is already initialised.");

            ServiceContext.Current = ServiceContext = CreateServiceContext();

            _initialized = true;

            return this;
        }

        /// <summary>
        /// Create service context
        /// </summary>
        /// <param name="dbFactory"></param>
        /// <returns></returns>
        protected virtual ServiceContext CreateServiceContext()
        {
            return new ServiceContext();
        }
    }
}
