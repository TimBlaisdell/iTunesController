namespace iTunesRatingsControl
{
    sealed partial class iTunesRatingControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iTunesRatingControl));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuExitITunesRatingControl = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExitITunes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFindMissingTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFindMissingTracks,
            this.toolStripSeparator1,
            this.menuExitITunesRatingControl,
            this.menuExitITunes});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(205, 98);
            // 
            // menuExitITunesRatingControl
            // 
            this.menuExitITunesRatingControl.Name = "menuExitITunesRatingControl";
            this.menuExitITunesRatingControl.Size = new System.Drawing.Size(204, 22);
            this.menuExitITunesRatingControl.Text = "Exit iTunesRatingControl";
            this.menuExitITunesRatingControl.Click += new System.EventHandler(this.menuExitITunesRatingControl_Click);
            // 
            // menuExitITunes
            // 
            this.menuExitITunes.Name = "menuExitITunes";
            this.menuExitITunes.Size = new System.Drawing.Size(204, 22);
            this.menuExitITunes.Text = "Exit iTunes";
            this.menuExitITunes.Click += new System.EventHandler(this.menuExitITunes_Click);
            // 
            // menuFindMissingTracks
            // 
            this.menuFindMissingTracks.Name = "menuFindMissingTracks";
            this.menuFindMissingTracks.Size = new System.Drawing.Size(204, 22);
            this.menuFindMissingTracks.Text = "Find missing tracks";
            this.menuFindMissingTracks.Click += new System.EventHandler(this.menuFindMissingTracks_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // iTunesRatingControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(160, 36);
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Font = new System.Drawing.Font("Wingdings", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "iTunesRatingControl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "iTunesRatingControl";
            this.Shown += new System.EventHandler(this.iTunesRatingControl_Shown);
            this.MouseEnter += new System.EventHandler(this.iTunesRatingControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.iTunesRatingControl_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.iTunesRatingControl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.iTunesRatingControl_MouseUp);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuExitITunesRatingControl;
        private System.Windows.Forms.ToolStripMenuItem menuExitITunes;
        private System.Windows.Forms.ToolStripMenuItem menuFindMissingTracks;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

