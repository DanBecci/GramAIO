namespace GramAIO
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
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.loginLbl = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.usernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.usernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(173)))), ((int)(((byte)(80)))));
            this.usernameTextBox.Location = new System.Drawing.Point(145, 67);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(267, 38);
            this.usernameTextBox.TabIndex = 0;
            this.usernameTextBox.Text = "Username";
            this.usernameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.usernameTextBox.Click += new System.EventHandler(this.usernameTextBox_Click);
            // 
            // loginLbl
            // 
            this.loginLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.loginLbl.AutoSize = true;
            this.loginLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(42)))), ((int)(((byte)(141)))));
            this.loginLbl.Location = new System.Drawing.Point(214, 14);
            this.loginLbl.Name = "loginLbl";
            this.loginLbl.Size = new System.Drawing.Size(132, 37);
            this.loginLbl.TabIndex = 1;
            this.loginLbl.Text = "LOGIN:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.passwordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.passwordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(173)))), ((int)(((byte)(80)))));
            this.passwordTextBox.Location = new System.Drawing.Point(145, 117);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(267, 38);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.Text = "Password";
            this.passwordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.passwordTextBox.Click += new System.EventHandler(this.passwordTextBox_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(173)))), ((int)(((byte)(80)))));
            this.loginBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(173)))), ((int)(((byte)(80)))));
            this.loginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(42)))), ((int)(((byte)(141)))));
            this.loginBtn.Location = new System.Drawing.Point(145, 165);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(267, 46);
            this.loginBtn.TabIndex = 3;
            this.loginBtn.Text = "LOGIN";
            this.loginBtn.UseVisualStyleBackColor = false;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.ClientSize = new System.Drawing.Size(581, 260);
            this.Controls.Add(this.loginBtn);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.loginLbl);
            this.Controls.Add(this.usernameTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.Label loginLbl;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginBtn;
    }
}

