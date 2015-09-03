namespace ifunction.MicrosoftSqlDeployTool
{
    partial class ExecutionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.outputText = new System.Windows.Forms.RichTextBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Abort = new System.Windows.Forms.Button();
            this.btn_SaveReportAs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outputText
            // 
            this.outputText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputText.Location = new System.Drawing.Point(12, 12);
            this.outputText.Name = "outputText";
            this.outputText.ReadOnly = true;
            this.outputText.Size = new System.Drawing.Size(523, 325);
            this.outputText.TabIndex = 1;
            this.outputText.Text = "";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(460, 343);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Abort
            // 
            this.btn_Abort.Location = new System.Drawing.Point(379, 343);
            this.btn_Abort.Name = "btn_Abort";
            this.btn_Abort.Size = new System.Drawing.Size(75, 23);
            this.btn_Abort.TabIndex = 3;
            this.btn_Abort.Text = "Abort";
            this.btn_Abort.UseVisualStyleBackColor = true;
            this.btn_Abort.Click += new System.EventHandler(this.btn_Abort_Click);
            // 
            // btn_SaveReportAs
            // 
            this.btn_SaveReportAs.Location = new System.Drawing.Point(12, 343);
            this.btn_SaveReportAs.Name = "btn_SaveReportAs";
            this.btn_SaveReportAs.Size = new System.Drawing.Size(123, 23);
            this.btn_SaveReportAs.TabIndex = 4;
            this.btn_SaveReportAs.Text = "Save Report As";
            this.btn_SaveReportAs.UseVisualStyleBackColor = true;
            this.btn_SaveReportAs.Click += new System.EventHandler(this.btn_SaveReportAs_Click);
            // 
            // ExecutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 376);
            this.Controls.Add(this.btn_SaveReportAs);
            this.Controls.Add(this.btn_Abort);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.outputText);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExecutionForm";
            this.ShowIcon = false;
            this.Text = "Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExcutionForm_FormClosing);
            this.Load += new System.EventHandler(this.ExcutionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox outputText;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Abort;
        private System.Windows.Forms.Button btn_SaveReportAs;
    }
}