using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private Size m_Initial_Size_Form;
        private Point m_Initial_Location_Load;
        private Size m_Initial_Size_Url;
        private Size m_Initial_Size_WebView;

        private void Form1_Load(object sender, EventArgs e)
        {
            m_Initial_Size_Form = this.Size;
            m_Initial_Location_Load = m_Load.Location;
            m_Initial_Size_Url = m_Url.Size;
            m_Initial_Size_WebView = m_WebView.Size;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            var adjust = this.Size - m_Initial_Size_Form;

            m_Load.Location = new Point(m_Initial_Location_Load.X + adjust.Width, m_Initial_Location_Load.Y);
            m_Url.Size = new Size(m_Initial_Size_Url.Width + adjust.Width, m_Initial_Size_Url.Height + adjust.Height);
            m_WebView.Size = new Size(m_Initial_Size_WebView.Width + adjust.Width, m_Initial_Size_WebView.Height + adjust.Height);
        }

        private void OnLoadClicked(object sender, EventArgs e)
        {
            var url = m_Url.Text;

            if (!string.IsNullOrEmpty(url))
            {
                m_WebView.LoadUrl(url);
            }
        }
    }
}
