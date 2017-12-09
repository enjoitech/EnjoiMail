using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using EnjoiMail.Handlers;

namespace EnjoiMail
{
    public partial class Form1 : Form
    {
        public List<ChromiumWebBrowser> chromeBrowsersList = new List<ChromiumWebBrowser>();
        private string[] mailsList = new string[] {"https://www.gmail.com", "http://mail.enjoitech.com"};
        //private string[] mailsList = new string[] { "https://mail.google.com/mail/u/0/", "http://mail.enjoitech.com"};

        public Form1()
        {
            InitializeComponent();

            // init form
            InitializeForm();

            // init chrome
            InitializeChromium();

            // init browsers
            for(var i = 0; i < mailsList.Length; i++)
            {
                InitializeBrowsers(i, mailsList[i]);
            }
        }

        private void InitializeForm()
        {
            this.KeyPreview = true;
            this.KeyDown += (sender, e) => {
                if (e.Alt && !this.menuStrip1.Visible)
                    this.menuStrip1.Visible = true;
            };
            this.menuStrip1.MenuDeactivate += (s, e) => this.menuStrip1.Visible = false;

            // tab
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = "";

            settings.SetOffScreenRenderingBestPerformanceArgs();
            //settings.CefCommandLineArgs.Add("disable-gpu", "1");
            //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
            //settings.CefCommandLineArgs.Add("enable-begin-frame-scheduling", "1");

            //settings.CefCommandLineArgs.Add("disable-gpu-vsync", "1"); //Disable Vsync

            // Initialize cef with the provided settings
            Cef.Initialize(settings);
        }

        public void InitializeBrowsers(int tabIndex, string url)
        {
            Regex rgx = new Regex("[^\\w\\d]");
            var cacheExt = rgx.Replace(url, "");
            var requestContextSettings = new RequestContextSettings { CachePath = "cache-" + cacheExt };
            var requestContext = new RequestContext(requestContextSettings);

            // Create a browser component
            var chromeBrowser = new ChromiumWebBrowser(url)
            {
                RequestContext = requestContext
            };

            // Add it to the form and fill it to the form window.
            //this.Controls.Add(chromeBrowser);
            tab.TabPages[tabIndex].Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.RequestHandler = new RequestHandler();
            chromeBrowser.LifeSpanHandler = new LifeSpanHandler();
            chromeBrowser.DownloadHandler = new DownloadHandler();

            chromeBrowsersList.Add(chromeBrowser);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private void tab_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tab.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }

        private void tab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == this.tab.TabCount - 1)
            {
                e.Cancel = true;
            }
        }
    }
}
