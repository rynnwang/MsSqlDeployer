using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Linq;

namespace Beyova.MicrosoftSqlDeployTool.Core
{
    public delegate void WriteOutputDelegate(string text);

    /// <summary>
    /// Class SqlExecutor. This class cannot be inherited.
    /// </summary>
    public sealed class SqlExecutor
    {
        private bool isFull = false;
        private Version baseVersion = null;
        private Version targetVersion = null;
        private ConnectionStringModel connectionModel = null;
        private WriteOutputDelegate writeOutputDelegate = null;
        private XElement runningXml = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutor" /> class.
        /// </summary>
        /// <param name="writeOutputDelegate">The write output delegate.</param>
        /// <param name="connectionSetting">The connection setting.</param>
        /// <param name="runningXml">The running XML.</param>
        public SqlExecutor(WriteOutputDelegate writeOutputDelegate, ConnectionStringModel connectionSetting, XElement runningXml)
        {
            this.writeOutputDelegate = writeOutputDelegate;
            connectionModel = connectionSetting;

            if (runningXml != null)
            {
                this.runningXml = runningXml;
                isFull = runningXml.Name.LocalName.Equals("full", StringComparison.InvariantCultureIgnoreCase);
            }

            ScriptHelper.WriteOutputDelegate = writeOutputDelegate;
        }

        /// <summary>
        /// Executes the specified solution name.
        /// </summary>
        /// <param name="solutionName">Name of the solution.</param>
        public void Execute(string solutionName)
        {
            string fullOrIncremental = string.Empty;

            if (runningXml != null && connectionModel != null)
            {
                fullOrIncremental = (isFull ? Constants.Full : Constants.Incremental);

                /// Read configuration values for later use.
                #region Read version

                try
                {
                    WriteOutputLine("Checking settings & xml contents...");

                    baseVersion = isFull ? null : new Version(runningXml.Attribute(Constants.BaseVersion).Value);
                    targetVersion = new Version(runningXml.Attribute(Constants.TargetVersion).Value);

                    WriteOutputLine("Running version selected: " + fullOrIncremental);
                    WriteOutputLine("Base version: " + (isFull ? "*" : baseVersion.ToString()));
                    WriteOutputLine("Target version: " + targetVersion.ToString());
                }
                catch (Exception ex)
                {
                    WriteOutputLine("Failed to load version info caused by: " + ex.Message);
                    return;
                }

                #endregion
            }

            #region Try to initialize SQL connection instance

            SqlConnection sqlConnection = null;

            try
            {
                sqlConnection = new SqlConnection(connectionModel.ToConnectionString());
            }
            catch (Exception ex)
            {
                WriteOutputLine("Failed to initialize SQL connection instance caused by: " + ex.Message);
                return;
            }

            #endregion

            #region Try to match version for deployment

            try
            {
                WriteOutput("Trying to check database version ... ");
                string versionString = ScriptHelper.GetRequiredVersion(connectionModel);
                Version currentVersion = !string.IsNullOrWhiteSpace(versionString) ? new Version(versionString) : null;
                if (!isFull && currentVersion != null && currentVersion.CompareTo(baseVersion) != 0)
                {
                    WriteOutputLine("Unmatched version to deploy. Current:" + currentVersion.ToString() + ", Required:" + baseVersion.ToString());
                    return;
                }
                else
                {
                    WriteOutputLine("matched.");
                }
            }
            catch (Exception ex)
            {
                WriteOutputLine("Failed to match version caused by: " + ex.Message);
                return;
            }

            #endregion

            #region Prepare deployment scripts

            string baseContainer = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullOrIncremental);
            WriteOutputLine("Original script container: " + GetFileUrl(baseContainer));
            string targetContainer = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, solutionName.ToShortId() + "_" + fullOrIncremental + "_" + (baseVersion == null ? string.Empty : baseVersion.ToString()) + "_" + targetVersion.ToString());
            WriteOutputLine("Output script container: " + GetFileUrl(targetContainer));
            if (!Directory.Exists(targetContainer))
            {
                Directory.CreateDirectory(targetContainer);
                WriteOutputLine("Directory created for output script container: " + GetFileUrl(targetContainer));
            }

            List<string> scriptNames = new List<string>();
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.Table, connectionModel.Database);
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.Function, connectionModel.Database);
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.View, connectionModel.Database);
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.ViewBaseFunction, connectionModel.Database);
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.StoredProcedure, connectionModel.Database);
            GenerateScripts(scriptNames, baseContainer, targetContainer, runningXml, Constants.Data, connectionModel.Database);
            var bcpFilePath = GenerateBCPCommand(connectionModel, baseContainer, targetContainer, runningXml, Constants.BCP, connectionModel.Database);

            #endregion

            WriteOutputLine();

            bool isSuccess = false;
            try
            {
                ScriptHelper.RunSqlScript(connectionModel, scriptNames);

                if (!string.IsNullOrWhiteSpace(bcpFilePath))
                {
                    ScriptHelper.RunBatCommand(bcpFilePath, writeOutputDelegate);
                }

                WriteOutputLine("----------------------------------------------");
                WriteOutputLine("Completed!");
                WriteOutputLine();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                WriteOutputLine("Error occurred!");
                WriteOutputLine("----------------------------------------------");
                WriteOutputLine(ex.Message);
                WriteOutputLine();
            }


            if (isSuccess)
            {
                string operatorInfo = connectionModel.IsWindowsAuthentication ? System.Security.Principal.WindowsIdentity.GetCurrent().Name : connectionModel.UserName;
                ScriptHelper.WriteExecutionScriptLog(operatorInfo, connectionModel.ToConnectionString(), baseVersion, targetVersion, isFull);
                WriteOutputLine("SQL log has updated to:" + targetVersion.ToString());
            }
        }

        /// <summary>
        /// Decodes from base64.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string DecodeFromBase64(string input)
        {
            string result = string.Empty;
            try
            {
                byte[] bytes = Convert.FromBase64String(input);
                result = System.Text.Encoding.UTF8.GetString(bytes);
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Generates the scripts.
        /// </summary>
        /// <param name="scriptNames">The script names.</param>
        /// <param name="baseContainer">The base container.</param>
        /// <param name="targetContainer">The target container.</param>
        /// <param name="rootNode">The root node.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="includedXml">The included XML.</param>
        private void GenerateScripts(List<string> scriptNames, string baseContainer, string targetContainer, XElement rootNode, string sectionName, string databaseName)
        {
            var node = rootNode.Element(sectionName);
            if (node == null)
            {
                WriteOutputLine("Skip to generate scripts for " + sectionName + " caused by no section node found.");
            }
            else
            {
                string path = ScriptHelper.GenerateScripts(baseContainer, targetContainer, node, databaseName);
                scriptNames.Add(path);
                WriteOutputLine(sectionName + " path: " + (string.IsNullOrWhiteSpace(path) ? Constants.NA : ("file://" + path.Replace('\\', '/'))));

            }
        }

        /// <summary>
        /// Generates the BCP command.
        /// </summary>
        /// <param name="connectionModel">The connection model.</param>
        /// <param name="baseContainer">The base container.</param>
        /// <param name="targetContainer">The target container.</param>
        /// <param name="rootNode">The root node.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>System.String.</returns>
        private string GenerateBCPCommand(ConnectionStringModel connectionModel, string baseContainer, string targetContainer, XElement rootNode, string sectionName, string databaseName)
        {
            if (sectionName == Constants.BCP)
            {
                var node = rootNode.Element(sectionName);
                if (node == null)
                {
                    WriteOutputLine("Skip to generate scripts for " + sectionName + " caused by no section node found.");

                }
                else
                {
                    var path = ScriptHelper.GenerateBCPCommand(connectionModel, baseContainer, targetContainer, node, databaseName);
                    WriteOutputLine(sectionName + " path: " + (string.IsNullOrWhiteSpace(path) ? Constants.NA : ("file://" + path.Replace('\\', '/'))));
                    return path;
                }
            }

            return null;
        }

        /// <summary>
        /// Writes the output line.
        /// </summary>
        /// <param name="content">The content.</param>
        public void WriteOutputLine(string content = null)
        {
            WriteOutput(content.SafeToString() + Environment.NewLine);
        }

        /// <summary>
        /// Writes the output.
        /// </summary>
        /// <param name="content">The content.</param>
        public void WriteOutput(string content = null)
        {
            if (writeOutputDelegate != null)
            {
                writeOutputDelegate.Invoke(content);
            }
        }

        /// <summary>
        /// Gets the file URL.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetFileUrl(string path)
        {
            return string.IsNullOrWhiteSpace(path) ? string.Empty : ("file://" + (path.Trim().Replace('\\', '/').TrimEnd('/')) + "/");
        }
    }
}
