using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SkyFrost.Base;

namespace EverChat
{
    internal class RichTextMessage : Panel
    {
        private Label nameLabel;
        private Label timestampLabel;
        private Label messageTextLabel;

        public RichTextMessage(SkyFrost.Base.Message msg, int maxWidth)
        {

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Padding = new Padding(3);
            this.Margin = new Padding(3);
            this.BorderStyle = BorderStyle.FixedSingle;
            if (msg.SenderId == Program._cloud.CurrentUser.Id)
            {
                this.BackColor = Color.SkyBlue;
            }
            else
            {
                this.BackColor = Color.LightBlue;
            }

            string username = "";
            if (msg.SenderId == Program._cloud.CurrentUser.Id)
            { 
                username = Program._cloud.CurrentUser.Username;
            } else
            {
                //username = Program._cloud.Contacts.GetContact(msg.SenderId).ContactUsername;
                username = Form1.GetUsername(msg.SenderId);
            }

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


            messageTextLabel = new Label
            {
                Text = msg.Content,
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, this.Font.Size * 1.5f, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(3, timestampLabel.Bottom + 3),
                Parent = this,
                MaximumSize = new Size(maxWidth, 0)
            };
        }

        public RichTextMessage(string text, int maxWidth, string username)
        {

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Padding = new Padding(3);
            this.Margin = new Padding(3);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.BackColor = Color.SkyBlue;
            DateTime now = DateTime.Now;

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
                Text = now.ToString("MM/dd/yyyy h:mm tt"),
                AutoSize = true,
                Location = new Point(nameLabel.Right + 3, 3),
                Parent = this
            };


            messageTextLabel = new Label
            {
                Text = text,
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, this.Font.Size * 1.5f, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(3, timestampLabel.Bottom + 3),
                Parent = this,
                MaximumSize = new Size(maxWidth, 0)
            };
        }

    }
}
