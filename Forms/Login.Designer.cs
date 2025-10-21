namespace ExperimentalManagementSystem
{
    partial class Login
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
            this.lbusername = new System.Windows.Forms.Label();
            this.lbpassword = new System.Windows.Forms.Label();
            this.tbusername = new System.Windows.Forms.TextBox();
            this.tbpassword = new System.Windows.Forms.TextBox();
            this.btlogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbusername
            // 
            this.lbusername.AutoSize = true;
            this.lbusername.Font = new System.Drawing.Font("宋体", 12F);
            this.lbusername.Location = new System.Drawing.Point(111, 139);
            this.lbusername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbusername.Name = "lbusername";
            this.lbusername.Size = new System.Drawing.Size(106, 24);
            this.lbusername.TabIndex = 0;
            this.lbusername.Text = "用户名：";
            // 
            // lbpassword
            // 
            this.lbpassword.AutoSize = true;
            this.lbpassword.Font = new System.Drawing.Font("宋体", 12F);
            this.lbpassword.Location = new System.Drawing.Point(111, 296);
            this.lbpassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbpassword.Name = "lbpassword";
            this.lbpassword.Size = new System.Drawing.Size(106, 24);
            this.lbpassword.TabIndex = 1;
            this.lbpassword.Text = "密  码：";
            // 
            // tbusername
            // 
            this.tbusername.Location = new System.Drawing.Point(233, 139);
            this.tbusername.Name = "tbusername";
            this.tbusername.Size = new System.Drawing.Size(468, 35);
            this.tbusername.TabIndex = 2;
            this.tbusername.TextChanged += new System.EventHandler(this.tbusername_TextChanged);
            // 
            // tbpassword
            // 
            this.tbpassword.Location = new System.Drawing.Point(233, 296);
            this.tbpassword.Name = "tbpassword";
            this.tbpassword.Size = new System.Drawing.Size(468, 35);
            this.tbpassword.TabIndex = 3;
            // 
            // btlogin
            // 
            this.btlogin.Location = new System.Drawing.Point(701, 385);
            this.btlogin.Name = "btlogin";
            this.btlogin.Size = new System.Drawing.Size(128, 71);
            this.btlogin.TabIndex = 4;
            this.btlogin.Text = "登录";
            this.btlogin.UseVisualStyleBackColor = true;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 499);
            this.Controls.Add(this.btlogin);
            this.Controls.Add(this.tbpassword);
            this.Controls.Add(this.tbusername);
            this.Controls.Add(this.lbpassword);
            this.Controls.Add(this.lbusername);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Login";
            this.Text = "登录";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbusername;
        private System.Windows.Forms.Label lbpassword;
        private System.Windows.Forms.TextBox tbusername;
        private System.Windows.Forms.TextBox tbpassword;
        private System.Windows.Forms.Button btlogin;
    }
}