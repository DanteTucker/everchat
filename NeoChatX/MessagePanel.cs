using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverChat
{
    public class MessagePanel : Panel
    {
        public FlowLayoutPanel messagesContainer;
        public MessagePanel()
        {
            
            messagesContainer = new FlowLayoutPanel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(messagesContainer);
        }

        public void AddTextMessage(SkyFrost.Base.Message msg)
        {
            int maxWidth = messagesContainer.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            RichTextMessage textMessage = new RichTextMessage(msg, maxWidth);
            messagesContainer.Controls.Add(textMessage);
        }
        public void AddTextMessage(String text, string username)
        {
            int maxWidth = messagesContainer.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            RichTextMessage textMessage = new RichTextMessage(text, maxWidth, username);
            messagesContainer.Controls.Add(textMessage);
        }

        public void AddAudioMessage(SkyFrost.Base.Message msg)
        {
            int maxWidth = messagesContainer.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            RichAudioMessage textMessage = new RichAudioMessage(msg, maxWidth);
            messagesContainer.Controls.Add(textMessage);
        }
        public void AddInvite(SkyFrost.Base.Message msg)
        {
            int maxWidth = messagesContainer.ClientSize.Width - SystemInformation.VerticalScrollBarWidth;
            RichInvite textMessage = new RichInvite(msg, maxWidth);
            messagesContainer.Controls.Add(textMessage);
        }

        public void ScrollToBottom()
        {
            messagesContainer.AutoScrollPosition = new Point(0, messagesContainer.DisplayRectangle.Height);
        }

    }
}
