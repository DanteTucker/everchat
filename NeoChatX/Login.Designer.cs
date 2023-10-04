namespace EverChat
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
            tUsername = new TextBox();
            tPassword = new TextBox();
            tOTP = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            loginButton = new Button();
            indicator = new Label();
            SuspendLayout();
            // 
            // tUsername
            // 
            tUsername.Location = new Point(12, 33);
            tUsername.Name = "tUsername";
            tUsername.Size = new Size(100, 23);
            tUsername.TabIndex = 0;
            // 
            // tPassword
            // 
            tPassword.Location = new Point(118, 33);
            tPassword.Name = "tPassword";
            tPassword.PasswordChar = '*';
            tPassword.Size = new Size(100, 23);
            tPassword.TabIndex = 1;
            // 
            // tOTP
            // 
            tOTP.Location = new Point(224, 33);
            tOTP.Name = "tOTP";
            tOTP.Size = new Size(100, 23);
            tOTP.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 3;
            label1.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(118, 15);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 4;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(224, 15);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 5;
            label3.Text = "OTP?";
            // 
            // loginButton
            // 
            loginButton.Location = new Point(249, 62);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(75, 23);
            loginButton.TabIndex = 6;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            // 
            // indicator
            // 
            indicator.AutoSize = true;
            indicator.ForeColor = Color.Red;
            indicator.Location = new Point(12, 66);
            indicator.Name = "indicator";
            indicator.Size = new Size(0, 15);
            indicator.TabIndex = 7;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(340, 101);
            Controls.Add(indicator);
            Controls.Add(loginButton);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tOTP);
            Controls.Add(tPassword);
            Controls.Add(tUsername);
            Name = "Login";
            Text = "Form2";
            Load += Login_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tUsername;
        private TextBox tPassword;
        private TextBox tOTP;
        private Label label1;
        private Label label2;
        private Label label3;
        public Button loginButton;
        public Label indicator;
    }
}