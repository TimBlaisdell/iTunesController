namespace iTunesController {
   sealed partial class DuplicateTrack {
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
         this.panelMain = new System.Windows.Forms.Panel();
         this.splitContainer = new System.Windows.Forms.SplitContainer();
         this.lblCurrentAlbum = new System.Windows.Forms.Label();
         this.lblCurrentArtist = new System.Windows.Forms.Label();
         this.lblCurrentName = new System.Windows.Forms.Label();
         this.lblCurrentLabel = new System.Windows.Forms.Label();
         this.lblOtherAlbum = new System.Windows.Forms.Label();
         this.lblOtherArtist = new System.Windows.Forms.Label();
         this.lblOtherName = new System.Windows.Forms.Label();
         this.lblOtherNameLabel = new System.Windows.Forms.Label();
         this.panelMain.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
         this.splitContainer.Panel1.SuspendLayout();
         this.splitContainer.Panel2.SuspendLayout();
         this.splitContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // panelMain
         // 
         this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.panelMain.Controls.Add(this.splitContainer);
         this.panelMain.Location = new System.Drawing.Point(13, 13);
         this.panelMain.Name = "panelMain";
         this.panelMain.Size = new System.Drawing.Size(481, 296);
         this.panelMain.TabIndex = 0;
         // 
         // splitContainer
         // 
         this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer.Location = new System.Drawing.Point(0, 0);
         this.splitContainer.Name = "splitContainer";
         // 
         // splitContainer.Panel1
         // 
         this.splitContainer.Panel1.Controls.Add(this.lblCurrentAlbum);
         this.splitContainer.Panel1.Controls.Add(this.lblCurrentArtist);
         this.splitContainer.Panel1.Controls.Add(this.lblCurrentName);
         this.splitContainer.Panel1.Controls.Add(this.lblCurrentLabel);
         this.splitContainer.Panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.border_MouseDown);
         this.splitContainer.Panel1.MouseEnter += new System.EventHandler(this.border_MouseEnter);
         this.splitContainer.Panel1.MouseLeave += new System.EventHandler(this.border_MouseLeave);
         this.splitContainer.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.border_MouseMove);
         this.splitContainer.Panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.border_MouseUp);
         // 
         // splitContainer.Panel2
         // 
         this.splitContainer.Panel2.Controls.Add(this.lblOtherAlbum);
         this.splitContainer.Panel2.Controls.Add(this.lblOtherArtist);
         this.splitContainer.Panel2.Controls.Add(this.lblOtherName);
         this.splitContainer.Panel2.Controls.Add(this.lblOtherNameLabel);
         this.splitContainer.Panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.border_MouseDown);
         this.splitContainer.Panel2.MouseEnter += new System.EventHandler(this.border_MouseEnter);
         this.splitContainer.Panel2.MouseLeave += new System.EventHandler(this.border_MouseLeave);
         this.splitContainer.Panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.border_MouseMove);
         this.splitContainer.Panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.border_MouseUp);
         this.splitContainer.Size = new System.Drawing.Size(481, 296);
         this.splitContainer.SplitterDistance = 236;
         this.splitContainer.TabIndex = 0;
         // 
         // lblCurrentAlbum
         // 
         this.lblCurrentAlbum.AutoSize = true;
         this.lblCurrentAlbum.Location = new System.Drawing.Point(-1, 46);
         this.lblCurrentAlbum.Name = "lblCurrentAlbum";
         this.lblCurrentAlbum.Size = new System.Drawing.Size(36, 13);
         this.lblCurrentAlbum.TabIndex = 0;
         this.lblCurrentAlbum.Text = "Album";
         // 
         // lblCurrentArtist
         // 
         this.lblCurrentArtist.AutoSize = true;
         this.lblCurrentArtist.Location = new System.Drawing.Point(-1, 31);
         this.lblCurrentArtist.Name = "lblCurrentArtist";
         this.lblCurrentArtist.Size = new System.Drawing.Size(30, 13);
         this.lblCurrentArtist.TabIndex = 0;
         this.lblCurrentArtist.Text = "Artist";
         // 
         // lblCurrentName
         // 
         this.lblCurrentName.AutoSize = true;
         this.lblCurrentName.Location = new System.Drawing.Point(-1, 15);
         this.lblCurrentName.Name = "lblCurrentName";
         this.lblCurrentName.Size = new System.Drawing.Size(35, 13);
         this.lblCurrentName.TabIndex = 0;
         this.lblCurrentName.Text = "Name";
         // 
         // lblCurrentLabel
         // 
         this.lblCurrentLabel.AutoSize = true;
         this.lblCurrentLabel.Location = new System.Drawing.Point(-1, 0);
         this.lblCurrentLabel.Name = "lblCurrentLabel";
         this.lblCurrentLabel.Size = new System.Drawing.Size(68, 13);
         this.lblCurrentLabel.TabIndex = 0;
         this.lblCurrentLabel.Text = "Current track";
         // 
         // lblOtherAlbum
         // 
         this.lblOtherAlbum.AutoSize = true;
         this.lblOtherAlbum.Location = new System.Drawing.Point(3, 46);
         this.lblOtherAlbum.Name = "lblOtherAlbum";
         this.lblOtherAlbum.Size = new System.Drawing.Size(36, 13);
         this.lblOtherAlbum.TabIndex = 0;
         this.lblOtherAlbum.Text = "Album";
         // 
         // lblOtherArtist
         // 
         this.lblOtherArtist.AutoSize = true;
         this.lblOtherArtist.Location = new System.Drawing.Point(3, 31);
         this.lblOtherArtist.Name = "lblOtherArtist";
         this.lblOtherArtist.Size = new System.Drawing.Size(30, 13);
         this.lblOtherArtist.TabIndex = 0;
         this.lblOtherArtist.Text = "Artist";
         // 
         // lblOtherName
         // 
         this.lblOtherName.AutoSize = true;
         this.lblOtherName.Location = new System.Drawing.Point(3, 15);
         this.lblOtherName.Name = "lblOtherName";
         this.lblOtherName.Size = new System.Drawing.Size(35, 13);
         this.lblOtherName.TabIndex = 0;
         this.lblOtherName.Text = "Name";
         // 
         // lblOtherNameLabel
         // 
         this.lblOtherNameLabel.AutoSize = true;
         this.lblOtherNameLabel.Location = new System.Drawing.Point(3, 0);
         this.lblOtherNameLabel.Name = "lblOtherNameLabel";
         this.lblOtherNameLabel.Size = new System.Drawing.Size(60, 13);
         this.lblOtherNameLabel.TabIndex = 0;
         this.lblOtherNameLabel.Text = "Other track";
         // 
         // DuplicateTrack
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.ClientSize = new System.Drawing.Size(506, 321);
         this.Controls.Add(this.panelMain);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "DuplicateTrack";
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.Text = "DuplicateTrack";
         this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.border_MouseDown);
         this.MouseEnter += new System.EventHandler(this.border_MouseEnter);
         this.MouseLeave += new System.EventHandler(this.border_MouseLeave);
         this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.border_MouseMove);
         this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.border_MouseUp);
         this.panelMain.ResumeLayout(false);
         this.splitContainer.Panel1.ResumeLayout(false);
         this.splitContainer.Panel1.PerformLayout();
         this.splitContainer.Panel2.ResumeLayout(false);
         this.splitContainer.Panel2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
         this.splitContainer.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panelMain;
      private System.Windows.Forms.SplitContainer splitContainer;
      private System.Windows.Forms.Label lblCurrentLabel;
      private System.Windows.Forms.Label lblOtherNameLabel;
      private System.Windows.Forms.Label lblCurrentName;
      private System.Windows.Forms.Label lblOtherName;
      private System.Windows.Forms.Label lblCurrentAlbum;
      private System.Windows.Forms.Label lblCurrentArtist;
      private System.Windows.Forms.Label lblOtherArtist;
      private System.Windows.Forms.Label lblOtherAlbum;
   }
}