using System;
using System.Net.Http.Formatting;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace Notely.Web.Trees
{
    /// <summary>
    /// Defines a new tree for the notely section
    /// </summary>
    [Tree("notely", "notely", "Notely")]
    [PluginController("Notely")]
    public class NotelyTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            // Check if its the root node
            if(id == "-1")
            {
                var nodes = new TreeNodeCollection();

                // ===> Routepath has to be: /appsection/treealias/htmlview/ID 
                nodes.Add(this.CreateTreeNode("comments", id, queryStrings, "All comments", "icon-notepad", false, "/notely/notely/comments/0"));
                nodes.Add(this.CreateTreeNode("myComments", id, queryStrings, "My comments", "icon-operator", false, "/notely/notely/comments/1"));
                nodes.Add(this.CreateTreeNode("cleanup", id, queryStrings, "Cleanup", "icon-trash-alt-2", false, "/notely/notely/cleanup/manage"));

                return nodes;
            }

            // No more then one level of nodes is accepted
            throw new NotSupportedException();
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            return new MenuItemCollection();
        }
    }
}