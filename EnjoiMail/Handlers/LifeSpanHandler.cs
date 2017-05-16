using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using System.Text.RegularExpressions;
using System.Net;

namespace EnjoiMail.Handlers
{
    class LifeSpanHandler : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {
        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            if (Regex.IsMatch(targetUrl, "^https?://[\\w\\.\\-]+.google.com", RegexOptions.IgnoreCase)
            || Regex.IsMatch(targetUrl, "^https?://www.gmail.com", RegexOptions.IgnoreCase)
            || Regex.IsMatch(targetUrl, "^https?://mail.enjoitech.com", RegexOptions.IgnoreCase)
            || Regex.IsMatch(targetUrl, "^https?://\\w+.gstatic.com", RegexOptions.IgnoreCase)
            || Regex.IsMatch(targetUrl, "^https?://lh3.googleusercontent.com", RegexOptions.IgnoreCase))
            {
                // Google link
                var m = Regex.Match(targetUrl, "^https://www.google.com/url\\?hl=\\w+&q=([^&]+)&", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    var url = m.Groups[1];
                    System.Diagnostics.Process.Start(WebUtility.UrlDecode(url.Value));
                    return true;
                }

                // calendar
                if (Regex.IsMatch(targetUrl, "^https://www.google.com/calendar/render", RegexOptions.IgnoreCase))
                {
                    System.Diagnostics.Process.Start(targetUrl);
                    return true;
                }

                return false;
            }
            else
            {
                System.Diagnostics.Process.Start(targetUrl);
                return true;
            }
        }
    }
}
