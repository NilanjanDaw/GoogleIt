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
using System.Net.NetworkInformation;
using System.Threading; 

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
            String ua = "Mozilla/5.0 (Windows Phone 8.1; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; Lumia 920) like Gecko";
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
            connectionChecker.RunWorkerAsync();
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

        private void checkConnection(object sender, DoWorkEventArgs e)
        {
            int status = 1;
            while (true)
            {
                try
                {
                    Ping ping = new Ping();
                    String server = "8.8.8.8";
                    int timeout = 1000;
                    byte[] buffer = new byte[32];
                    PingOptions options = new PingOptions();
                    PingReply reply = ping.Send(server, timeout, buffer, options);
                    if (reply.Status == IPStatus.Success)
                    {
                        if (status == 0)
                            connectionChecker.ReportProgress(1);
                        status = 1;
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        if (status == 1)
                            connectionChecker.ReportProgress(0);
                        status = 0;
                        Thread.Sleep(100);
                    }
                }
                catch (Exception)
                {
                    if (status == 1)
                        connectionChecker.ReportProgress(0);
                    status = 0;
                    Thread.Sleep(100);
                }
            }
        }

        private void connectionChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0)
                connectionStatus.BackColor = Color.Red;
            else
                connectionStatus.BackColor = Color.Green;
        }



    }
}
