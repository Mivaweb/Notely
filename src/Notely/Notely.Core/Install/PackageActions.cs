using System;
using System.Xml;
using System.Text;

using umbraco.BusinessLogic;
using umbraco.interfaces;
using Umbraco.Core.IO;
using Umbraco.Core;

namespace Notely.Core.Install
{
    /// <summary>
    /// Implements the Notely package actions
    /// </summary>
    public class PackageActions : IPackageAction
    {
        /// Path to the dashboard
        private const string PLUGIN_SOURCE = "/app_plugins/notely/backoffice/notely/dashboard.html";

        /// <summary>
        /// Returns the alias of the package
        /// </summary>
        /// <returns></returns>
        public string Alias()
        {
            return "Notely";
        }

        /// <summary>
        /// Execute the actions
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public bool Execute(string packageName, XmlNode xmlData)
        {
            ExecuteDashConfig();
            ExecuteLang();

            return true;

        }

        /// <summary>
        /// Sample method
        /// </summary>
        /// <returns></returns>
        public XmlNode SampleXml()
        {
            string sample = "<Action runat=\"install\" undo=\"true/false\" alias=\"Notely\"/>";
            return umbraco.cms.businesslogic.packager.standardPackageActions.helper.parseStringToXmlNode(sample);
        }

        /// <summary>
        /// Undo method
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public bool Undo(string packageName, XmlNode xmlData)
        {
            UndoDashConfig();
            UndoLang();

            return true;
        }

        #region Private methods
        /// <summary>
        /// Add Notely to the dashboard.config
        /// </summary>
        private void ExecuteDashConfig()
        {
            bool saveFile = false;

            // Get the dashboard.config file
            var dashConfig = IOHelper.MapPath(SystemFiles.DashboardConfig);

            // Add log item
            Log.Add(LogTypes.Notify, 0, "Adding " + Alias() + " tab to the Notely section om Umbraco backoffice");

            // Load in the dash config content
            XmlDocument dashXml = new XmlDocument();
            dashXml.Load(dashConfig);

            // Get notely item
            XmlNode _node = dashXml.SelectSingleNode("//section [@alias='" + Alias() + "']");

            // Only add this item if its not present
            if (_node == null)
            {
                var xmlSection = new StringBuilder();
                xmlSection.AppendLine("<section alias='" + Alias() + "'>");
                xmlSection.AppendLine("<areas>");
                xmlSection.AppendLine("<area>notely</area>");
                xmlSection.AppendLine("</areas>");
                xmlSection.AppendLine("<tab caption='Dashboard'>");
                xmlSection.AppendLine("<control addPanel='true' panelCaption=''>" + PLUGIN_SOURCE + "</control>");
                xmlSection.AppendLine("</tab>");
                xmlSection.AppendLine("</section>");

                // Get root node
                XmlNode rootNode = dashXml.SelectSingleNode("//dashBoard");

                if (rootNode != null)
                {
                    // Create xml document of the StringBuilder
                    XmlDocument xmlNodeToAdd = new XmlDocument();
                    xmlNodeToAdd.LoadXml(xmlSection.ToString());

                    var nodeToAdd = xmlNodeToAdd.SelectSingleNode("*");

                    rootNode.AppendChild(rootNode.OwnerDocument.ImportNode(nodeToAdd, true));

                    saveFile = true;
                }
            }

            if (saveFile)
            {
                dashXml.Save(dashConfig);
            }
        }

        /// <summary>
        /// Remove Notely from the dasboard.config
        /// </summary>
        private void UndoDashConfig()
        {
            // Remove the node from the xml config
            var dashConfig = IOHelper.MapPath(SystemFiles.DashboardConfig);

            XmlDocument dashXml = XmlHelper.OpenAsXmlDocument(dashConfig);

            XmlNode contentMapNode = dashXml.SelectSingleNode("//section [@alias='" + Alias() + "']");

            if (contentMapNode != null)
            {
                dashXml.SelectSingleNode("/dashBoard").RemoveChild(contentMapNode);
                dashXml.Save(dashConfig);
            }
        }

        /// <summary>
        /// Add Notely translation to the en.xml
        /// </summary>
        private void ExecuteLang()
        {
            bool saveFile = false;

            var _enLang = IOHelper.MapPath(SystemDirectories.Umbraco + "/Config/Lang/en.xml");

            // Add log item
            Log.Add(LogTypes.Notify, 0, "Adding " + Alias() + " translation to the en.xml language file");

            XmlDocument _enDoc = new XmlDocument();
            _enDoc.Load(_enLang);

            // Get notely item
            XmlNode _areaNode = _enDoc.SelectSingleNode("//area [@alias='sections']");
            XmlNode _notelyNode = _areaNode.SelectSingleNode("//key [@alias='notely']");

            if(_notelyNode == null)
            {
                var xmlSection = new StringBuilder();
                xmlSection.AppendLine("<key alias='notely'>Notely</key>");

                if (_areaNode != null)
                {
                    // Create xml document of the StringBuilder
                    XmlDocument xmlNodeToAdd = new XmlDocument();
                    xmlNodeToAdd.LoadXml(xmlSection.ToString());

                    var nodeToAdd = xmlNodeToAdd.SelectSingleNode("*");

                    _areaNode.AppendChild(_areaNode.OwnerDocument.ImportNode(nodeToAdd, true));

                    saveFile = true;
                }
            }

            if (saveFile)
            {
                _enDoc.Save(_enLang);
            }
        }

        /// <summary>
        /// Removes Notely translation from the en.xml file
        /// </summary>
        private void UndoLang()
        {
            // Remove the node from the xml config
            var enFile = IOHelper.MapPath(SystemDirectories.Umbraco + "/Config/Lang/en.xml");

            XmlDocument enXml = XmlHelper.OpenAsXmlDocument(enFile);

            XmlNode notelyNode = enXml.SelectSingleNode("//key [@alias='notely']");

            if (notelyNode != null)
            {
                enXml.SelectSingleNode("//area [@alias='sections']").RemoveChild(notelyNode);
                enXml.Save(enFile);
            }
        }
        #endregion
    }
}
