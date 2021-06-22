namespace Jiggle
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.jiggleTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.options = new System.Windows.Forms.GroupBox();
            this.cbRightClick = new System.Windows.Forms.CheckBox();
            this.cbCloseMinimize = new System.Windows.Forms.CheckBox();
            this.cbMinimize = new System.Windows.Forms.CheckBox();
            this.options.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbEnabled
            // 
            resources.ApplyResources(this.cbEnabled, "cbEnabled");
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // jiggleTimer
            // 
            this.jiggleTimer.Interval = 1000;
            this.jiggleTimer.Tick += new System.EventHandler(this.jiggleTimer_Tick);
            // 
            // notifyIcon
            // 
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // icons
            // 
            this.icons.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject("icons.ImageStream")));
            this.icons.TransparentColor = System.Drawing.Color.Transparent;
            this.icons.Images.SetKeyName(0, "icon.ico");
            this.icons.Images.SetKeyName(1, "icon_active.ico");
            // 
            // options
            // 
            resources.ApplyResources(this.options, "options");
            this.options.Controls.Add(this.cbRightClick);
            this.options.Controls.Add(this.cbCloseMinimize);
            this.options.Controls.Add(this.cbMinimize);
            this.options.Name = "options";
            this.options.TabStop = false;
            // 
            // cbRightClick
            // 
            resources.ApplyResources(this.cbRightClick, "cbRightClick");
            this.cbRightClick.Name = "cbRightClick";
            this.cbRightClick.UseVisualStyleBackColor = true;
            // 
            // cbCloseMinimize
            // 
            resources.ApplyResources(this.cbCloseMinimize, "cbCloseMinimize");
            this.cbCloseMinimize.Name = "cbCloseMinimize";
            this.cbCloseMinimize.UseVisualStyleBackColor = true;
            // 
            // cbMinimize
            // 
            resources.ApplyResources(this.cbMinimize, "cbMinimize");
            this.cbMinimize.Name = "cbMinimize";
            this.cbMinimize.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.options);
            this.Controls.Add(this.cbEnabled);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.options.ResumeLayout(false);
            this.options.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox cbCloseMinimize;
        private System.Windows.Forms.CheckBox cbRightClick;

        private System.Windows.Forms.GroupBox options;
        private System.Windows.Forms.CheckBox cbMinimize;

        private System.Windows.Forms.ImageList icons;

        private System.Windows.Forms.NotifyIcon notifyIcon;

        private System.Windows.Forms.Timer jiggleTimer;

        private System.Windows.Forms.CheckBox cbEnabled;

        #endregion
    }
}