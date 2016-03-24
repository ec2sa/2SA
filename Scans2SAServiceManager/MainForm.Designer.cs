namespace Scans2SAServiceManager
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.konToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.zainstalujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odinstalujToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konfiguracjaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.koniecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBackupDirectory = new System.Windows.Forms.Button();
            this.tbBackupDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnScansDirectory = new System.Windows.Forms.Button();
            this.tbScansDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbServiceUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Zarządzanie usługą RemoteScans";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konToolStripMenuItem,
            this.konfiguracjaToolStripMenuItem,
            this.toolStripMenuItem1,
            this.koniecToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 76);
            // 
            // konToolStripMenuItem
            // 
            this.konToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.zainstalujToolStripMenuItem,
            this.odinstalujToolStripMenuItem});
            this.konToolStripMenuItem.Name = "konToolStripMenuItem";
            this.konToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.konToolStripMenuItem.Text = "Usługa";
            this.konToolStripMenuItem.DropDownOpening += new System.EventHandler(this.konToolStripMenuItem_DropDownOpening);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem1
            // 
            this.stopToolStripMenuItem1.Name = "stopToolStripMenuItem1";
            this.stopToolStripMenuItem1.Size = new System.Drawing.Size(128, 22);
            this.stopToolStripMenuItem1.Text = "Stop";
            this.stopToolStripMenuItem1.Click += new System.EventHandler(this.stopToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(125, 6);
            // 
            // zainstalujToolStripMenuItem
            // 
            this.zainstalujToolStripMenuItem.Name = "zainstalujToolStripMenuItem";
            this.zainstalujToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.zainstalujToolStripMenuItem.Text = "Zainstaluj";
            this.zainstalujToolStripMenuItem.Click += new System.EventHandler(this.zainstalujToolStripMenuItem_Click);
            // 
            // odinstalujToolStripMenuItem
            // 
            this.odinstalujToolStripMenuItem.Name = "odinstalujToolStripMenuItem";
            this.odinstalujToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.odinstalujToolStripMenuItem.Text = "Odinstaluj";
            this.odinstalujToolStripMenuItem.Click += new System.EventHandler(this.odinstalujToolStripMenuItem_Click);
            // 
            // konfiguracjaToolStripMenuItem
            // 
            this.konfiguracjaToolStripMenuItem.Name = "konfiguracjaToolStripMenuItem";
            this.konfiguracjaToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.konfiguracjaToolStripMenuItem.Text = "Konfiguracja...";
            this.konfiguracjaToolStripMenuItem.Click += new System.EventHandler(this.konfiguracjaToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
            // 
            // koniecToolStripMenuItem
            // 
            this.koniecToolStripMenuItem.Name = "koniecToolStripMenuItem";
            this.koniecToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.koniecToolStripMenuItem.Text = "Koniec";
            this.koniecToolStripMenuItem.Click += new System.EventHandler(this.koniecToolStripMenuItem_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(289, 249);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBackupDirectory);
            this.groupBox1.Controls.Add(this.tbBackupDirectory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnScansDirectory);
            this.groupBox1.Controls.Add(this.tbScansDirectory);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(445, 128);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Konfiguracja katalogów";
            // 
            // btnBackupDirectory
            // 
            this.btnBackupDirectory.Location = new System.Drawing.Point(404, 80);
            this.btnBackupDirectory.Name = "btnBackupDirectory";
            this.btnBackupDirectory.Size = new System.Drawing.Size(26, 23);
            this.btnBackupDirectory.TabIndex = 5;
            this.btnBackupDirectory.Text = "...";
            this.btnBackupDirectory.UseVisualStyleBackColor = true;
            this.btnBackupDirectory.Click += new System.EventHandler(this.btnBackupDirectory_Click);
            // 
            // tbBackupDirectory
            // 
            this.tbBackupDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Scans2SAServiceManager.Properties.Settings.Default, "BackupDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbBackupDirectory.Location = new System.Drawing.Point(15, 81);
            this.tbBackupDirectory.Name = "tbBackupDirectory";
            this.tbBackupDirectory.Size = new System.Drawing.Size(378, 20);
            this.tbBackupDirectory.TabIndex = 4;
            this.tbBackupDirectory.Text = global::Scans2SAServiceManager.Properties.Settings.Default.BackupDirectory;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Katalog archiwum";
            // 
            // btnScansDirectory
            // 
            this.btnScansDirectory.Location = new System.Drawing.Point(404, 36);
            this.btnScansDirectory.Name = "btnScansDirectory";
            this.btnScansDirectory.Size = new System.Drawing.Size(26, 23);
            this.btnScansDirectory.TabIndex = 2;
            this.btnScansDirectory.Text = "...";
            this.btnScansDirectory.UseVisualStyleBackColor = true;
            this.btnScansDirectory.Click += new System.EventHandler(this.btnScansDirectory_Click);
            // 
            // tbScansDirectory
            // 
            this.tbScansDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Scans2SAServiceManager.Properties.Settings.Default, "WatchDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbScansDirectory.Location = new System.Drawing.Point(15, 37);
            this.tbScansDirectory.Name = "tbScansDirectory";
            this.tbScansDirectory.Size = new System.Drawing.Size(378, 20);
            this.tbScansDirectory.TabIndex = 1;
            this.tbScansDirectory.Text = global::Scans2SAServiceManager.Properties.Settings.Default.WatchDirectory;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Katalog skanów";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbPassword);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbUsername);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbServiceUrl);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(445, 115);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Konfiguracja usługi sieciowej";
            // 
            // tbPassword
            // 
            this.tbPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Scans2SAServiceManager.Properties.Settings.Default, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbPassword.Location = new System.Drawing.Point(132, 81);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(99, 20);
            this.tbPassword.TabIndex = 6;
            this.tbPassword.Text = global::Scans2SAServiceManager.Properties.Settings.Default.Password;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Hasło";
            // 
            // tbUsername
            // 
            this.tbUsername.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Scans2SAServiceManager.Properties.Settings.Default, "Username", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbUsername.Location = new System.Drawing.Point(15, 81);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(99, 20);
            this.tbUsername.TabIndex = 4;
            this.tbUsername.Text = global::Scans2SAServiceManager.Properties.Settings.Default.Username;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nazwa użytkownika";
            // 
            // tbServiceUrl
            // 
            this.tbServiceUrl.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Scans2SAServiceManager.Properties.Settings.Default, "ServiceUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbServiceUrl.Location = new System.Drawing.Point(15, 37);
            this.tbServiceUrl.Name = "tbServiceUrl";
            this.tbServiceUrl.Size = new System.Drawing.Size(378, 20);
            this.tbServiceUrl.TabIndex = 1;
            this.tbServiceUrl.Text = global::Scans2SAServiceManager.Properties.Settings.Default.ServiceUrl;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "URL usługi";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(370, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Anuluj";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(445, 279);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOK);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Konfiguracja usługi RemoteScans";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem konfiguracjaToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem koniecToolStripMenuItem;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolStripMenuItem konToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem zainstalujToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem odinstalujToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbScansDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBackupDirectory;
        private System.Windows.Forms.TextBox tbBackupDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnScansDirectory;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbServiceUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
    }
}