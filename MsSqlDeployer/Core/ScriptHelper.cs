using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Beyova.MicrosoftSqlDeployTool.Core
{
    /// <summary>
    /// Static class for Script Helper
    /// </summary>
    public static class ScriptHelper
    {
        private static Regex goSperator = new Regex("([Gg][Oo](;)?[\r\n\t]+)", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        /// <summary>
        /// The proc lock
        /// </summary>
        private static Object procLock = new Object();

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public static WriteOutputDelegate WriteOutputDelegate
        {
            get;
            set;
        }

        /// <summary>
        /// Writes the output line.
        /// </summary>
        /// <param name="content">The content.</param>
        public static void WriteOutputLine(string content)
        {
            WriteOutput(content.SafeToString() + Environment.NewLine);
        }

        /// <summary>
        /// Writes the output.
        /// </summary>
        /// <param name="content">The content.</param>
        public static void WriteOutput(string content)
        {
            if (WriteOutputDelegate != null)
            {
                WriteOutputDelegate.Invoke(content);
            }
        }

        /// <summary>
        /// Generates the scripts.
        /// </summary>
        /// <param name="baseFolder">The base folder.</param>
        /// <param name="resultContainer">The result container.</param>
        /// <param name="sectionXml">The x element.</param>
        /// <param name="databaseName">Name of the Database.</param>
        /// <returns></returns>
        internal static string GenerateScripts(string baseFolder, string resultContainer, XElement sectionXml, string databaseName)
        {
            string fullPath = Path.Combine(resultContainer, sectionXml.Name.LocalName + "_All.sql");

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                List<string> sqlScriptList = new List<string>();
                foreach (var node in sectionXml.Elements())
                {
                    if (sectionXml.IsEnabled())
                    {
                        string fileNamePath = Path.Combine(baseFolder, sectionXml.Name.LocalName + @"\" + node.Value);
                        if (File.Exists(fileNamePath))
                        {
                            sqlScriptList.Add(fileNamePath);
                        }
                    }
                }
                CombineScripts(databaseName, sqlScriptList, fullPath);
            }
            catch (Exception ex)
            {
                WriteOutputLine("Failed to generate scripts caused by:" + ex.Message);
            }

            return fullPath;
        }

        /// <summary>
        /// Determines whether [has matched value node] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="includedXml">The included XML.</param>
        /// <returns>
        ///   <c>true</c> if [has matched value node] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasMatchedValueNode(string value, XElement includedXml)
        {
            if (includedXml == null)
            {
                return false;
            }
            var matchedItem = from one in includedXml.Elements() where one.Value == value select one;
            return matchedItem != null && matchedItem.Count() > 0;
        }

        /// <summary>
        /// Combines the scripts.
        /// </summary>
        /// <param name="DatabaseName">Name of the Database.</param>
        /// <param name="sqlScriptList">The SQL script list.</param>
        /// <param name="pathToSaveScript">The path to save script.</param>
        public static void CombineScripts(string DatabaseName, List<string> sqlScriptList, string pathToSaveScript)
        {
            StreamReader steamReader = null;
            StreamWriter streamWriter = null;

            try
            {
                if (!string.IsNullOrWhiteSpace(pathToSaveScript))
                {
                    StringBuilder fileContentBuilder = new StringBuilder(string.IsNullOrWhiteSpace(DatabaseName) ? string.Empty : "USE [" + DatabaseName + "]" + Environment.NewLine + "GO" + Environment.NewLine);
                    foreach (string s in sqlScriptList)
                    {
                        steamReader = new StreamReader(s, Encoding.UTF8);
                        fileContentBuilder.Append(steamReader.ReadToEnd() + Environment.NewLine);
                    }
                    streamWriter = new StreamWriter(pathToSaveScript, false, Encoding.UTF8);
                    streamWriter.Write(fileContentBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (steamReader != null)
                {
                    steamReader.Close();
                }

                if (streamWriter != null)
                {
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }
        }

        /// <summary>
        /// Runs the SQL script.
        /// </summary>
        /// <param name="connectionStringModel">The connection string model.</param>
        /// <param name="scriptFullPath">The script full path.</param>
        public static void RunSqlScript(ConnectionStringModel connectionStringModel, List<string> scriptFullPath)
        {
            string connectionString = connectionStringModel.ToConnectionString();
            try
            {
                WriteOutputLine("Ready to execute SQL scripts...");

                foreach (var path in scriptFullPath)
                {
                    FileInfo fileSource = new FileInfo(path);
                    string script = fileSource.OpenText().ReadToEnd();

                    WriteOutput("Executing SQL script for: " + path + " ...");
                    ExecuteSql(connectionString, script);
                    WriteOutput("Done." + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Runs the bat command.
        /// </summary>
        /// <param name="batFilePath">The bat file path.</param>
        /// <param name="outputDelegate">The output delegate.</param>
        public static void RunBatCommand(string batFilePath, WriteOutputDelegate outputDelegate)
        {
            if (!string.IsNullOrWhiteSpace(batFilePath))
            {
                Process batProcess = new Process();
                // Redirect the output stream of the child process.
                batProcess.StartInfo.UseShellExecute = false;
                batProcess.StartInfo.RedirectStandardOutput = true;
                batProcess.StartInfo.RedirectStandardError = true;

                batProcess.StartInfo.FileName = batFilePath;

                if (outputDelegate != null)
                {
                    batProcess.OutputDataReceived += (sender, args) => outputDelegate(args.Data);
                    batProcess.ErrorDataReceived += (sender, args) => outputDelegate(args.Data);
                }
                batProcess.Start();

                batProcess.BeginOutputReadLine();

                //StreamReader streamReader = batProcess.StandardOutput;
                //// Read the standard output of the spawned process. 
                //string output = streamReader.ReadLine();
                //outputDelegate(output);

                batProcess.WaitForExit();
                batProcess.Close();
            }
        }


        /// <summary>
        /// Writes the execution script log.
        /// </summary>
        /// <param name="userIdentity">The user identity.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="baseVersion">The base version.</param>
        /// <param name="targetVersion">The target version.</param>
        /// <param name="isFull">if set to <c>true</c> [is full].</param>
        public static void WriteExecutionScriptLog(string userIdentity, string connectionString, Version baseVersion, Version targetVersion, bool isFull)
        {
            if (!string.IsNullOrWhiteSpace(userIdentity) && !string.IsNullOrWhiteSpace(connectionString) && targetVersion != null)
            {
                Guid key = Guid.NewGuid();
                string sql = string.Format("insert into dbo.[SQLDeployToolLog] ([Key],[ExecutedBy],[BaseVersion],[TargetVersion],[IsFull]) select '{0}','{1}','{2}','{3}',{4} ", key, userIdentity, baseVersion == null ? string.Empty : baseVersion.ToString(), targetVersion.ToString(), isFull ? "1" : "0");
                ExecuteSql(connectionString, sql);
            }
        }

        /// <summary>
        /// Gets the required version.
        /// </summary>
        /// <param name="connectionStringModel">The connection string model.</param>
        /// <returns></returns>
        internal static string GetRequiredVersion(ConnectionStringModel connectionStringModel)
        {
            string result = string.Empty;

            if (connectionStringModel != null)
            {
                string useStatement = string.IsNullOrWhiteSpace(connectionStringModel.Database) ? string.Empty : "USE " + connectionStringModel.Database + @";
            ";
                string sql = @"
            DECLARE @VersionResult AS  [nvarchar](50);
            SET @VersionResult = NULL;
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SQLDeployToolLog]') AND type in (N'U'))
                SELECT TOP 1 @VersionResult = [TargetVersion]  from dbo.[SQLDeployToolLog] ORDER BY [ExecutedTime] DESC;
            ELSE
            BEGIN
                CREATE TABLE [dbo].[SQLDeployToolLog](" +
                    "[Key] [uniqueidentifier] NULL," +
                    "[ExecutedBy] [nvarchar](50) NULL," +
                    "[BaseVersion] [nvarchar](50) NULL," +
                    "[TargetVersion] [nvarchar](50) NULL," +
                    "[IsFull] [bit] NULL," +
                    "[ExecutedTime] [datetime] NOT NULL default(getutcdate())" +
                    @") ON [PRIMARY];               
            END
            IF  @VersionResult IS NULL
                SET @VersionResult =  ' ';
            SELECT @VersionResult;
            ";
                result = ExecuteSqlScalar(connectionStringModel.ToConnectionString(), useStatement + sql) as string;
            }

            return result;
        }

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlText">The SQL text.</param>
        /// <returns>System.Int32.</returns>
        public static int ExecuteSql(string connectionString, string sqlText)
        {
            int result = 0;

            if (!string.IsNullOrWhiteSpace(sqlText))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(string.Empty, conn);
                    cmd.CommandType = CommandType.Text;

                    var pieces = goSperator.Split(sqlText);

                    try
                    {
                        foreach (var one in pieces)
                        {
                            if (!string.IsNullOrWhiteSpace(one))
                            {
                                cmd.CommandText = one;
                                if (conn.State != ConnectionState.Open)
                                {
                                    conn.Open();
                                }
                                result += cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Executes the SQL.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlText">The SQL text.</param>
        /// <returns></returns>
        public static object ExecuteSqlScalar(string connectionString, string sqlText)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sqlText, conn);

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                try
                {
                    object result = cmd.ExecuteScalar();
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (conn != null && conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static string TestConnection(string connectionString)
        {
            string result = null;
            string sqlText = "SELECT @@VERSION";
            try
            {
                result = (string)ExecuteSqlScalar(connectionString, sqlText);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Generates the BCP script.
        /// </summary>
        /// <param name="baseFolder">The base folder.</param>
        /// <param name="resultContainer">The result container.</param>
        /// <param name="sectionXml">The section XML.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns></returns>
        [Obsolete]
        internal static string GenerateBCPScript(string baseFolder, string resultContainer, XElement sectionXml, string databaseName)
        {
            string fullPath = Path.Combine(resultContainer, sectionXml.Name.LocalName + "_All.sql");

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                StringBuilder stringBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(databaseName) && sectionXml.IsEnabled())
                {
                    stringBuilder.Append("USE [" + databaseName + "]" + Environment.NewLine + "GO" + Environment.NewLine);
                    stringBuilder.Append(@"
EXEC sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
EXEC sp_configure 'xp_cmdshell', 1
GO
RECONFIGURE
GO
");
                    foreach (XElement node in sectionXml.Elements())
                    {
                        string targetTable = node.GetAttributeValue("TargetTable");
                        if (!string.IsNullOrWhiteSpace(targetTable))
                        {
                            string fileNamePath = Path.Combine(baseFolder, sectionXml.Name.LocalName + @"\" + node.Value);
                            if (File.Exists(fileNamePath))
                            {
                                stringBuilder.Append(@"TRUNCATE TABLE dbo.[" + targetTable + "];" + Environment.NewLine);


                                stringBuilder.Append(@"exec xp_cmdShell 'bcp [" + databaseName + "].[dbo].[" + targetTable + @"] in """ + fileNamePath + @""" -c -T'" + Environment.NewLine);
                            }
                        }
                    }
                }


                // Save to file
                File.WriteAllText(fullPath, stringBuilder.ToString(), Encoding.UTF8);

            }
            catch (Exception ex)
            {
                WriteOutputLine("Failed to generate BCP scripts caused by:" + ex.Message);
            }

            return fullPath;
        }

        /// <summary>
        /// Generates the BCP command.
        /// </summary>
        /// <param name="connectionStringModel">The connection string model.</param>
        /// <param name="baseFolder">The base folder.</param>
        /// <param name="resultContainer">The result container.</param>
        /// <param name="sectionXml">The section XML.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>System.String.</returns>
        internal static string GenerateBCPCommand(ConnectionStringModel connectionStringModel, string baseFolder, string resultContainer, XElement sectionXml, string databaseName)
        {
            string fullPath = Path.Combine(resultContainer, sectionXml.Name.LocalName + "_All.bat");

            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                StringBuilder stringBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(databaseName) && sectionXml.IsEnabled())
                {
                    stringBuilder.Append(@"@echo off
");

                    string lineFormat = @"@echo ---------------------------------------------
@echo Importing data for {1} ...
@bcp [{0}].[dbo].[{1}] in ""{2}"" -S {4} -w {3}
";

                    foreach (XElement node in sectionXml.Elements())
                    {
                        string targetTable = node.GetAttributeValue("TargetTable");
                        if (!string.IsNullOrWhiteSpace(targetTable))
                        {
                            string fileNamePath = Path.Combine(baseFolder, sectionXml.Name.LocalName + @"\" + node.Value);
                            if (File.Exists(fileNamePath))
                            {

                                stringBuilder.AppendFormat(lineFormat,
                                    // Database Name
                                    databaseName,
                                    // Table Name
                                    targetTable,
                                    // BCP file name
                                    fileNamePath,
                                    // Login info: -T or 
                                    connectionStringModel.IsWindowsAuthentication ? "-T" : string.Format("-U {0} -P{1}", connectionStringModel.UserName.SafeToString(), connectionStringModel.Password.SafeToString()),
                                    // server name
                                    connectionStringModel.Server
                                    );
                            }
                        }
                    }
                }

                // Save to file
                File.WriteAllText(fullPath, stringBuilder.ToString(), Encoding.ASCII);
            }
            catch (Exception ex)
            {
                WriteOutputLine("Failed to generate BCP command file caused by:" + ex.Message);
            }

            return fullPath;
        }
    }
}
