using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EverChat
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }

        private void LoginWithEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter key is pressed, trigger your custom event here
                Program.Login(sender, e, this.tUsername.Text, this.tPassword.Text, this.tOTP.Text);

                // Prevent the Enter key from creating a newline in the text box
                e.SuppressKeyPress = true;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.Text = "Login";
            this.loginButton.Click += (sender, EventArgs) => { Program.Login(sender, EventArgs, this.tUsername.Text, this.tPassword.Text, this.tOTP.Text); };
            this.tUsername.KeyDown += LoginWithEnter;
            this.tPassword.KeyDown += LoginWithEnter;
            this.tOTP.KeyDown += LoginWithEnter;

            this.FormClosed += (sender, EventArgs) =>
            {
                if (Program._cloud.CurrentUser == null)
                {
                    Application.Exit();
                }
            };
        }
    }
}
