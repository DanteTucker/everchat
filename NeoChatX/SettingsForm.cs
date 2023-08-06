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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            this.invisibleBox.Checked = Program.settings.GetSetting("StartInvisible", true);
            this.notifyBox.Checked = Program.settings.GetSetting("Notify", true);
            apply.Click += Apply_Click;
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            Program.settings.SetSetting("StartInvisible", this.invisibleBox.Checked);
            Program.settings.SetSetting("Notify", this.notifyBox.Checked);
            Program.settings.SaveSettings();
            this.Close();
        }
    }
}
