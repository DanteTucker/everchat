using EverChat;
using Microsoft.VisualBasic.ApplicationServices;
using System.Text.RegularExpressions;
using SkyFrost.Base;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EverChat
{
    internal static class Program
    {
        public static SkyFrostInterface _cloud;
        public static String machineId;
        public static Login _login = new Login();
        public static Form1 _form1;
        public static bool friendsLoaded = false;
        public static UserSession _userSession = new UserSession();
        public static string statussession;
        public static UserStatus status = new UserStatus();
        public static SettingsManager settings = new SettingsManager();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            SkyFrostConfig _skyFrostConfig = SkyFrostConfig.DEFAULT_PRODUCTION;
            machineId = CryptoHelper.GenerateCryptoToken();
            string uid = CryptoHelper.HashIDToToken(machineId);
            _cloud = new SkyFrostInterface(uid, _skyFrostConfig);

            _form1 = new Form1();
            Application.Run(_form1);

            
        }

        static public void Login(object sender, EventArgs e, String user, String pass, String OTP)
        {
            _login.loginButton.Enabled = false;
            LoginAsync(user, pass, OTP);
        }

        static public async Task UpdateOnlineStatus()
        {
            status = new UserStatus();
            
            status.UserId = _cloud.CurrentUser.Id;
            if(statussession == null)
            {
                statussession = Guid.NewGuid().ToString();
            }
            status.AppVersion = "EverChat";

            if (Form1.currentStatus != OnlineStatus.Invisible)
            {
                status.UserSessionId = _cloud.Status.UserSessionId;
                status.OnlineStatus = Form1.currentStatus;
                status.SessionType = UserSessionType.ChatClient;
                status.Sessions = new List<UserSessionMetadata>();
                status.IsPresent = true;
                status.LastStatusChange = DateTime.Now;
                Task.Run((Func<Task>)(async () => await _cloud.HubClient.BroadcastStatus(status, BroadcastTarget.ALL_CONTACTS).ConfigureAwait(false)));
            }
            else
            {
                status.UserSessionId = _cloud.Status.UserSessionId;
                status.OnlineStatus = new OnlineStatus?(OnlineStatus.Offline);
                status.SessionType = UserSessionType.ChatClient;
                status.IsPresent = false;
                //status.Sessions.Clear();
                status.CurrentSessionIndex = -1;
                status.LastStatusChange = DateTime.Now;
                Task.Run((Func<Task>)(async () => await _cloud.HubClient.BroadcastStatus(status, BroadcastTarget.ALL_CONTACTS).ConfigureAwait(false)));
            }
            
        }

        static public void UpdateTitleBar()
        {
            if (_cloud.Status.OnlineStatus == null)
            {
                _form1.Text = "Logged in as: " + Program._cloud.CurrentUser.Username + " - Showing as: Offline";
            }
            else
            {
                _form1.Text = "Logged in as: " + Program._cloud.CurrentUser.Username + " - Showing as: " + Form1.currentStatus.ToString();
            }

        }

        static public async Task LoginAsync(String user, String pass, String OTP)
        {
            var _otp = OTP;
            if (_otp == "")
            {
                _otp = null;
            }
            var result = await _cloud.Session.Login(user, (LoginAuthentication)new PasswordLogin(pass), machineId, false, _otp);
            if (result.IsOK)
            {
                _userSession = result.Entity;
                UpdateTitleBar();
                _login.Visible = false;
                UpdateOnlineStatus();
                _form1.LoadFriends();
            } else
            {
                _login.loginButton.Enabled = true;
                _login.indicator.Text = "Invalid Credentials";
            }
            
        }

        public static async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.currentStatus = OnlineStatus.Offline;
            await UpdateOnlineStatus();
            _cloud.Session.Logout(true);
        }

        public static string RemoveTags(string input)
        {
            string pattern = @"<[^>]*>";
            return Regex.Replace(input, pattern, "");
        }

    }
}