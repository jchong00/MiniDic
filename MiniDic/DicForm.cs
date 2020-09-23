using CefSharp;
using CefSharp.WinForms;
using System;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PlatformFarm.ShareWare.MiniDic
{
    public partial class DicForm : Form
    {
        
        #region fields
        public static int MOD_ALT = 0x1;
        public static int MOD_CONTROL = 0x2;
        public static int MOD_SHIFT = 0x4;
        public static int MOD_WIN = 0x8;
        public static int WM_HOTKEY = 0x312;
        public static int WM_QUERYENDSESSION = 0x0011;
        #endregion

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private static int keyId;

        private ChromiumWebBrowser _webBrowser;

        public static void RegisterHotKey(Form f, Keys key)
        {
            int modifiers = 0;

            if ((key & Keys.Alt) == Keys.Alt)
                modifiers = modifiers | MOD_ALT;

            if ((key & Keys.Control) == Keys.Control)
                modifiers = modifiers | MOD_CONTROL;

            if ((key & Keys.Shift) == Keys.Shift)
                modifiers = modifiers | MOD_SHIFT;

            Keys k = key & ~Keys.Control & ~Keys.Shift & ~Keys.Alt;
            // this should be a key unique ID, modify this if you want more than one hotkey
            keyId = f.GetHashCode(); 

            RegisterHotKey((IntPtr)f.Handle, keyId, (int)modifiers, (int)k);
        }


        public static void UnregisterHotKey(Form f)
        {
            try
            {
                UnregisterHotKey(f.Handle, keyId); // modify this if you want more than one hotkey
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public DicForm()
        {
            InitializeComponent();
        }

        private void DicForm_Load(object sender, EventArgs e)
        {
            Keys k = Keys.D | Keys.Control | Keys.Shift;
            RegisterHotKey(this, k);

            Keys k2 = Keys.C | Keys.Control | Keys.Shift;
            RegisterHotKey(this, k2);

            string browseUrl = ConfigurationManager.AppSettings["browseUrl"];

            _webBrowser = new ChromiumWebBrowser(browseUrl);
            JsDialogHandler jss = new JsDialogHandler();
            _webBrowser.JsDialogHandler = jss;
            _webBrowser.Dock = DockStyle.Fill;
            this.panel.Controls.Add(_webBrowser);


            string windowWidth = ConfigurationManager.AppSettings["windowWidth"];
            string windowHeight = ConfigurationManager.AppSettings["windowHeight"];


            this.Size = new Size(int.Parse(windowWidth), int.Parse(windowHeight));

            notifyIcon.ShowBalloonTip(5000);
            
        }

        public static bool _systemShutdown = false;
        
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_QUERYENDSESSION)
            {
                _systemShutdown = true;
                Program.SaveSettings();
            }
            else if (m.Msg == WM_HOTKEY)
            {
                int keyCode = HIWORD(m.LParam);

                if ((MOD_CONTROL | MOD_SHIFT) == LOWORD(m.LParam))
                {
                    if ((int)Keys.D == keyCode)
                    {
                        this.Visible = true;
                        this.TopMost = true;
                        this.TopMost = false;
                        this.Focus();
                    }

                    if ((int)Keys.C == keyCode)
                    {
                        _spellCheckForm.Visible = true;
                        _spellCheckForm.TopMost = true;
                        _spellCheckForm.TopMost = false;                        
                        _spellCheckForm.Focus();
                    }
                }
            }

            base.WndProc(ref m);

        }

        private static int LOWORD(IntPtr param)
        {
            uint uLParam = unchecked(IntPtr.Size == 8 ? (uint)param.ToInt64() : (uint)param.ToInt32());
            int nLowValue = unchecked((short)uLParam);
            return nLowValue;
        }

        private static int HIWORD(IntPtr param)
        {
            uint uWParam = unchecked(IntPtr.Size == 8 ? (uint)param.ToInt64() : (uint)param.ToInt32());
            int nHighValue = unchecked((short)(uWParam >> 16));
            return nHighValue;
        }

        protected override void OnClosed(EventArgs e)
        {
            Program.SaveSettings();
            base.OnClosed(e);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = ((double)trackBar1.Value/100);
        }

        private void contextMenuTray_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null && e.ClickedItem.Tag.ToString().Equals("EXIT"))
            {
                UnregisterHotKey(this);
                this.Close();
                this.Dispose();
                Properties.Settings.Default.Save();
                Application.Exit();
            }
            else if (e.ClickedItem.Tag != null && e.ClickedItem.Tag.ToString().Equals("DIC"))
            {
                this.Visible = true;
            }            
        }

        private void DicForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_systemShutdown)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                this.Visible = false;
            }

        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
            }
        }

        private SpellCheckForm _spellCheckForm = new SpellCheckForm();

        private void DicForm_Shown(object sender, EventArgs e)
        {
            _spellCheckForm.Show();
            _spellCheckForm.Visible = false;
        }
    }

    public class JsDialogHandler : IJsDialogHandler
    {
        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            callback.Continue(true);
            return true;
        }

        public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            return true;
        }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public bool OnBeforeUnloadDialog(IWebBrowser chromiumWebBrowser, IBrowser browser, string messageText, bool isReload, IJsDialogCallback callback)
        {
            return true;
        }
    }
}
