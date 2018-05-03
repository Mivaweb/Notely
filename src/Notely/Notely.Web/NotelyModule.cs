using Notely.Core;
using Notely.Core.Services;
using System.Web;

namespace Notely.Web
{
    /// <summary>
    /// Implements a custom HttpModule
    /// </summary>
    public class NotelyModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        /// <summary>
        /// Init module
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                var httpContext = ((HttpApplication)sender).Context;
                BeginRequest(new HttpContextWrapper(httpContext));
            };
        }

        /// <summary>
        /// Setup NotelyContext on every request
        /// </summary>
        /// <param name="httpContext"></param>
        static void BeginRequest(HttpContextBase httpContext)
        {
            new NotelyBootManager().Initialize();

            NotelyContext.SetupContext(
                    httpContext, ServiceContext.Current
                );
        }
    }
}