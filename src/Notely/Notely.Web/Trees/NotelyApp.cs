using umbraco.businesslogic;
using umbraco.interfaces;
using Umbraco.Web.Mvc;

namespace Notely.Web.Trees
{
    /// <summary>
    /// Defines a new application ( section ) for Notely
    /// </summary>
    [Application("Notely", "Notely", "icon-notepad", 10)]
    [PluginController("Notely")]
    public class NotelyApp : IApplication
    {

    }
}