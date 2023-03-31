namespace WindowsFormsApp2
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_Load = new System.Windows.Forms.Button();
            this.m_WebView = new Miniblink.WebView();
            this.m_Url = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_Load
            // 
            this.m_Load.Location = new System.Drawing.Point(713, 12);
            this.m_Load.Name = "m_Load";
            this.m_Load.Size = new System.Drawing.Size(75, 23);
            this.m_Load.TabIndex = 1;
            this.m_Load.Text = "加载";
            this.m_Load.UseVisualStyleBackColor = true;
            this.m_Load.Click += new System.EventHandler(this.OnLoadClicked);
            // 
            // m_WebView
            // 
            this.m_WebView.Location = new System.Drawing.Point(12, 41);
            this.m_WebView.Name = "m_WebView";
            this.m_WebView.Size = new System.Drawing.Size(776, 397);
            this.m_WebView.TabIndex = 2;
            // 
            // m_Url
            // 
            this.m_Url.Location = new System.Drawing.Point(12, 12);
            this.m_Url.Name = "m_Url";
            this.m_Url.Size = new System.Drawing.Size(695, 21);
            this.m_Url.TabIndex = 3;
            this.m_Url.Text = "https://www.bilibili.com";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.m_Url);
            this.Controls.Add(this.m_WebView);
            this.Controls.Add(this.m_Load);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "Form1";
            this.Text = "Miniblink UserControl";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button m_Load;
        private Miniblink.WebView m_WebView;
        private System.Windows.Forms.TextBox m_Url;
    }
}

