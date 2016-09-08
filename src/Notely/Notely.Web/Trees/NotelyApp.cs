using umbraco.businesslogic;
using umbraco.interfaces;
using Umbraco.Web.Mvc;

namespace Notely.Web.Trees
{
    /// <summary>
    /// Implements a new application ( section ) for Notely
    /// </summary>
    [Application("notely", "Notely", "icon-notepad", 10)]
    [PluginController("Notely")]
    public class NotelyApp : IApplication
    {

    }
}