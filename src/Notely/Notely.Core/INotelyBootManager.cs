using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notely.Core
{
    /// <summary>
    /// Bootstrapper interface to boot the Notely application
    /// </summary>
    public interface INotelyBootManager
    {
        INotelyBootManager Initialize();
    }
}
