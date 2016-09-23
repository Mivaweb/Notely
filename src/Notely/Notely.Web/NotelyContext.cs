using System;
using System.Web;

using Umbraco.Core;

using Notely.Core.Services;

namespace Notely.Web
{
    /// <summary>
    /// Implements the Notely information
    /// </summary>
    public class NotelyContext : DisposableObject, IDisposable
    {
        internal const string HttpContextItemName = "Notely.Web.NotelyContext";

        internal NotelyContext(HttpContextBase httpContext, ServiceContext serviceContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");
            if (serviceContext == null) throw new ArgumentNullException("serviceContext");

            HttpContext = httpContext;
            Services = serviceContext;
        }

        [ThreadStatic]
        private static NotelyContext _notelyContext;

        /// <summary>
        ///  Get or sets the NotelyContext
        /// </summary>
        public static NotelyContext Current
        {
            get
            {
                if (System.Web.HttpContext.Current != null)
                    return (NotelyContext)System.Web.HttpContext.Current.Items[HttpContextItemName];

                return _notelyContext;
            }

            internal set
            {
                if(System.Web.HttpContext.Current != null && Current != null)
                    throw new ApplicationException("The current NotelyContext can only be set once during a request.");

                if(System.Web.HttpContext.Current != null)
                {
                    System.Web.HttpContext.Current.Items[HttpContextItemName] = value;
                } else
                {
                    _notelyContext = value;
                }
            }
        }

        /// <summary>
        /// Setup the NotelyContext
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static NotelyContext SetupContext(HttpContextBase httpContext, ServiceContext serviceContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");
            if (serviceContext == null) throw new ArgumentNullException("serviceContext");

            if (NotelyContext.Current != null)
                return NotelyContext.Current;

            var _context = new NotelyContext(httpContext, serviceContext);

            NotelyContext.Current = _context;

            return _context;
        }

        /// <summary>
        /// Exposes the current ServiceContext
        /// </summary>
        public ServiceContext Services { get; private set; }

        /// <summary>
        /// Exposes the current HttpContext
        /// </summary>
        public HttpContextBase HttpContext { get; private set; }

        private HttpRequestBase GetRequestFromContext()
        {
            try
            {
                return HttpContext.Request;
            }
            catch (System.Web.HttpException)
            {
                return null;
            }
        }

        /// <summary>
        /// Dispose notely context
        /// </summary>
        protected override void DisposeResources()
        {
            _notelyContext = null;
        }
    }
}