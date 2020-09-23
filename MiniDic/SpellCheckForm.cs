using CefSharp;
using CefSharp.WinForms;
using System;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PlatformFarm.ShareWare.MiniDic
{
    public partial class SpellCheckForm : Form
    {                
        private ChromiumWebBrowser _webBrowser;

     
        public SpellCheckForm()
        {
            InitializeComponent();
        }

        private void SpellCheckForm_Load(object sender, EventArgs e)
        {
     
            string browseUrl = ConfigurationManager.AppSettings["spell_browseUrl"];

            _webBrowser = new ChromiumWebBrowser(browseUrl);
            JsDialogHandler jss = new JsDialogHandler();
            _webBrowser.JsDialogHandler = jss;
            _webBrowser.Dock = DockStyle.Fill;
            this.panel.Controls.Add(_webBrowser);


            string windowWidth = ConfigurationManager.AppSettings["spell_windowWidth"];
            string windowHeight = ConfigurationManager.AppSettings["spell_windowHeight"];


            this.Size = new Size(int.Parse(windowWidth), int.Parse(windowHeight));
            
            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            this.Opacity = ((double)trackBar1.Value/100);
        }

        private void SpellCheckForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DicForm._systemShutdown)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                this.Visible = false;
            }
        }
    }
    
}
