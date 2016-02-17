using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Beyova.MicrosoftSqlDeployTool.Core;

namespace Beyova.MicrosoftSqlDeployTool
{
    /// <summary>
    /// Class for form of Save Solution As
    /// </summary>
    public partial class SaveSolutionAs : Form
    {
        /// <summary>
        /// The XML document
        /// </summary>
        private XDocument xDocument = null;

        /// <summary>
        /// The main form
        /// </summary>
        private MainForm mainForm = null;

        /// <summary>
        /// The current solution
        /// </summary>
        private Solution currentSolution = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveSolutionAs"/> class.
        /// </summary>
        public SaveSolutionAs()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveSolutionAs" /> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="xDocument">The x document.</param>
        /// <param name="currentSolution">The current solution.</param>
        public SaveSolutionAs(MainForm mainForm, XDocument xDocument, Solution currentSolution)
        {
            this.mainForm = mainForm;
            this.xDocument = xDocument;
            this.currentSolution = currentSolution;

            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the SaveSolutionAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveSolutionAs_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btn_Save control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (xDocument != null)
            {
                var text = txt_SolutionName.Text;
                if (string.IsNullOrWhiteSpace(text))
                {
                    MessageBox.Show("Solution name can not be empty.", "Error", MessageBoxButtons.OK);
                }
                else
                {
                    text = text.Trim();
                    var nameMatchedItems = from one in xDocument.Root.Elements(Constants.Solution) where one.GetAttributeValue(Constants.Name).Equals(text, StringComparison.InvariantCultureIgnoreCase) select one;

                    if (nameMatchedItems.Count() > 0)
                    {
                        MessageBox.Show("Solution node with same name has already existed.", "Error", MessageBoxButtons.OK);
                    }
                    else
                    {
                        var solutionXml = currentSolution.ToXml();
                        solutionXml.SetAttributeValue(Constants.Name, text);
                        currentSolution.Name = text;
                        xDocument.Root.Add(solutionXml);

                        xDocument.SaveConfiguration();

                        if (this.mainForm != null)
                        {
                            this.mainForm.Invoke((MethodInvoker)delegate
                            {
                                this.mainForm.AddSolutionOption(currentSolution);
                            });
                        }

                        this.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_Cancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
