using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

        public Form1()
        {
            InitializeComponent();

            InitializeChromium();

            for(var i = 0; i < mailsList.Length; i++)
            {
                InitializeBrowsers(i, mailsList[i]);
            }
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            settings.CachePath = "cache";

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
            // Create a browser component
            var chromeBrowser = new ChromiumWebBrowser(url);
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
    }
}
