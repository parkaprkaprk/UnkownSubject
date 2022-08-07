using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using CefSharp;
using CefSharp.WinForms;

namespace CefSharpTest
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser chrome = null;
        public Form1()
        {
            InitializeComponent();
            InitializeChromeBrowser();
        }

        private void InitializeChromeBrowser()
        {
            CefSettings cefSettings = new CefSettings();
            Cef.Initialize(cefSettings);
            chrome = new ChromiumWebBrowser("http://10.0.18.159/setup/setup.html");
            //chrome = new ChromiumWebBrowser("http://admin:`12qwert@10.0.18.159/setup/setup.html");
            chrome.Dock = DockStyle.Bottom;

            chrome.RequestHandler = new MyRequestHandler("admin", "`12qwert");
            chrome.FrameLoadEnd += BrowserFrameLoadEnd;

            this.Controls.Add(chrome);

        }

        //해당 페이지가 완전히 로드 된 뒤 발생시킬 이벤트 이다. 
        private void BrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            
        }

        public void ChangeFrameRate()
        {
            string strTargetScript = getScriptMoveTabAndClick("#Page400_id", "#mirror-h");
            chrome.ExecuteScriptAsync(strTargetScript);
            this.BeginInvoke(new MethodInvoker(delegate () { this.chrome.Refresh(); }));
        }   

        public void ChangeHorizontal()
        {
            var script3 = @"document.querySelector('#Page103_id').click()
            function waitUntilElementLoad(selector,  delay) {
            if(document.querySelector(selector) != null){
            document.querySelector('#mirror-h').click();
            } else setTimeout(()=>waitUntilElementLoad(selector, delay), delay);}
            waitUntilElementLoad('#mirror-h',2);";
            chrome.ExecuteScriptAsync(script3);
        }

        //탭 이동후 특정 element 표시 될때까지 기다리는 script
        public string getScriptMoveTabAndClick(string strtabName, string strTargetSelector)
        {
            string result = string.Format(@"document.querySelector('{0}').click();
            function waitUntilElementLoad(selector,  delay) {{
            if(document.querySelector(selector) != null){{
            document.querySelector('{1}').click();
            }} else setTimeout(()=>waitUntilElementLoad(selector, delay), delay);}}
            waitUntilElementLoad('{1}',2);", strtabName, strTargetSelector);

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeFrameRate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chrome != null)
                chrome.Reload();
        }
    }
}
