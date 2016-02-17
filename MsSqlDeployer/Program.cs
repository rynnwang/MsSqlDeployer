using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Beyova.MicrosoftSqlDeployTool.Core;

namespace Beyova.MicrosoftSqlDeployTool
{
    static class Program
    {
        /// <summary>
        /// Application entry
        /// </summary>
        /// <param name="args">The arguments.
        /// <list type="bullet">
        /// <item>0: solution name.</item>
        /// <item>1: package name. e.g.: (1)full, (2)incremental-1.0-1.2</item>
        /// <item>2: xml path. Default: Configuration.xml</item>
        /// </list></param>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Any())
            {
                var solutionName = args[0];
                var packageName = args.Length > 1 ? args[1] : null;
                var configurationPath = args.Length > 2 ? args[2] : ConfigurationHelper.ConfigPath;

                Execute(configurationPath, solutionName, packageName, Console.WriteLine);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        /// <summary>
        /// Executes the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="package">The package.</param>
        /// <param name="outputDelegate">The output delegate.</param>
        static void Execute(string filePath, string solutionName, string package, WriteOutputDelegate outputDelegate)
        {
            try
            {
                var xDocument = XDocument.Load(filePath, LoadOptions.None);
                var solutions = ConfigurationHelper.LoadSolutions(xDocument);

                var selectedSolution = solutions.Find((x) => { return x.Name.Equals(solutionName, StringComparison.OrdinalIgnoreCase); });

                XElement xmlToRun = null;

                if (selectedSolution != null)
                {
                    package = package.SafeToString();
                    if (package.Equals("full", StringComparison.OrdinalIgnoreCase))
                    {
                        xmlToRun = selectedSolution.FullXml;
                    }
                    else if (package.StartsWith("incremental-", StringComparison.OrdinalIgnoreCase))
                    {
                        var pieces = package.Split('-');
                        if (pieces.Length > 2)
                        {
                            var fromVersion = pieces[1];
                            var toVersion = pieces[2];
                            xmlToRun = selectedSolution.IncrementalXml.ToList().Find((x) =>
                              {
                                  return x.GetAttributeValue("BaseVersion").Equals(fromVersion.SafeToString(), StringComparison.OrdinalIgnoreCase) && x.GetAttributeValue("TargetVersion").Equals(toVersion.SafeToString(), StringComparison.OrdinalIgnoreCase);
                              });
                        }
                    }

                    if (xmlToRun != null)
                    {
                        SqlExecutor sqlExecutor = new SqlExecutor(outputDelegate, selectedSolution.ConnectionSetting, xmlToRun);
                        sqlExecutor.Execute(solutionName);
                    }
                    else
                    {
                        outputDelegate("No matched SQL package to run.");
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                outputDelegate(ex.Message);
            }
            finally
            {
                outputDelegate("Execution is ended at " + DateTime.Now + Environment.NewLine);
            }
        }
    }
}
