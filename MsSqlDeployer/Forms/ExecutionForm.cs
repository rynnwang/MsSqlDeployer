using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Beyova.MicrosoftSqlDeployTool.Core;

namespace Beyova.MicrosoftSqlDeployTool
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ExecutionForm : Form
    {
        /// <summary>
        /// The date time format
        /// </summary>
        private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        /// <summary>
        /// The main form
        /// </summary>
        protected MainForm mainForm = null;

        /// <summary>
        /// The SQL executor
        /// </summary>
        protected SqlExecutor sqlExecutor;

        /// <summary>
        /// The solution
        /// </summary>
        protected Solution solution = null;

        /// <summary>
        /// The running XML
        /// </summary>
        protected XElement runningXml = null;

        /// <summary>
        /// The work thread
        /// </summary>
        protected Thread workThread = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionForm"/> class.
        /// </summary>
        public ExecutionForm()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionForm" /> class.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="runningXml">The running XML.</param>
        public ExecutionForm(MainForm mainForm, XElement runningXml)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            if (mainForm != null && runningXml != null)
            {
                this.solution = mainForm.GetSelectedSolutionWithCurrentConnectionSettings();
                this.runningXml = runningXml;
            }

            this.outputText.LinkClicked += new LinkClickedEventHandler(this.outputText_LinkClicked);
        }

        /// <summary>
        /// Initializes the work thread.
        /// </summary>
        protected void InitializeWorkThread()
        {
            RichTextBox richTextBox = this.outputText;

            if (this.runningXml != null)
            {
                this.workThread = new Thread(new ParameterizedThreadStart(this.ExecuteTask));
                this.workThread.IsBackground = true;
                this.workThread.Start(this);
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the ExcutionForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void ExcutionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.workThread != null)
            {
                if (DialogResult.OK == MessageBox.Show("Closing the dialog would abort the execution. Are you sure to close?", "Confirm", MessageBoxButtons.YesNo))
                {
                    AbortTask();
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the ExcutionForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ExcutionForm_Load(object sender, EventArgs e)
        {
            this.btn_SaveReportAs.Enabled = this.btn_OK.Enabled = false;

            if (this.mainForm != null)
            {
                solution = this.mainForm.GetSelectedSolutionWithCurrentConnectionSettings();
            }

            InitializeWorkThread();
        }

        /// <summary>
        /// Executions the completed.
        /// </summary>
        private void ExecutionCompleted()
        {
            this.btn_OK.Enabled = true;
            this.btn_Abort.Enabled = false;
        }

        /// <summary>
        /// Handles the LinkClicked event of the outputText control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.LinkClickedEventArgs"/> instance containing the event data.</param>
        private void outputText_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            if (e.LinkText.StartsWith("http://") || e.LinkText.StartsWith("https://"))
            {
                Process.Start("IExplore.exe", e.LinkText);
            }
            else if (e.LinkText.StartsWith("file://"))
            {
                Process.Start("Explorer.exe", e.LinkText);
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_SaveReportAs control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btn_SaveReportAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();

            // Initialize the SaveFileDialog to specify the RTF extension for the file.
            saveFile1.DefaultExt = "*.rtf";
            saveFile1.Filter = "RTF Files|*.rtf";

            // Determine if the user selected a file name from the saveFileDialog.
            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {
                // Save the contents of the RichTextBox into the file.
                this.outputText.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);

                this.WriteOutput("Report saved at: file://" + (saveFile1.FileName.Replace('\\', '/')));
            }
        }

        /// <summary>
        /// Handles the Click event of the btn_OK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the Click event of the btn_Abort control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btn_Abort_Click(object sender, EventArgs e)
        {
            AbortTask();
        }

        /// <summary>
        /// Aborts the task.
        /// </summary>
        private void AbortTask()
        {
            if (workThread != null)
            {
                try
                {
                    workThread.Abort();
                }
                catch (Exception ex)
                {
                }
                SetButtonAsCompleted();
            }
        }

        /// <summary>
        /// Sets the button as completed.
        /// </summary>
        private void SetButtonAsCompleted()
        {
            this.btn_Abort.Enabled = false;
            this.btn_SaveReportAs.Enabled = this.btn_OK.Enabled = true;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        private void ExecuteTask(object formObject)
        {
            ExecutionForm form = formObject as ExecutionForm;

            if (form != null)
            {
                form.Invoke(new WriteOutputDelegate(WriteOutput), "-----------------------------------" + Environment.NewLine);
                form.Invoke(new WriteOutputDelegate(WriteOutput), "Execution is started at " + DateTime.Now.ToString(dateTimeFormat) + Environment.NewLine);

                try
                {
                    SqlExecutor sqlExecutor = new SqlExecutor(form.WriteOutputLine, form.solution.ConnectionSetting, form.runningXml);
                    sqlExecutor.Execute(form.solution.Name);
                    form.Invoke(new Action(SetButtonAsCompleted));

                }
                catch (ThreadAbortException taex)
                {
                    Thread.ResetAbort();
                    form.Invoke(new WriteOutputDelegate(WriteOutput), "Execution is aborted.");
                    form.Invoke(new Action(SetButtonAsCompleted));
                }
                catch (Exception ex)
                {
                    WriteOutput(ex.Message);
                }
                finally
                {
                    form.workThread = null;
                    form.Invoke(new WriteOutputDelegate(WriteOutput), "Execution is ended at " + DateTime.Now.ToString(dateTimeFormat) + Environment.NewLine);
                    form.Invoke(new WriteOutputDelegate(WriteOutput), "-----------------------------------" + Environment.NewLine);
                }
            }
        }

        ///// <summary>
        ///// Writes the output delegate.
        ///// </summary>
        ///// <param name="text">The text.</param>
        //private void WriteOutput(string text)
        //{
        //    this.outputText.AppendText(text.GetStringValue());
        //}

        /// <summary>
        /// Writes the output.
        /// </summary>
        /// <param name="text">The text.</param>
        private void WriteOutput(string text)
        {
            this.outputText.Invoke((MethodInvoker)
               (
                   () =>
                   {
                       this.outputText.AppendText(text.SafeToString());
                   }
               )
            );
        }

        /// <summary>
        /// Writes the output line.
        /// </summary>
        /// <param name="text">The text.</param>
        private void WriteOutputLine(string text)
        {
            WriteOutput(text.SafeToString() + Environment.NewLine);
        }
    }
}
