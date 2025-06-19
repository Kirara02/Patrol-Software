namespace Wm5000AEDemo
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnOpen = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.textse = new System.Windows.Forms.TextBox();
            this.textge = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 13);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 25);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open Usb";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 146);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(378, 248);
            this.textBox1.TabIndex = 9;
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(13, 72);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(117, 25);
            this.button9.TabIndex = 10;
            this.button9.Text = "Get records";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(145, 72);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(116, 25);
            this.button10.TabIndex = 11;
            this.button10.Text = "Erasure Records";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(13, 114);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 25);
            this.button13.TabIndex = 13;
            this.button13.Text = "Close Usb";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(277, 72);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(113, 25);
            this.button14.TabIndex = 14;
            this.button14.Text = "Adjust Time";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(13, 40);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(116, 25);
            this.button17.TabIndex = 20;
            this.button17.Text = "Set Equipment ID";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(215, 40);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(110, 25);
            this.button18.TabIndex = 21;
            this.button18.Text = "Get Equipment ID";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // textse
            // 
            this.textse.Location = new System.Drawing.Point(135, 40);
            this.textse.Name = "textse";
            this.textse.Size = new System.Drawing.Size(53, 20);
            this.textse.TabIndex = 22;
            this.textse.Text = "1234";
            // 
            // textge
            // 
            this.textge.Location = new System.Drawing.Point(331, 40);
            this.textge.Name = "textge";
            this.textge.Size = new System.Drawing.Size(59, 20);
            this.textge.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(315, 115);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 24;
            this.btnSave.Text = "Save Data";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 407);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.textge);
            this.Controls.Add(this.textse);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnOpen);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "5000A,E Demo";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.TextBox textse;
        private System.Windows.Forms.TextBox textge;
        private System.Windows.Forms.Button btnSave;
    }
}

