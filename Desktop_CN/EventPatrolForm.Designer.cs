namespace sample_CSharp2008
{
    partial class EventPatrolForm
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.lbl_SeverIP = new System.Windows.Forms.Label();
            this.txtPSWD = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.lbl_pass = new System.Windows.Forms.Label();
            this.lbl_ServerIP_Des = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lbl_netport = new System.Windows.Forms.Label();
            this.lbl_user = new System.Windows.Forms.Label();
            this.lbl_netport_des = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.lbl_netname = new System.Windows.Forms.Label();
            this.txtAP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 29);
            this.button2.TabIndex = 2;
            this.button2.Text = "Get Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(10, 16);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 66);
            this.button1.TabIndex = 3;
            this.button1.Text = "Active Device";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(218, 16);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(420, 264);
            this.listBox1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(26, 104);
            this.button3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 29);
            this.button3.TabIndex = 5;
            this.button3.Text = "GetDeviceID";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(26, 139);
            this.button4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(161, 29);
            this.button4.TabIndex = 6;
            this.button4.Text = "GetDeviceClock";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(26, 173);
            this.button5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(161, 29);
            this.button5.TabIndex = 7;
            this.button5.Text = "SetDeviceClock";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(26, 371);
            this.button6.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(161, 29);
            this.button6.TabIndex = 8;
            this.button6.Text = "GetNetConfig";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(26, 285);
            this.button7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(161, 29);
            this.button7.TabIndex = 9;
            this.button7.Text = "SetTagInfo";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(255, 293);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "00916FCF";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(215, 296);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "TagID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 296);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "TagName:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(442, 293);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(120, 20);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "Office";
            // 
            // lbl_SeverIP
            // 
            this.lbl_SeverIP.AutoSize = true;
            this.lbl_SeverIP.Location = new System.Drawing.Point(215, 341);
            this.lbl_SeverIP.Name = "lbl_SeverIP";
            this.lbl_SeverIP.Size = new System.Drawing.Size(125, 13);
            this.lbl_SeverIP.TabIndex = 43;
            this.lbl_SeverIP.Text = "Server IP/ domain name:";
            // 
            // txtPSWD
            // 
            this.txtPSWD.Location = new System.Drawing.Point(387, 499);
            this.txtPSWD.Name = "txtPSWD";
            this.txtPSWD.Size = new System.Drawing.Size(175, 20);
            this.txtPSWD.TabIndex = 54;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(387, 333);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(175, 20);
            this.txtIP.TabIndex = 44;
            // 
            // lbl_pass
            // 
            this.lbl_pass.AutoSize = true;
            this.lbl_pass.Location = new System.Drawing.Point(215, 502);
            this.lbl_pass.Name = "lbl_pass";
            this.lbl_pass.Size = new System.Drawing.Size(62, 13);
            this.lbl_pass.TabIndex = 53;
            this.lbl_pass.Text = "PassWord：";
            // 
            // lbl_ServerIP_Des
            // 
            this.lbl_ServerIP_Des.AutoSize = true;
            this.lbl_ServerIP_Des.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_ServerIP_Des.Location = new System.Drawing.Point(385, 358);
            this.lbl_ServerIP_Des.Name = "lbl_ServerIP_Des";
            this.lbl_ServerIP_Des.Size = new System.Drawing.Size(220, 13);
            this.lbl_ServerIP_Des.TabIndex = 45;
            this.lbl_ServerIP_Des.Text = "For example:111.111.11.11 | www.baidu.com";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(387, 461);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(175, 20);
            this.txtUser.TabIndex = 52;
            // 
            // lbl_netport
            // 
            this.lbl_netport.AutoSize = true;
            this.lbl_netport.Location = new System.Drawing.Point(215, 380);
            this.lbl_netport.Name = "lbl_netport";
            this.lbl_netport.Size = new System.Drawing.Size(62, 13);
            this.lbl_netport.TabIndex = 46;
            this.lbl_netport.Text = "Server port:";
            // 
            // lbl_user
            // 
            this.lbl_user.AutoSize = true;
            this.lbl_user.Location = new System.Drawing.Point(215, 464);
            this.lbl_user.Name = "lbl_user";
            this.lbl_user.Size = new System.Drawing.Size(63, 13);
            this.lbl_user.TabIndex = 51;
            this.lbl_user.Text = "UserName：";
            // 
            // lbl_netport_des
            // 
            this.lbl_netport_des.AutoSize = true;
            this.lbl_netport_des.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lbl_netport_des.Location = new System.Drawing.Point(385, 402);
            this.lbl_netport_des.Name = "lbl_netport_des";
            this.lbl_netport_des.Size = new System.Drawing.Size(91, 13);
            this.lbl_netport_des.TabIndex = 47;
            this.lbl_netport_des.Text = "For example:8888";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(387, 377);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(175, 20);
            this.txtPort.TabIndex = 49;
            // 
            // lbl_netname
            // 
            this.lbl_netname.AutoSize = true;
            this.lbl_netname.Location = new System.Drawing.Point(215, 419);
            this.lbl_netname.Name = "lbl_netname";
            this.lbl_netname.Size = new System.Drawing.Size(69, 13);
            this.lbl_netname.TabIndex = 48;
            this.lbl_netname.Text = "AccessPoint:";
            // 
            // txtAP
            // 
            this.txtAP.Location = new System.Drawing.Point(387, 419);
            this.txtAP.Name = "txtAP";
            this.txtAP.Size = new System.Drawing.Size(175, 20);
            this.txtAP.TabIndex = 55;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(385, 445);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "For example:UNINET";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(26, 413);
            this.button8.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(161, 29);
            this.button8.TabIndex = 57;
            this.button8.Text = "SetNetConfig";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(26, 245);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(161, 29);
            this.button9.TabIndex = 58;
            this.button9.Text = "Erase Data";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(26, 321);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(161, 32);
            this.button10.TabIndex = 59;
            this.button10.Text = "Save Data";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // EventPatrolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 528);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAP);
            this.Controls.Add(this.lbl_SeverIP);
            this.Controls.Add(this.txtPSWD);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lbl_pass);
            this.Controls.Add(this.lbl_ServerIP_Des);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lbl_netport);
            this.Controls.Add(this.lbl_user);
            this.Controls.Add(this.lbl_netport_des);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lbl_netname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button10);
            this.Name = "EventPatrolForm";
            this.Text = "EventPatrolForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label lbl_SeverIP;
        private System.Windows.Forms.TextBox txtPSWD;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label lbl_pass;
        private System.Windows.Forms.Label lbl_ServerIP_Des;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lbl_netport;
        private System.Windows.Forms.Label lbl_user;
        private System.Windows.Forms.Label lbl_netport_des;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label lbl_netname;
        private System.Windows.Forms.TextBox txtAP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
    }
}