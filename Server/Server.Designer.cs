namespace Server
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_IP = new System.Windows.Forms.TextBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.MemberList = new System.Windows.Forms.ListView();
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPassword = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TextBox_ServerLog = new System.Windows.Forms.TextBox();
            this.textBox_PortNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(518, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port : ";
            // 
            // textBox_IP
            // 
            this.textBox_IP.Location = new System.Drawing.Point(49, 22);
            this.textBox_IP.Name = "textBox_IP";
            this.textBox_IP.ReadOnly = true;
            this.textBox_IP.Size = new System.Drawing.Size(446, 25);
            this.textBox_IP.TabIndex = 2;
            this.textBox_IP.Text = "실행시, 서버 PC의 IP를 자동으로 입력 받아 와서 textBox에 표시";
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(679, 25);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(75, 23);
            this.button_Start.TabIndex = 3;
            this.button_Start.Text = "Start";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Member Account List";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Server Log";
            // 
            // MemberList
            // 
            this.MemberList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderIndex,
            this.columnHeaderID,
            this.columnHeaderPassword});
            this.MemberList.Location = new System.Drawing.Point(31, 85);
            this.MemberList.Name = "MemberList";
            this.MemberList.Size = new System.Drawing.Size(282, 294);
            this.MemberList.TabIndex = 6;
            this.MemberList.UseCompatibleStateImageBehavior = false;
            this.MemberList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.Text = "Index";
            // 
            // columnHeaderID
            // 
            this.columnHeaderID.Text = "ID";
            // 
            // columnHeaderPassword
            // 
            this.columnHeaderPassword.Text = "Password";
            this.columnHeaderPassword.Width = 98;
            // 
            // TextBox_ServerLog
            // 
            this.TextBox_ServerLog.Location = new System.Drawing.Point(354, 85);
            this.TextBox_ServerLog.Multiline = true;
            this.TextBox_ServerLog.Name = "TextBox_ServerLog";
            this.TextBox_ServerLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_ServerLog.Size = new System.Drawing.Size(400, 294);
            this.TextBox_ServerLog.TabIndex = 7;
            // 
            // textBox_PortNumber
            // 
            this.textBox_PortNumber.Location = new System.Drawing.Point(573, 21);
            this.textBox_PortNumber.Name = "textBox_PortNumber";
            this.textBox_PortNumber.Size = new System.Drawing.Size(100, 25);
            this.textBox_PortNumber.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 401);
            this.Controls.Add(this.textBox_PortNumber);
            this.Controls.Add(this.TextBox_ServerLog);
            this.Controls.Add(this.MemberList);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.textBox_IP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Mini Instagram Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_IP;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView MemberList;
        private System.Windows.Forms.TextBox TextBox_ServerLog;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderID;
        private System.Windows.Forms.ColumnHeader columnHeaderPassword;
        private System.Windows.Forms.TextBox textBox_PortNumber;
    }
}

