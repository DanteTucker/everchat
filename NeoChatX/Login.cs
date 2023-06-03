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

        private void Login_Load(object sender, EventArgs e)
        {
            this.Text = "Login";
            this.loginButton.Click += (sender, EventArgs) => { Program.Login(sender, EventArgs, this.tUsername.Text, this.tPassword.Text, this.tOTP.Text); };
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
