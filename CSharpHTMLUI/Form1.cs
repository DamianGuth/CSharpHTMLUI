using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IEFixLib;
using CSharpHTMLUI.Events;

namespace CSharpHTMLUI
{
    public partial class Form1 : Form
    {
        /*Form Drawing stuff...*/
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        {
            e.Graphics.FillRectangle(Brushes.Black, Top);
            e.Graphics.FillRectangle(Brushes.Black, Left);
            e.Graphics.FillRectangle(Brushes.Black, Right);
            e.Graphics.FillRectangle(Brushes.Black, Bottom);

            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);

            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 3; // you can rename this variable if you like

        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x84) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }


        }
        /*End of Form drawing stuff*/


        public static WebBrowser webBrowser;
        public static Form1 instace;

        public Form1()
        {
            InitializeComponent();
            instace = this;
            this.panel1.Width = this.Width - 6;
            this.panel1.Height = this.Height - 35;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser = webBrowser1;
            Browser.Initialize();

            DemoEvent devent = new DemoEvent();
            HTMLEventHandler.RegisterEvent(devent);

            CalculateBtnEvent cevent = new CalculateBtnEvent();
            HTMLEventHandler.RegisterEvent(cevent);

            this.SetStyle(ControlStyles.ResizeRedraw, true); // This is to avoid visual artifacts

            //webBrowser1.DocumentText = "<script>external.log('TEST');</script>";
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            lblTitle.Text = webBrowser.Document.Title;
            Browser.InjectMethods();
        }



    }
}
