namespace iTunesController {
    sealed partial class AlbumArtForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing ) {
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
        private void InitializeComponent ( ) {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlbumArtForm));
         this.timerUIUpdater = new System.Windows.Forms.Timer(this.components);
         this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
         this.menuitemExit = new System.Windows.Forms.ToolStripMenuItem();
         this.btnStars1 = new System.Windows.Forms.Button();
         this.btnStars2 = new System.Windows.Forms.Button();
         this.btnStars3 = new System.Windows.Forms.Button();
         this.btnStars4 = new System.Windows.Forms.Button();
         this.btnStars5 = new System.Windows.Forms.Button();
         this.lblAlbum = new iTunesController.OwnerDrawnLabel();
         this.lblArtist = new iTunesController.OwnerDrawnLabel();
         this.lblTrackName = new iTunesController.OwnerDrawnLabel();
         this.contextMenuStrip.SuspendLayout();
         this.SuspendLayout();
         // 
         // timerUIUpdater
         // 
         this.timerUIUpdater.Tick += new System.EventHandler(this.timerUIUpdater_Tick);
         // 
         // contextMenuStrip
         // 
         this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemExit});
         this.contextMenuStrip.Name = "contextMenuStrip";
         this.contextMenuStrip.Size = new System.Drawing.Size(93, 26);
         // 
         // menuitemExit
         // 
         this.menuitemExit.Name = "menuitemExit";
         this.menuitemExit.Size = new System.Drawing.Size(92, 22);
         this.menuitemExit.Text = "Exit";
         this.menuitemExit.Click += new System.EventHandler(this.menuitemExit_Click);
         // 
         // btnStars1
         // 
         this.btnStars1.FlatAppearance.BorderSize = 0;
         this.btnStars1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnStars1.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.btnStars1.ForeColor = System.Drawing.SystemColors.ControlDark;
         this.btnStars1.Location = new System.Drawing.Point(12, 86);
         this.btnStars1.Name = "btnStars1";
         this.btnStars1.Size = new System.Drawing.Size(18, 23);
         this.btnStars1.TabIndex = 3;
         this.btnStars1.Text = "";
         this.btnStars1.UseVisualStyleBackColor = true;
         this.btnStars1.Click += new System.EventHandler(this.btnStars_Click);
         this.btnStars1.MouseEnter += new System.EventHandler(this.btnStars_MouseEnter);
         this.btnStars1.MouseLeave += new System.EventHandler(this.btnStars_MouseLeave);
         this.btnStars1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnStars1_MouseMove);
         // 
         // btnStars2
         // 
         this.btnStars2.FlatAppearance.BorderSize = 0;
         this.btnStars2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnStars2.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.btnStars2.ForeColor = System.Drawing.SystemColors.ControlDark;
         this.btnStars2.Location = new System.Drawing.Point(27, 86);
         this.btnStars2.Name = "btnStars2";
         this.btnStars2.Size = new System.Drawing.Size(18, 23);
         this.btnStars2.TabIndex = 3;
         this.btnStars2.Text = "";
         this.btnStars2.UseVisualStyleBackColor = true;
         this.btnStars2.Click += new System.EventHandler(this.btnStars_Click);
         this.btnStars2.MouseEnter += new System.EventHandler(this.btnStars_MouseEnter);
         this.btnStars2.MouseLeave += new System.EventHandler(this.btnStars_MouseLeave);
         // 
         // btnStars3
         // 
         this.btnStars3.FlatAppearance.BorderSize = 0;
         this.btnStars3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnStars3.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.btnStars3.ForeColor = System.Drawing.SystemColors.ControlDark;
         this.btnStars3.Location = new System.Drawing.Point(42, 86);
         this.btnStars3.Name = "btnStars3";
         this.btnStars3.Size = new System.Drawing.Size(18, 23);
         this.btnStars3.TabIndex = 3;
         this.btnStars3.Text = "";
         this.btnStars3.UseVisualStyleBackColor = true;
         this.btnStars3.Click += new System.EventHandler(this.btnStars_Click);
         this.btnStars3.MouseEnter += new System.EventHandler(this.btnStars_MouseEnter);
         this.btnStars3.MouseLeave += new System.EventHandler(this.btnStars_MouseLeave);
         // 
         // btnStars4
         // 
         this.btnStars4.FlatAppearance.BorderSize = 0;
         this.btnStars4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnStars4.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.btnStars4.ForeColor = System.Drawing.SystemColors.ControlDark;
         this.btnStars4.Location = new System.Drawing.Point(57, 86);
         this.btnStars4.Name = "btnStars4";
         this.btnStars4.Size = new System.Drawing.Size(18, 23);
         this.btnStars4.TabIndex = 3;
         this.btnStars4.Text = "";
         this.btnStars4.UseVisualStyleBackColor = true;
         this.btnStars4.Click += new System.EventHandler(this.btnStars_Click);
         this.btnStars4.MouseEnter += new System.EventHandler(this.btnStars_MouseEnter);
         this.btnStars4.MouseLeave += new System.EventHandler(this.btnStars_MouseLeave);
         // 
         // btnStars5
         // 
         this.btnStars5.FlatAppearance.BorderSize = 0;
         this.btnStars5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.btnStars5.Font = new System.Drawing.Font("Wingdings", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.btnStars5.ForeColor = System.Drawing.SystemColors.ControlDark;
         this.btnStars5.Location = new System.Drawing.Point(72, 86);
         this.btnStars5.Name = "btnStars5";
         this.btnStars5.Size = new System.Drawing.Size(18, 23);
         this.btnStars5.TabIndex = 3;
         this.btnStars5.Text = "";
         this.btnStars5.UseVisualStyleBackColor = true;
         this.btnStars5.Click += new System.EventHandler(this.btnStars_Click);
         this.btnStars5.MouseEnter += new System.EventHandler(this.btnStars_MouseEnter);
         this.btnStars5.MouseLeave += new System.EventHandler(this.btnStars_MouseLeave);
         // 
         // lblAlbum
         // 
         this.lblAlbum.AutoSize = true;
         this.lblAlbum.BackColor = System.Drawing.Color.Transparent;
         this.lblAlbum.ContextMenuStrip = this.contextMenuStrip;
         this.lblAlbum.DropShadow = false;
         this.lblAlbum.DropShadowOffset = new System.Drawing.Point(0, 0);
         this.lblAlbum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblAlbum.ForeColor = System.Drawing.Color.Black;
         this.lblAlbum.Location = new System.Drawing.Point(12, 66);
         this.lblAlbum.Name = "lblAlbum";
         this.lblAlbum.OutlineColor = System.Drawing.Color.Gainsboro;
         this.lblAlbum.OutlineText = false;
         this.lblAlbum.OutlineWidth = 0F;
         this.lblAlbum.Size = new System.Drawing.Size(54, 20);
         this.lblAlbum.TabIndex = 2;
         this.lblAlbum.Text = "Album";
         // 
         // lblArtist
         // 
         this.lblArtist.AutoSize = true;
         this.lblArtist.BackColor = System.Drawing.Color.Transparent;
         this.lblArtist.ContextMenuStrip = this.contextMenuStrip;
         this.lblArtist.DropShadow = false;
         this.lblArtist.DropShadowOffset = new System.Drawing.Point(0, 0);
         this.lblArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblArtist.ForeColor = System.Drawing.Color.Black;
         this.lblArtist.Location = new System.Drawing.Point(12, 42);
         this.lblArtist.Name = "lblArtist";
         this.lblArtist.OutlineColor = System.Drawing.Color.Gainsboro;
         this.lblArtist.OutlineText = false;
         this.lblArtist.OutlineWidth = 0F;
         this.lblArtist.Size = new System.Drawing.Size(50, 24);
         this.lblArtist.TabIndex = 2;
         this.lblArtist.Text = "Artist";
         // 
         // lblTrackName
         // 
         this.lblTrackName.AutoSize = true;
         this.lblTrackName.BackColor = System.Drawing.Color.Transparent;
         this.lblTrackName.ContextMenuStrip = this.contextMenuStrip;
         this.lblTrackName.DropShadow = false;
         this.lblTrackName.DropShadowOffset = new System.Drawing.Point(0, 0);
         this.lblTrackName.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F);
         this.lblTrackName.ForeColor = System.Drawing.Color.Black;
         this.lblTrackName.Location = new System.Drawing.Point(12, 9);
         this.lblTrackName.Name = "lblTrackName";
         this.lblTrackName.OutlineColor = System.Drawing.Color.Gainsboro;
         this.lblTrackName.OutlineText = false;
         this.lblTrackName.OutlineWidth = 0F;
         this.lblTrackName.Size = new System.Drawing.Size(175, 33);
         this.lblTrackName.TabIndex = 2;
         this.lblTrackName.Text = "Track Name";
         // 
         // AlbumArtForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.ClientSize = new System.Drawing.Size(418, 112);
         this.ContextMenuStrip = this.contextMenuStrip;
         this.Controls.Add(this.btnStars4);
         this.Controls.Add(this.btnStars2);
         this.Controls.Add(this.btnStars5);
         this.Controls.Add(this.btnStars3);
         this.Controls.Add(this.btnStars1);
         this.Controls.Add(this.lblAlbum);
         this.Controls.Add(this.lblArtist);
         this.Controls.Add(this.lblTrackName);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "AlbumArtForm";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "Form1";
         this.TopMost = true;
         this.Load += new System.EventHandler(this.AlbumArtForm_Load);
         this.Shown += new System.EventHandler(this.AlbumArtForm_Shown);
         this.contextMenuStrip.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerUIUpdater;
        private OwnerDrawnLabel lblTrackName;
        private OwnerDrawnLabel lblArtist;
        private OwnerDrawnLabel lblAlbum;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuitemExit;
        private System.Windows.Forms.Button btnStars1;
        private System.Windows.Forms.Button btnStars2;
        private System.Windows.Forms.Button btnStars3;
        private System.Windows.Forms.Button btnStars4;
        private System.Windows.Forms.Button btnStars5;
    }
}

