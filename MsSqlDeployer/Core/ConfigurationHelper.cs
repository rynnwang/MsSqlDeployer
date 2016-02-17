using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using Beyova.MicrosoftSqlDeployTool.Core;

namespace Beyova
{
    /// <summary>
    /// Class ConfigurationHelper.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// The config path
        /// </summary>
        public static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.ConfigurationFilePath);

        /// <summary>
        /// Loads the solutions.
        /// </summary>
        /// <param name="xDocument">The x document.</param>
        /// <returns></returns>
        public static List<Solution> LoadSolutions(XDocument xDocument)
        {
            List<Solution> solutions = new List<Solution>();

            if (xDocument != null && xDocument.Root.Name.LocalName == Constants.Database)
            {
                bool isOldVersion = xDocument.Root.Elements().Count() > 0 && xDocument.Root.Element(Constants.Solution) == null;
                if (isOldVersion)
                {
                    AdjustToNewVersion(xDocument);
                }

                foreach (var one in xDocument.Root.Elements(Constants.Solution))
                {
                    Solution solution = LoadSolution(one);
                    if (solution != null)
                    {
                        solutions.Add(solution);
                    }
                }
            }

            return solutions;
        }

        /// <summary>
        /// Adjusts to new version.
        /// </summary>
        /// <param name="xDocument">The x document.</param>
        private static void AdjustToNewVersion(XDocument xDocument)
        {
            if (xDocument != null)
            {
                XElement xElement = CreateNodeByName(Constants.Solution);
                xElement.SetAttributeValue(Constants.Name, "Default");

                foreach (var one in xDocument.Root.Elements())
                {
                    xElement.Add(one);
                }

                xDocument.Root.RemoveAll();
                xDocument.Root.Add(xElement);

                xDocument.SaveConfiguration();
            }
        }

        /// <summary>
        /// Loads the solution.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private static Solution LoadSolution(XElement node)
        {
            Solution solution = null;

            if (node != null)
            {
                solution = new Solution();
                solution.Name = node.GetAttributeValue(Constants.Name);
                solution.ConnectionSetting = ConnectionStringModel.LoadConnectionString(node.Element(Constants.ConnectionString));
                solution.FullXml = node.Element(Constants.Full);
                solution.IncrementalXml = node.Elements(Constants.Incremental);

                if (solution.FullXml != null)
                {
                    SetDefaultEnabledValue(solution.FullXml);
                }
                if (solution.IncrementalXml != null)
                {
                    foreach (var one in solution.IncrementalXml)
                    {
                        SetDefaultEnabledValue(one);
                    }
                }
            }

            return solution;
        }

        /// <summary>
        /// Sets the default enabled value.
        /// </summary>
        /// <param name="node">The node.</param>
        private static void SetDefaultEnabledValue(XElement databaseNode)
        {
            if (databaseNode != null)
            {
                foreach (var node in databaseNode.Elements())
                {
                    if (node.Attribute(Constants.Enabled) == null)
                    {
                        node.SetAttributeValue(Constants.Enabled, true);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the name of the node by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static XElement CreateNodeByName(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("Name is null or empty.");
            }

            try
            {
                return XElement.Parse(string.Format("<{0}></{0}>", name));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create node by name.", ex);
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <param name="xDocument">The x document.</param>
        public static void SaveConfiguration(this XDocument xDocument)
        {
            if (xDocument != null)
            {
                xDocument.Save(ConfigPath);
            }
        }

        /// <summary>
        /// Generates the id string.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <returns></returns>
        public static string GenerateIdString(this XElement xElement)
        {
            return xElement != null ?
                string.Format("{0}_{1}_{2}", xElement.Name.LocalName, xElement.GetAttributeValue(Constants.BaseVersion), xElement.GetAttributeValue(Constants.TargetVersion)) : string.Empty;
        }

        /// <summary>
        /// Determines whether the specified x element is enabled.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <returns></returns>
        public static bool IsEnabled(this XElement xElement)
        {
            return xElement != null && xElement.GetAttributeValue(Constants.Enabled).Equals("true", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
