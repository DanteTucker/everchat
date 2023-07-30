using SkyFrost.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverChat
{
    public class Menu
    {
        public MenuStrip menuStrip = new MenuStrip();
        public Menu() 
        {
            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Click += AboutMenuItem_Click;

            ToolStripMenuItem quitMenuItem = new ToolStripMenuItem("Quit");
            quitMenuItem.Click += QuitMenuItem_Click;

            ToolStripMenuItem menuDropDown = new ToolStripMenuItem("Menu");
            menuDropDown.DropDownItems.AddRange(new ToolStripItem[] { aboutMenuItem, quitMenuItem });

            ToolStripMenuItem onlineMenuItem = new ToolStripMenuItem("Online");
            onlineMenuItem.Tag = "Online";
            onlineMenuItem.Click += StatusMenuItem_Click;

            ToolStripMenuItem awayMenuItem = new ToolStripMenuItem("Away");
            awayMenuItem.Tag = "Away";
            awayMenuItem.Enabled = false;
            awayMenuItem.Click += StatusMenuItem_Click;

            ToolStripMenuItem busyMenuItem = new ToolStripMenuItem("Busy");
            busyMenuItem.Tag = "Busy";
            busyMenuItem.Enabled = false;
            busyMenuItem.Click += StatusMenuItem_Click;

            ToolStripMenuItem invisibleMenuItem = new ToolStripMenuItem("Invisible");
            invisibleMenuItem.Tag = "Invisible";
            invisibleMenuItem.Click += StatusMenuItem_Click;

            ToolStripMenuItem statusDropDown = new ToolStripMenuItem("Status");
            statusDropDown.DropDownItems.AddRange(new ToolStripItem[] { onlineMenuItem, awayMenuItem, busyMenuItem, invisibleMenuItem });

            ToolStripMenuItem refreshMenuItem = new ToolStripMenuItem("Refresh");
            refreshMenuItem.Click += RefreshMenuItem_Click;

            ToolStripMenuItem debugDropDown = new ToolStripMenuItem("Debug");
            debugDropDown.DropDownItems.AddRange(new ToolStripItem[] { refreshMenuItem });

            menuStrip.Items.AddRange(new ToolStripItem[] { menuDropDown, statusDropDown});
            menuStrip.Dock = DockStyle.Top;

        }

        private void RefreshMenuItem_Click(object sender, EventArgs e)
        {
            Form1._friendB.Clear();
            //Form1._friend = new Friend();
            Form1._messageHistory.Controls.Clear();
            Form1._friendInfo.Clear();
            Program._form1.LoadFriends();
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            // Implement the About menu item click event logic here
            MessageBox.Show("EverChat 1.3" + Environment.NewLine + "Created by Dante");
        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            // Implement the Quit menu item click event logic here
            Application.Exit();
        }

        private void StatusMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedMenuItem = sender as ToolStripMenuItem;
            if (clickedMenuItem == null) return;

            string status = clickedMenuItem.Tag.ToString();

            switch (status)
            {
                case "Online":
                    Form1.currentStatus = OnlineStatus.Online;
                    Program.UpdateOnlineStatus();
                    break;
                case "Away":
                    Form1.currentStatus = OnlineStatus.Away;
                    Program.UpdateOnlineStatus();
                    break;
                case "Busy":
                    Form1.currentStatus = OnlineStatus.Busy;
                    Program.UpdateOnlineStatus();
                    break;
                case "Invisible":
                    Form1.currentStatus = OnlineStatus.Invisible;
                    Program.UpdateOnlineStatus();
                    break;
            }
            Program.UpdateTitleBar();
        }
    }
}
