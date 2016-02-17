namespace Beyova.MicrosoftSqlDeployTool
{
    partial class MainForm
    {

        /// <summary>
        /// The components
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="T:System.Windows.Forms.Form" />.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows components

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.lblUID = new System.Windows.Forms.Label();
            this.txtUID = new System.Windows.Forms.TextBox();
            this.lblPwd = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtConnectionString = new System.Windows.Forms.TextBox();
            this.btnExecution = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rdBtnWindow = new System.Windows.Forms.RadioButton();
            this.rdBtnSQL = new System.Windows.Forms.RadioButton();
            this.lblCheckLogIn = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_TestConnection = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Advanced = new System.Windows.Forms.Button();
            this.combo_DBVerions = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.combo_Solutions = new System.Windows.Forms.ComboBox();
            this.btn_CopyCurrentSolutionAs = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtServerIP
            // 
            this.txtServerIP.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtServerIP.Location = new System.Drawing.Point(122, 64);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(205, 23);
            this.txtServerIP.TabIndex = 2;
            this.txtServerIP.Leave += new System.EventHandler(this.UpdateConnectionStringByValueChanged);
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerIP.Location = new System.Drawing.Point(12, 67);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(44, 15);
            this.lblServerIP.TabIndex = 1;
            this.lblServerIP.Text = "Server:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabase.Location = new System.Drawing.Point(12, 185);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(99, 15);
            this.lblDatabase.TabIndex = 3;
            this.lblDatabase.Text = "Database  Name:";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDatabase.Location = new System.Drawing.Point(122, 182);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(193, 23);
            this.txtDatabase.TabIndex = 8;
            this.txtDatabase.Leave += new System.EventHandler(this.UpdateConnectionStringByValueChanged);
            // 
            // lblUID
            // 
            this.lblUID.AutoSize = true;
            this.lblUID.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUID.Location = new System.Drawing.Point(12, 156);
            this.lblUID.Name = "lblUID";
            this.lblUID.Size = new System.Drawing.Size(69, 15);
            this.lblUID.TabIndex = 5;
            this.lblUID.Text = "User Name:";
            // 
            // txtUID
            // 
            this.txtUID.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUID.Location = new System.Drawing.Point(122, 153);
            this.txtUID.Name = "txtUID";
            this.txtUID.Size = new System.Drawing.Size(193, 23);
            this.txtUID.TabIndex = 6;
            this.txtUID.Leave += new System.EventHandler(this.UpdateConnectionStringByValueChanged);
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPwd.Location = new System.Drawing.Point(321, 156);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(67, 15);
            this.lblPwd.TabIndex = 7;
            this.lblPwd.Text = "Password: ";
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPwd.Location = new System.Drawing.Point(398, 153);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(241, 23);
            this.txtPwd.TabIndex = 7;
            this.txtPwd.Leave += new System.EventHandler(this.UpdateConnectionStringByValueChanged);
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.Location = new System.Drawing.Point(122, 35);
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Size = new System.Drawing.Size(517, 23);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Leave += new System.EventHandler(this.txtConnectionString_Leave);
            // 
            // btnExecution
            // 
            this.btnExecution.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecution.Location = new System.Drawing.Point(486, 211);
            this.btnExecution.Name = "btnExecution";
            this.btnExecution.Size = new System.Drawing.Size(153, 36);
            this.btnExecution.TabIndex = 21;
            this.btnExecution.Text = "Execute";
            this.btnExecution.UseVisualStyleBackColor = true;
            this.btnExecution.Click += new System.EventHandler(this.btnExecution_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection:";
            // 
            // rdBtnWindow
            // 
            this.rdBtnWindow.AutoSize = true;
            this.rdBtnWindow.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnWindow.Location = new System.Drawing.Point(7, 17);
            this.rdBtnWindow.Name = "rdBtnWindow";
            this.rdBtnWindow.Size = new System.Drawing.Size(159, 19);
            this.rdBtnWindow.TabIndex = 4;
            this.rdBtnWindow.TabStop = true;
            this.rdBtnWindow.Text = "Windows Authentication";
            this.rdBtnWindow.UseVisualStyleBackColor = true;
            this.rdBtnWindow.Click += new System.EventHandler(this.rdBtnWindow_Click);
            // 
            // rdBtnSQL
            // 
            this.rdBtnSQL.AutoSize = true;
            this.rdBtnSQL.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnSQL.Location = new System.Drawing.Point(248, 17);
            this.rdBtnSQL.Name = "rdBtnSQL";
            this.rdBtnSQL.Size = new System.Drawing.Size(164, 19);
            this.rdBtnSQL.TabIndex = 5;
            this.rdBtnSQL.TabStop = true;
            this.rdBtnSQL.Text = "SQL Server Authentication";
            this.rdBtnSQL.UseVisualStyleBackColor = true;
            this.rdBtnSQL.Click += new System.EventHandler(this.rdBtnSQL_Click);
            // 
            // lblCheckLogIn
            // 
            this.lblCheckLogIn.AutoSize = true;
            this.lblCheckLogIn.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckLogIn.Location = new System.Drawing.Point(12, 112);
            this.lblCheckLogIn.Name = "lblCheckLogIn";
            this.lblCheckLogIn.Size = new System.Drawing.Size(89, 15);
            this.lblCheckLogIn.TabIndex = 17;
            this.lblCheckLogIn.Text = "Authentication:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(321, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Version:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdBtnWindow);
            this.groupBox1.Controls.Add(this.rdBtnSQL);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(122, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 44);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // btn_TestConnection
            // 
            this.btn_TestConnection.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TestConnection.Location = new System.Drawing.Point(15, 211);
            this.btn_TestConnection.Name = "btn_TestConnection";
            this.btn_TestConnection.Size = new System.Drawing.Size(147, 36);
            this.btn_TestConnection.TabIndex = 20;
            this.btn_TestConnection.Text = "Test Connection";
            this.btn_TestConnection.UseVisualStyleBackColor = true;
            this.btn_TestConnection.Click += new System.EventHandler(this.btn_TestConnection_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(333, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "(Sample: .\\INSTANCE, OR IP\\INSTANCE, PORT)";
            // 
            // btn_Advanced
            // 
            this.btn_Advanced.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Advanced.Location = new System.Drawing.Point(327, 211);
            this.btn_Advanced.Name = "btn_Advanced";
            this.btn_Advanced.Size = new System.Drawing.Size(153, 36);
            this.btn_Advanced.TabIndex = 27;
            this.btn_Advanced.Text = "Advanced Settings";
            this.btn_Advanced.UseVisualStyleBackColor = true;
            this.btn_Advanced.Click += new System.EventHandler(this.btn_Advanced_Click);
            // 
            // combo_DBVerions
            // 
            this.combo_DBVerions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_DBVerions.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combo_DBVerions.FormattingEnabled = true;
            this.combo_DBVerions.Location = new System.Drawing.Point(398, 182);
            this.combo_DBVerions.Name = "combo_DBVerions";
            this.combo_DBVerions.Size = new System.Drawing.Size(241, 23);
            this.combo_DBVerions.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 15);
            this.label4.TabIndex = 30;
            this.label4.Text = "Solution:";
            // 
            // combo_Solutions
            // 
            this.combo_Solutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_Solutions.FormattingEnabled = true;
            this.combo_Solutions.Location = new System.Drawing.Point(122, 6);
            this.combo_Solutions.Name = "combo_Solutions";
            this.combo_Solutions.Size = new System.Drawing.Size(517, 23);
            this.combo_Solutions.TabIndex = 31;
            this.combo_Solutions.SelectedIndexChanged += new System.EventHandler(this.combo_Solutions_SelectedIndexChanged);
            // 
            // btn_CopyCurrentSolutionAs
            // 
            this.btn_CopyCurrentSolutionAs.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CopyCurrentSolutionAs.Location = new System.Drawing.Point(168, 211);
            this.btn_CopyCurrentSolutionAs.Name = "btn_CopyCurrentSolutionAs";
            this.btn_CopyCurrentSolutionAs.Size = new System.Drawing.Size(153, 36);
            this.btn_CopyCurrentSolutionAs.TabIndex = 32;
            this.btn_CopyCurrentSolutionAs.Text = "Copy Current Solution As";
            this.btn_CopyCurrentSolutionAs.UseVisualStyleBackColor = true;
            this.btn_CopyCurrentSolutionAs.Click += new System.EventHandler(this.btn_CopyCurrentSolutionAs_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 268);
            this.Controls.Add(this.btn_CopyCurrentSolutionAs);
            this.Controls.Add(this.combo_Solutions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.combo_DBVerions);
            this.Controls.Add(this.btn_Advanced);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_TestConnection);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCheckLogIn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExecution);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.lblPwd);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.lblUID);
            this.Controls.Add(this.txtUID);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.txtServerIP);
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SQL Deploy Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label lblUID;
        private System.Windows.Forms.TextBox txtUID;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtConnectionString;
        private System.Windows.Forms.Button btnExecution;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdBtnWindow;
        private System.Windows.Forms.RadioButton rdBtnSQL;
        private System.Windows.Forms.Label lblCheckLogIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_TestConnection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Advanced;
        private System.Windows.Forms.ComboBox combo_DBVerions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox combo_Solutions;
        private System.Windows.Forms.Button btn_CopyCurrentSolutionAs;
    }
}

