using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Linq;
using Beyova.MicrosoftSqlDeployTool.Core;
using Beyova.MicrosoftSqlDeployTool.Properties;

namespace Beyova.MicrosoftSqlDeployTool
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The is windows authentication
        /// </summary>
        protected bool isWindowsAuthentication = false;

        /// <summary>
        /// The x document
        /// </summary>
        protected XDocument xDocument = null;

        #region Properties

        /// <summary>
        /// The solutions
        /// </summary>
        public Dictionary<string, Solution> Solutions
        {
            get;
            protected set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.Solutions = new Dictionary<string, Solution>();
        }

        #endregion

        #region Control events

        /// <summary>
        /// Handles the Click event of the rdBtnSQL control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rdBtnSQL_Click(object sender, EventArgs e)
        {
            isWindowsAuthentication = false;
            ChooseLogType(isWindowsAuthentication);
            this.txtConnectionString.Text = GetConnectionStringByControl(isWindowsAuthentication);
        }

        /// <summary>
        /// Handles the Leave event of the txtConnectionString control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void txtConnectionString_Leave(object sender, EventArgs e)
        {
            ConnectionStringModel connectionStringModel = ConnectionStringModel.LoadConnectionString(this.txtConnectionString.Text);
            SetControlValueByConnectionStringModel(connectionStringModel);
        }

        /// <summary>
        /// Handles the Click event of the btn_TestConnection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_TestConnection_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;

            string versionInfo = string.Empty;
            try
            {
                versionInfo = ScriptHelper.TestConnection(GetConnectionStringModelByControl().ToConnectionString());
                isSuccess = true;
            }
            catch (Exception ex)
            {
                versionInfo = ex.Message;
            }
            MessageBox.Show(versionInfo, isSuccess ? "Success" : "Failed", MessageBoxButtons.OK, isSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }

        /// <summary>
        /// Handles the Click event of the btn_Advanced control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btn_Advanced_Click(object sender, EventArgs e)
        {
            AdvancedForm form = new AdvancedForm(GetSelectedVersionXml());
            form.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the btn_CopyCurrentSolutionAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_CopyCurrentSolutionAs_Click(object sender, EventArgs e)
        {
            var solution = GetSelectedSolution();
            if (solution != null)
            {
                solution = solution.Clone() as Solution;
                solution.ConnectionSetting = GetConnectionStringModelByControl();
                SaveSolutionAs form = new SaveSolutionAs(this, this.xDocument, solution);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the combo_Solutions control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void combo_Solutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            Solution solution = GetSelectedSolution();

            if (solution != null)
            {
                DisplaySolution(solution);
            }
        }

        /// <summary>
        /// Handles the Load event of the MainForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Bitmap bmp = Resources.Logo;
            this.Icon = Icon.FromHandle(bmp.GetHicon());

            InitializeSolutions();
        }

        /// <summary>
        /// Handles the Click event of the btnExit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of the btnImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnExecution_Click(object sender, EventArgs e)
        {
            ExecutionForm form = new ExecutionForm(this, this.GetSelectedVersionXml());
            form.ShowDialog();
        }

        /// <summary>
        /// Handles the Click event of the rdBtnWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void rdBtnWindow_Click(object sender, EventArgs e)
        {
            isWindowsAuthentication = true;
            ChooseLogType(isWindowsAuthentication);
            this.txtConnectionString.Text = GetConnectionStringByControl(isWindowsAuthentication);
        }

        #endregion

        /// <summary>
        /// Initializes the solutions.
        /// </summary>
        private void InitializeSolutions()
        {
            xDocument = XDocument.Load(ConfigurationHelper.ConfigPath, LoadOptions.None);
            InitializeSolutions(this.combo_Solutions, xDocument);
        }

        /// <summary>
        /// Initializes the solutions.
        /// </summary>
        /// <param name="solutionComboBox">The solution combo box.</param>
        /// <param name="configurationRootNode">The configuration root node.</param>
        private void InitializeSolutions(ComboBox solutionComboBox, XDocument configurationDocument)
        {
            if (solutionComboBox != null && configurationDocument != null)
            {
                solutionComboBox.Items.Clear();

                if (configurationDocument.Root.Name.LocalName == Constants.Database)
                {
                    var solutions = ConfigurationHelper.LoadSolutions(configurationDocument);

                    if (solutions.Count > 0)
                    {
                        foreach (var one in solutions)
                        {
                            solutionComboBox.Items.Add(new ComboboxItem { Text = one.Name.SafeToString(), Value = one });
                        }

                        solutionComboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        solutionComboBox.Items.Add(new ComboboxItem { Text = "Default", Value = new Solution() { Name = "Default" } });
                    }
                }
            }
        }

        /// <summary>
        /// Displays the solution.
        /// </summary>
        /// <param name="solutionName">Name of the solution.</param>
        private void DisplaySolution(string solutionName)
        {
            Solution solution;

            if (this.Solutions.TryGetValue(solutionName, out solution))
            {
                DisplaySolution(solution);
            }
        }

        /// <summary>
        /// Displays the solution.
        /// </summary>
        /// <param name="solutionName">Name of the solution.</param>
        private void DisplaySolution(Solution solution)
        {
            if (solution != null)
            {
                var model = solution.ConnectionSetting;

                SetControlValueByConnectionStringModel(model);
                isWindowsAuthentication = string.IsNullOrWhiteSpace(model.UserName);
                this.txtConnectionString.Text = GetConnectionStringByControl(isWindowsAuthentication);
                ChooseLogType(isWindowsAuthentication);

                InitializeVersionCombo(solution.FullXml, solution.IncrementalXml);
            }
        }

        /// <summary>
        /// Updates the connection string by value changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UpdateConnectionStringByValueChanged(object sender, EventArgs e)
        {
            ConnectionStringModel model = GetConnectionStringModelByControl();
            this.txtConnectionString.Text = GetConnectionStringByControl(isWindowsAuthentication);
        }

        /// <summary>
        /// Gets the connection string model by control.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns></returns>
        public ConnectionStringModel GetConnectionStringModelByControl()
        {
            ConnectionStringModel model = new ConnectionStringModel();
            model.Server = this.txtServerIP.Text;
            model.Database = this.txtDatabase.Text;
            model.UserName = this.txtUID.Text;
            model.Password = this.txtPwd.Text;

            if (rdBtnWindow.Checked)
            {
                model.UserName = model.Password = string.Empty;
            }
            return model;
        }

        /// <summary>
        /// Sets the control value by connection string model.
        /// </summary>
        /// <param name="connectionStringModel">The connection string model.</param>
        private void SetControlValueByConnectionStringModel(ConnectionStringModel connectionStringModel)
        {
            this.txtServerIP.Text = connectionStringModel.Server;
            this.txtDatabase.Text = connectionStringModel.Database;
            this.txtUID.Text = connectionStringModel.UserName;
            this.txtPwd.Text = connectionStringModel.Password;

            this.rdBtnWindow.Checked = connectionStringModel.IsWindowsAuthentication;
        }

        /// <summary>
        /// Gets the connection string by control.
        /// </summary>
        /// <param name="isWindowsAuthentication">if set to <c>true</c> [is windows authentication].</param>
        /// <returns></returns>
        private string GetConnectionStringByControl(bool isWindowsAuthentication)
        {
            ConnectionStringModel model = GetConnectionStringModelByControl();
            if (isWindowsAuthentication)
            {
                model.UserName = model.Password = string.Empty;
            }
            return model.ToConnectionString();
        }

        /// <summary>
        /// Chooses the type of the log.
        /// </summary>
        /// <param name="isWindowsAuthentication">if set to <c>true</c> [is windows authentication].</param>
        private void ChooseLogType(bool isWindowsAuthentication)
        {
            this.rdBtnWindow.Checked = isWindowsAuthentication;
            this.rdBtnSQL.Checked = !isWindowsAuthentication;
            this.txtPwd.ReadOnly = this.txtUID.ReadOnly = isWindowsAuthentication;
        }

        /// <summary>
        /// Initializes the version combo.
        /// </summary>
        /// <param name="xElements">The x elements.</param>
        private void InitializeVersionCombo(XElement fullNode, IEnumerable<XElement> incrementalNodes)
        {
            const string format = "{0} ( {1} -> {2} )";
            this.combo_DBVerions.Items.Clear();

            this.combo_DBVerions.Items.Add(new ComboboxItem { Value = fullNode.GenerateIdString(), Text = string.Format(format, "Full", "x", fullNode.Attribute(Constants.TargetVersion).Value) });

            foreach (var one in incrementalNodes)
            {
                this.combo_DBVerions.Items.Add(new ComboboxItem { Value = one.GenerateIdString(), Text = string.Format(format, "Incremental", one.Attribute(Constants.BaseVersion).Value, one.Attribute(Constants.TargetVersion).Value) });
            }

            this.combo_DBVerions.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets the selected version XML.
        /// </summary>
        /// <returns></returns>
        private XElement GetSelectedVersionXml()
        {
            XElement result = null;
            ComboboxItem selectedVersion = this.combo_DBVerions.SelectedItem as ComboboxItem;
            string idString = selectedVersion.Value as string;

            var selectedSolution = GetSelectedSolution();

            if (selectedSolution != null)
            {
                if (selectedSolution.FullXml.GenerateIdString() == idString)
                {
                    result = selectedSolution.FullXml;
                }
                else if (selectedSolution.IncrementalXml != null)
                {
                    result = (from one in selectedSolution.IncrementalXml
                              where one.GenerateIdString() == idString
                              select one).FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the selected solution.
        /// </summary>
        /// <returns></returns>
        private Solution GetSelectedSolution()
        {
            ComboboxItem selectedSolution = ((ComboBox)this.combo_Solutions).SelectedItem as ComboboxItem;
            return selectedSolution.Value as Solution;
        }

        /// <summary>
        /// Gets the selected solution.
        /// </summary>
        /// <returns></returns>
        public Solution GetSelectedSolutionWithCurrentConnectionSettings()
        {
            Solution soltuion = GetSelectedSolution();
            if (soltuion != null)
            {
                soltuion.ConnectionSetting = GetConnectionStringModelByControl();
            }

            return soltuion;
        }

        /// <summary>
        /// Adds the solution option.
        /// </summary>
        /// <param name="solution">The solution.</param>
        public void AddSolutionOption(Solution solution)
        {
            if (solution != null)
            {
                ComboboxItem item = new ComboboxItem() { Text = solution.Name.SafeToString(), Value = solution };
                this.combo_Solutions.Items.Add(item);
            }
        }
    }

}
