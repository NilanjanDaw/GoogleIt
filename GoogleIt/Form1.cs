using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; 

namespace GoogleIt
{
    public partial class Form1 : Form
    {
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(
            int dwOption, string pBuffer, int dwBufferLength, int dwReserved);

        const int URLMON_OPTION_USERAGENT = 0x10000001;
        const int URLMON_OPTION_USERAGENT_REFRESH = 0x10000002;
       
        public Form1()
        {
            ChangeUserAgent();
            InitializeComponent();
        }

        public void ChangeUserAgent()
        {
            List<string> userAgent = new List<string>();
            String ua = "Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 5.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 920) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537";
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT_REFRESH, null, 0, 0);
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, ua, ua.Length, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate("https://google.com/");
        }

        

        private void doucleClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2DoubleClicked(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        private void goHome(object sender, EventArgs e)
        {
            webBrowser1.Navigate("https://google.com/");
        }

        private void showProgressBar(object sender, WebBrowserProgressChangedEventArgs e)
        {
            progressBar1.Visible = true;
            try
            {
                progressBar1.Maximum = (int)e.MaximumProgress;
                progressBar1.Value = (int)e.CurrentProgress;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
        }


        private void loadComplete(object sender, WebBrowserNavigatingEventArgs e)
        {
            progressBar1.Visible = false;
        }


    }
}
