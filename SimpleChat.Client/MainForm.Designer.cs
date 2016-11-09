namespace SimpleChat.Client
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
            System.Windows.Forms.Panel panel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.panel = new System.Windows.Forms.Panel();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainMenu_Chat = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu_Chat_Login = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu_Chat_Register = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainMenu_Chat_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenu_Logout = new System.Windows.Forms.ToolStripMenuItem();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            this.panel.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(this.webBrowser);
            panel1.Location = new System.Drawing.Point(12, 28);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(550, 361);
            panel1.TabIndex = 7;
            // 
            // webBrowser
            // 
            this.webBrowser.AllowNavigation = false;
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.Size = new System.Drawing.Size(548, 359);
            this.webBrowser.TabIndex = 6;
            // 
            // panel
            // 
            this.panel.Controls.Add(panel1);
            this.panel.Controls.Add(this.lstUsers);
            this.panel.Controls.Add(this.txtMessage);
            this.panel.Controls.Add(this.btnSend);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Enabled = false;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(701, 463);
            this.panel.TabIndex = 0;
            // 
            // lstUsers
            // 
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.HorizontalScrollbar = true;
            this.lstUsers.Location = new System.Drawing.Point(569, 28);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(120, 420);
            this.lstUsers.TabIndex = 5;
            this.lstUsers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lstUsers_MouseUp);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(12, 395);
            this.txtMessage.MaxLength = 500;
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(469, 56);
            this.txtMessage.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(487, 395);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 56);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Отправить";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu_Chat});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(701, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // mainMenu_Chat
            // 
            this.mainMenu_Chat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenu_Chat_Login,
            this.mainMenu_Chat_Register,
            this.toolStripSeparator1,
            this.mainMenu_Chat_Exit});
            this.mainMenu_Chat.Name = "mainMenu_Chat";
            this.mainMenu_Chat.Size = new System.Drawing.Size(38, 20);
            this.mainMenu_Chat.Text = "&Чат";
            // 
            // mainMenu_Chat_Login
            // 
            this.mainMenu_Chat_Login.Name = "mainMenu_Chat_Login";
            this.mainMenu_Chat_Login.Size = new System.Drawing.Size(143, 22);
            this.mainMenu_Chat_Login.Text = "&Вход";
            this.mainMenu_Chat_Login.Click += new System.EventHandler(this.mainMenu_Chat_Login_Click);
            // 
            // mainMenu_Chat_Register
            // 
            this.mainMenu_Chat_Register.Name = "mainMenu_Chat_Register";
            this.mainMenu_Chat_Register.Size = new System.Drawing.Size(143, 22);
            this.mainMenu_Chat_Register.Text = "&Регистрация";
            this.mainMenu_Chat_Register.Click += new System.EventHandler(this.mainMenu_Chat_Register_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // mainMenu_Chat_Exit
            // 
            this.mainMenu_Chat_Exit.Name = "mainMenu_Chat_Exit";
            this.mainMenu_Chat_Exit.Size = new System.Drawing.Size(143, 22);
            this.mainMenu_Chat_Exit.Text = "В&ыход";
            this.mainMenu_Chat_Exit.Click += new System.EventHandler(this.mainMenu_Chat_Exit_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenu_Logout});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(162, 26);
            // 
            // contextMenu_Logout
            // 
            this.contextMenu_Logout.Name = "contextMenu_Logout";
            this.contextMenu_Logout.Size = new System.Drawing.Size(161, 22);
            this.contextMenu_Logout.Text = "Выгнать из чата";
            this.contextMenu_Logout.Click += new System.EventHandler(this.contextMenu_Logout_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 463);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "SimpleChat";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            panel1.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mainMenu_Chat;
        private System.Windows.Forms.ToolStripMenuItem mainMenu_Chat_Login;
        private System.Windows.Forms.ToolStripMenuItem mainMenu_Chat_Register;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mainMenu_Chat_Exit;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextMenu_Logout;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.WebBrowser webBrowser;
    }
}

