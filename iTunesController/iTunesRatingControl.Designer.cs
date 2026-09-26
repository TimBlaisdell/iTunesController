namespace iTunesRatingsControl {
    sealed partial class iTunesRatingControl {
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iTunesRatingControl));
            timer = new System.Windows.Forms.Timer(components);
            contextMenuStrip = new ContextMenuStrip(components);
            menuFindMissingTracks = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuExitITunesRatingControl = new ToolStripMenuItem();
            menuExitITunes = new ToolStripMenuItem();
            contextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // timer
            // 
            timer.Tick += timer_Tick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { menuFindMissingTracks, toolStripSeparator1, menuExitITunesRatingControl, menuExitITunes });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(205, 76);
            // 
            // menuFindMissingTracks
            // 
            menuFindMissingTracks.Name = "menuFindMissingTracks";
            menuFindMissingTracks.Size = new Size(204, 22);
            menuFindMissingTracks.Text = "Find missing tracks";
            menuFindMissingTracks.Click += menuFindMissingTracks_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(201, 6);
            // 
            // menuExitITunesRatingControl
            // 
            menuExitITunesRatingControl.Name = "menuExitITunesRatingControl";
            menuExitITunesRatingControl.Size = new Size(204, 22);
            menuExitITunesRatingControl.Text = "Exit iTunesRatingControl";
            menuExitITunesRatingControl.Click += menuExitITunesRatingControl_Click;
            // 
            // menuExitITunes
            // 
            menuExitITunes.Name = "menuExitITunes";
            menuExitITunes.Size = new Size(204, 22);
            menuExitITunes.Text = "Exit iTunes";
            menuExitITunes.Click += menuExitITunes_Click;
            // 
            // iTunesRatingControl
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(160, 36);
            ContextMenuStrip = contextMenuStrip;
            Font = new Font("Wingdings", 24F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "iTunesRatingControl";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "iTunesRatingControl";
            Shown += iTunesRatingControl_Shown;
            MouseEnter += iTunesRatingControl_MouseEnter;
            MouseLeave += iTunesRatingControl_MouseLeave;
            MouseMove += iTunesRatingControl_MouseMove;
            MouseUp += iTunesRatingControl_MouseUp;
            contextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem menuExitITunesRatingControl;
        private ToolStripMenuItem menuExitITunes;
        private ToolStripMenuItem menuFindMissingTracks;
        private ToolStripSeparator toolStripSeparator1;
    }
}

