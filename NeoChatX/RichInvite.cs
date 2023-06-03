using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SkyFrost.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EverChat
{
    internal class RichInvite : Panel
    {
        private Label nameLabel;
        private Label timestampLabel;
        private Label messageTextLabel;

        public RichInvite(SkyFrost.Base.Message msg, int maxWidth)
        {

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Padding = new Padding(3);
            this.Margin = new Padding(3);
            this.BorderStyle = BorderStyle.FixedSingle;

            string username = "";
            if (msg.SenderId == Program._cloud.CurrentUser.Id)
            {
                this.BackColor = Color.SkyBlue;
            }
            else
            {
                this.BackColor = Color.LightBlue;
            }

            if (msg.SenderId == Program._cloud.CurrentUser.Id)
            {
                username = Program._cloud.CurrentUser.Username;
            }
            else
            {
                //username = Program._cloud.Contacts.GetContact(msg.SenderId).ContactUsername;
                username = Form1.GetUsername(msg.SenderId);
            }

            SessionInfo sessionInfo = msg.ExtractContent<SessionInfo>();
            sessionInfo = (Program._cloud.Sessions.TryGetInfo(sessionInfo.SessionId) ?? sessionInfo);

            var invitetext = "Invite to:" + Environment.NewLine +
                Program.RemoveTags(sessionInfo.Name) + Environment.NewLine +
                sessionInfo.JoinedUsers.ToString() + " out of " + sessionInfo.MaximumUsers.ToString() + Environment.NewLine +
                "Host: " + sessionInfo.HostUsername;

            nameLabel = new Label
            {
                Text = username,
                AutoSize = true,
                Location = new Point(3, 3),
                Parent = this,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            timestampLabel = new Label
            {
                Text = msg.SendTime.ToString("MM/dd/yyyy h:mm tt"),
                AutoSize = true,
                Location = new Point(nameLabel.Right + 3, 3),
                Parent = this
            };

            messageTextLabel = new System.Windows.Forms.Label
            {
                Text = invitetext,
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, this.Font.Size, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(3, timestampLabel.Bottom + 3),
                Parent = this,
                MaximumSize = new Size(maxWidth, 0)
            };
        }
    }
}
