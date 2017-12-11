using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using EnjoiMail.Handlers;

namespace EnjoiMail
{
    public partial class Form : System.Windows.Forms.Form
    {
        private const string JsonFile = "site.json";
        public List<ChromiumWebBrowser> ChromeBrowsersList = new List<ChromiumWebBrowser>();
        private List<Model.Site> _sites = new List<Model.Site>();

        public Form()
        {
            InitializeComponent();

            // init form
            InitializeForm();

            // init chrome
            InitializeChromium();

            // init browsers
            if (_sites.Count > 0)
            {
                tab.CreateControl();
                var handle = tab.Handle;
                foreach (var site in _sites)
                {
                    AddNewTab(site);
                }
            }
        }

        private void InitializeForm()
        {
            this.KeyPreview = true;
            this.KeyDown += (sender, e) => {
                //if (e.Alt && !this.menuStrip1.Visible)
                    //this.menuStrip1.Visible = true;
            };
            this.menuStrip1.MenuDeactivate += (s, e) => this.menuStrip1.Visible = false;

            // tab
            this._sites = Model.Site.LoadFromJson(JsonFile);
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

        private void AddNewTab(Model.Site site)
        {
            Regex rgx = new Regex("[^\\w\\d\\-_\\.]");
            var cacheExt = rgx.Replace(site.Name, "");
            cacheExt += "-" + rgx.Replace(site.Url, "");
            var requestContextSettings = new RequestContextSettings { CachePath = "cache-" + cacheExt };
            var requestContext = new RequestContext(requestContextSettings);

            // Create a browser component
            var chromeBrowser = new ChromiumWebBrowser(site.Url)
            {
                RequestContext = requestContext
            };

            // Add it to the form and fill it to the form window.
            tab.TabPages.Insert(tab.TabCount - 1, site.Name);
            tab.TabPages[tab.TabPages.Count - 2].Controls.Add(chromeBrowser);

            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.RequestHandler = new RequestHandler { Url = site.Url };
            chromeBrowser.LifeSpanHandler = new LifeSpanHandler { Url = site.Url };
            chromeBrowser.DownloadHandler = new DownloadHandler();

            ChromeBrowsersList.Add(chromeBrowser);

            tab.Refresh();
            tab.SelectTab(tab.TabCount - 2);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        protected void ShowAddForm()
        {
            // and popup add new URL
            var add = new FormAdd();
            if (add.ShowDialog(this) == DialogResult.OK)
            {
                // Read the contents of add's TextBox.
                var site = add.GetSite();
                this._sites.Add(site);
                Model.Site.SaveToJson(this._sites, JsonFile);
                AddNewTab(site);
            }
            else
            {
                // canceled
            }
            add.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TcmSetMinTabWidth = 0x1300 + 49;
        private void tab_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tab.Handle, TcmSetMinTabWidth, IntPtr.Zero, (IntPtr)16);
        }

        private void tab_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == this.tab.TabCount - 1)
            {
                // cancel tab selecting itself
                e.Cancel = true;
            }
        }

        private int? GetClickedTabIndex()
        {
            Point p = this.tab.PointToClient(Cursor.Position);
            for (int i = 0; i < this.tab.TabCount; i++)
            {
                Rectangle r = this.tab.GetTabRect(i);
                if (r.Contains(p))
                {
                    return i;
                }
            }

            return null;
        }

        private void tab_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip.Show(this.tab, e.Location);
            }
            else
            {
                if (GetClickedTabIndex() == tab.TabCount - 1)
                {
                    ShowAddForm();
                }
            }
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var idx = this.tab.SelectedIndex;
            _sites.RemoveAt(idx);
            Model.Site.SaveToJson(this._sites, JsonFile);
            this.tab.TabPages.RemoveAt(idx);
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var idx = GetClickedTabIndex();
            if (idx != null && idx != tab.TabCount - 1)
            {
                this.tab.SelectTab(idx ?? 0);
                return;
            }

            e.Cancel = true;
        }

        private void tab_Selected(object sender, TabControlEventArgs e)
        {
        }
    }
}
