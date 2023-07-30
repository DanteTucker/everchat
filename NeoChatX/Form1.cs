using SkyFrost.Base;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Windows.Forms;

namespace EverChat
{
    public delegate void ContactsLoaded();
    public partial class Form1 : Form
    {
        public static OnlineStatus currentStatus = OnlineStatus.Invisible;
        public static DateTime _friendsupdateTime = DateTime.Now;
        public static ListBox _friendsList = new ListBox();
        public static SortableBindingList<ChatContact> _friendB = new SortableBindingList<ChatContact>();
        public static List<Contact> _contacts = new List<Contact>();
        public static TextBox _friendInfo;
        public static TextBox _messageInput = new TextBox();
        public static MessagePanel _messageHistory = new MessagePanel();
        public static ChatContact _friend = new ChatContact();
        public static Menu _menu = new Menu();
        public static bool blockupdate = false;
        private static bool _pendingUpdate = false;
        private Panel loadingSplash;

        public event ContactsLoaded OnContactsLoaded;

        public async void LoadFriends()
        {
            ShowLoadingSplash();
            OnContactsLoaded += DoneLoaded;
            WaitForLoaded();
        }

        public void DoneLoaded()
        {
            if (loadingSplash.InvokeRequired)
            {
                loadingSplash.Invoke(new Action(DoneLoaded));
            }
            else
            {
                Program._cloud.Contacts.GetContacts(_contacts);
                _contacts.ForEach((Contact friend) =>
                {
                    if (!friend.IsPartiallyMigrated)
                    {
                        ChatContact f = new ChatContact(friend);
                        _friendB.Add(f);
                    }
                });
                UpdateFriendsList();
                Program.friendsLoaded = true;
                HideLoadingSplash();
                Program._cloud.Contacts.ContactStatusChanged += UpdateContact;
                Program._cloud.Messages.OnMessageReceived += Form1.On_message;
            }
        }

        private void ShowLoadingSplash()
        {
            loadingSplash = new Panel
            {
                Size = new Size(200, 100),
                BackColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label loadingLabel = new Label
            {
                Text = "Loading Contacts...",
                AutoSize = true,
                Font = new Font(this.Font, FontStyle.Bold),
                Parent = loadingSplash
            };
            loadingLabel.Location = new Point((loadingSplash.Width - loadingLabel.Width) / 2, (loadingSplash.Height - loadingLabel.Height) / 2);

            loadingSplash.Location = new Point((this.ClientSize.Width - loadingSplash.Width) / 2, (this.ClientSize.Height - loadingSplash.Height) / 2);

            this.Controls.Add(loadingSplash);
            loadingSplash.BringToFront();
        }

        private void HideLoadingSplash()
        {
                this.Controls.Remove(loadingSplash);
                loadingSplash.Dispose();
                loadingSplash = null;
        }

        public async void WaitForLoaded()
        {
            Task.Run(async () =>
            {
                
                while (!Program._cloud.Contacts.ContactListLoaded) { }
                OnContactsLoaded();
                
            });
        }

        public static void UpdateFriendsList()
        {
            if (_friendsList.InvokeRequired)
            {
                _friendsList.Invoke(new Action(UpdateFriendsList));
            }
            else
            {
                if (!blockupdate)
                {
                    blockupdate = true;
                    _friendsList.BeginUpdate();
                    _friendB.SortBy("UserStatus", ListSortDirection.Descending);
                    Rectangle visibleArea = new Rectangle(_friendsList.ClientRectangle.Location, _friendsList.ClientSize);
                    //_friendsList.Invalidate(visibleArea);
                    _friendsList.EndUpdate();
                    //_friendsList.Update();
                    //_friendsList.Refresh();
                }
                blockupdate = false;
            }
        }

        public static string GetUsername(string id)
        {
            var f = _contacts.Where(x => x.ContactUserId == id).First();
            var i = _contacts.IndexOf(f);
            return _contacts[i].ContactUsername;
        }

        public static async void UpdateContact(ContactData a)
        {
            if (_friendsList.InvokeRequired)
            {
                _friendsList.Invoke(new Action<ContactData>(UpdateContact), a);
            }
            else
            {
                try
                { 
                    var f = _friendB.Where(x => x._contact.ContactUserId == a.UserId).First();
                    var i = _friendB.IndexOf(f);
                    int savedTopIndex = _friendsList.TopIndex;
                    if (a.Contact.ContactStatus == ContactStatus.Accepted && a.Contact.IsMigrated)
                    {
                        if (a.CurrentStatus.OnlineStatus != null && a.CurrentStatus.OnlineStatus != _friendB[i]._data.PreviousStatus.OnlineStatus)
                        {
                            _friendB[i]._data = a;
                            UpdateFriendsList();
                        }
                        Task.Run((Func<Task>)(async () => await Program._cloud.HubClient.BroadcastStatus(Program.status, BroadcastTarget.ToContact(a.Contact.ContactUserId)).ConfigureAwait(false)));
                    }
                    _friendsList.TopIndex = savedTopIndex;
                }
                catch { }
            }
        }

        private static void AppendTextToHistory(MessagePanel richTextBox, SkyFrost.Base.Message msg)
        {
                richTextBox.AddTextMessage(msg);
                richTextBox.ScrollToBottom();
        }

        private static void AppendTextToHistory(MessagePanel richTextBox, string text, string username)
        {
                richTextBox.AddTextMessage(text, username);
                richTextBox.ScrollToBottom();
        }

        public static void AddHistory(SkyFrost.Base.Message msg)
        {
            if (_messageHistory.InvokeRequired)
            {
                _messageHistory.Invoke(new Action<SkyFrost.Base.Message>(AddHistory), msg);
            }
            else
            {
                string username = "";
                if (msg.SenderId == Program._cloud.CurrentUser.Id)
                {
                    username = Program._cloud.CurrentUser.Username;
                }
                else
                {
                    //username = Program._cloud.Contacts.GetContact(msg.SenderId).ContactUsername;
                    username = GetUsername(msg.SenderId);
                }

                switch (msg.MessageType)
                {
                    case MessageType.Text:
                        AppendTextToHistory(_messageHistory, msg);
                        break;
                    case MessageType.Sound:
                        _messageHistory.AddAudioMessage(msg);
                        _messageHistory.ScrollToBottom();
                        break;
                    case MessageType.SessionInvite:
                        _messageHistory.AddInvite(msg);
                        _messageHistory.ScrollToBottom();
                        break;
                    default:
                        AppendTextToHistory(_messageHistory, Environment.NewLine + msg.SenderId + ": " + "[Unsupported Message Type]", username);
                        break;
                }
            }
        }

        private async void FriendsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected friend from the friends list
            _friend = _friendsList.SelectedItem as ChatContact;
            if (_friend != null)
            {
                _friendsList.Enabled = false;
                // Update the friend info and message history based on the selected friend
                var sessionname = "";
                
                if (_friend._data.CurrentSessionInfo != null)
                {
                    sessionname = _friend._data.CurrentSessionInfo.Name;
                }
                _friendInfo.Text = $"Name: {_friend._contact.ContactUsername}{Environment.NewLine}Status: {_friend._data.CurrentStatus.OnlineStatus.ToString()}{Environment.NewLine}Current Session: {sessionname}";
                var msgs = await Program._cloud.Messages.GetMessageHistory(_friend._contact.ContactUserId, 30);
                _messageHistory.messagesContainer.Controls.Clear();
                msgs.Entity.Reverse();
                msgs.Entity.ForEach((SkyFrost.Base.Message msg) => { AddHistory(msg); });
                Program._cloud.Messages.GetUserMessages(_friend._contact.ContactUserId).MarkAllRead();
                _friendsList.Enabled = true;
            }
            else
            {
                // Clear the friend info and message history if no friend is selected
                _friendInfo.Clear();
                //messageHistory.Clear();
            }
            UpdateFriendsList();
        }

        private async void SendMessage()
        {
            if (_friend != null && _messageInput.Text != "")
            {
                _messageHistory.AddTextMessage(_messageInput.Text, Program._cloud.CurrentUser.Username);
                _messageHistory.ScrollToBottom();
                Program._cloud.Messages.GetUserMessages(_friend._contact.ContactUserId).SendTextMessage(_messageInput.Text);
                _messageInput.Text = "";
            }
        }
        private void SendMessage(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendWithEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter key is pressed, trigger your custom event here
                SendMessage();

                // Prevent the Enter key from creating a newline in the text box
                e.SuppressKeyPress = true;
            }
        }

        public static void On_message(SkyFrost.Base.Message msg)       
        {
            if (msg.SenderId == _friend._contact.ContactUserId)
            {
                AddHistory(msg);
                Program._cloud.Messages.GetUserMessages(_friend._contact.ContactUserId).MarkAllRead();
            }
            UpdateFriendsList();
        }

        private void FriendsList_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }

            var listBox = (ListBox)sender;
            var friend = (ChatContact)listBox.Items[e.Index];
            

            e.DrawBackground();
            using (var brush = new SolidBrush(e.ForeColor))
            {
                SolidBrush backbrush;

                switch (friend._data.CurrentStatus.OnlineStatus)
                {
                    case OnlineStatus.Online:
                        backbrush = new SolidBrush(Color.Green);
                        e.Graphics.FillRectangle(backbrush, e.Bounds);
                        break;
                    case OnlineStatus.Away:
                        backbrush = new SolidBrush(Color.Yellow);
                        e.Graphics.FillRectangle(backbrush, e.Bounds);
                        break;
                    case OnlineStatus.Busy:
                        backbrush = new SolidBrush(Color.Red);
                        e.Graphics.FillRectangle(backbrush, e.Bounds);
                        break;
                }
                if (Program._cloud.Messages.GetUserMessages(friend._contact.ContactUserId).UnreadCount > 0)
                {
                    var unreadbrush = new SolidBrush(Color.Orange);
                    Font unreadFont = new Font(e.Font, FontStyle.Bold);
                    e.Graphics.DrawString("* " + friend._contact.ContactUsername, unreadFont, unreadbrush, e.Bounds);
                }
                else
                {
                    e.Graphics.DrawString(friend._contact.ContactUsername, e.Font, brush, e.Bounds);
                }
            }
            e.DrawFocusRectangle();
        }
        public Form1()
        {
            InitializeComponent();

            this.Text = "EverChat - Logged Out";
            Program._login.Visible = true;
            Program._login.TopMost = true;

            
        }

        private void MessageHistory_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.LinkText) { UseShellExecute = true });
        }
            private void Form1_Load(object sender, EventArgs e)
        {
            // Create the main container
            var mainContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };
            this.Controls.Add(mainContainer);

            // Create the left panel for the friends list
            _friendsList = new ListBox
            {
                Dock = DockStyle.Fill
            };
            mainContainer.Panel1.Controls.Add(_friendsList);
            mainContainer.SplitterDistance = (int)(mainContainer.Width * 0.25);
            _friendsList.SelectedIndexChanged += FriendsList_SelectedIndexChanged;
            _friendsList.DrawMode = DrawMode.OwnerDrawFixed;
            _friendsList.DrawItem += FriendsList_DrawItem;
            _friendsList.ItemHeight = 20;
            _friendsList.DataSource = _friendB;

            // Create the center and right panels using a nested SplitContainer
            var nestedContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical
            };
            mainContainer.Panel2.Controls.Add(nestedContainer);

            // Create the center panel for friend info
            _friendInfo = new TextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Multiline = true
            };
            nestedContainer.Panel1.Controls.Add(_friendInfo);

            // Create the right panel for message history and message input
            var messageContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal
            };
            nestedContainer.Panel2.Controls.Add(messageContainer);
            // Create the message history area
            _messageHistory = new MessagePanel
            {
                Dock = DockStyle.Fill
            };
            messageContainer.Panel1.Controls.Add(_messageHistory);

            // Create the bottom area for sending messages
            var bottomContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                ColumnCount = 2,
                RowCount = 1
            };

            messageContainer.Panel2.Controls.Add(bottomContainer);

            // Create the message input text box
            _messageInput = new TextBox
            {
                Dock = DockStyle.Bottom
            };
            bottomContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
            bottomContainer.Controls.Add(_messageInput, 0, 0);
            _messageInput.KeyDown += SendWithEnter;

            // Create the send message button
            var sendMessageButton = new Button
            {
                Text = "Send",
                Dock = DockStyle.Bottom
            };
            sendMessageButton.Click += SendMessage;
            bottomContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            bottomContainer.Controls.Add(sendMessageButton, 1, 0);
            messageContainer.SplitterDistance = messageContainer.Height - _messageInput.PreferredSize.Height - messageContainer.SplitterWidth;

            this.Controls.Add(_menu.menuStrip);

            this.FormClosing += Program.Form1_FormClosing;
        }
    }
}