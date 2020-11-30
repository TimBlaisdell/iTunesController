namespace CompareImages {
    partial class Form1 {
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
            this.pbox1 = new System.Windows.Forms.PictureBox();
            this.pbox2 = new System.Windows.Forms.PictureBox();
            this.btnLoad1 = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnLoad2 = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.txtImageFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numMinPercent = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCompareFiles = new System.Windows.Forms.Button();
            this.lblPercentMatch = new System.Windows.Forms.Label();
            this.listMatches = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.progbar = new System.Windows.Forms.ProgressBar();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numScaleSize = new System.Windows.Forms.NumericUpDown();
            this.btnDelete1 = new System.Windows.Forms.Button();
            this.btnDelete2 = new System.Windows.Forms.Button();
            this.btnWriteHashes = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScaleSize)).BeginInit();
            this.SuspendLayout();
            // 
            // pbox1
            // 
            this.pbox1.Location = new System.Drawing.Point(13, 13);
            this.pbox1.Name = "pbox1";
            this.pbox1.Size = new System.Drawing.Size(400, 400);
            this.pbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox1.TabIndex = 0;
            this.pbox1.TabStop = false;
            // 
            // pbox2
            // 
            this.pbox2.Location = new System.Drawing.Point(419, 13);
            this.pbox2.Name = "pbox2";
            this.pbox2.Size = new System.Drawing.Size(400, 400);
            this.pbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbox2.TabIndex = 0;
            this.pbox2.TabStop = false;
            // 
            // btnLoad1
            // 
            this.btnLoad1.Location = new System.Drawing.Point(13, 420);
            this.btnLoad1.Name = "btnLoad1";
            this.btnLoad1.Size = new System.Drawing.Size(89, 23);
            this.btnLoad1.TabIndex = 1;
            this.btnLoad1.Text = "Load image 1";
            this.btnLoad1.UseVisualStyleBackColor = true;
            this.btnLoad1.Click += new System.EventHandler(this.btnLoad1_Click);
            // 
            // btnLoad2
            // 
            this.btnLoad2.Location = new System.Drawing.Point(419, 420);
            this.btnLoad2.Name = "btnLoad2";
            this.btnLoad2.Size = new System.Drawing.Size(89, 23);
            this.btnLoad2.TabIndex = 1;
            this.btnLoad2.Text = "Load image 2";
            this.btnLoad2.UseVisualStyleBackColor = true;
            this.btnLoad2.Click += new System.EventHandler(this.btnLoad2_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(730, 420);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(89, 23);
            this.btnCompare.TabIndex = 1;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // txtImageFolder
            // 
            this.txtImageFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtImageFolder.Location = new System.Drawing.Point(107, 462);
            this.txtImageFolder.Name = "txtImageFolder";
            this.txtImageFolder.Size = new System.Drawing.Size(273, 20);
            this.txtImageFolder.TabIndex = 3;
            this.txtImageFolder.TextChanged += new System.EventHandler(this.txtImageFolder_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(107, 447);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Image folder";
            // 
            // numMinPercent
            // 
            this.numMinPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numMinPercent.Location = new System.Drawing.Point(387, 462);
            this.numMinPercent.Name = "numMinPercent";
            this.numMinPercent.Size = new System.Drawing.Size(88, 20);
            this.numMinPercent.TabIndex = 5;
            this.numMinPercent.ValueChanged += new System.EventHandler(this.numMinPercent_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(386, 446);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Min pct for match";
            // 
            // btnCompareFiles
            // 
            this.btnCompareFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCompareFiles.Location = new System.Drawing.Point(481, 461);
            this.btnCompareFiles.Name = "btnCompareFiles";
            this.btnCompareFiles.Size = new System.Drawing.Size(89, 23);
            this.btnCompareFiles.TabIndex = 1;
            this.btnCompareFiles.Text = "Compare files";
            this.btnCompareFiles.UseVisualStyleBackColor = true;
            this.btnCompareFiles.Click += new System.EventHandler(this.btnCompareFiles_Click);
            // 
            // lblPercentMatch
            // 
            this.lblPercentMatch.Location = new System.Drawing.Point(730, 446);
            this.lblPercentMatch.Name = "lblPercentMatch";
            this.lblPercentMatch.Size = new System.Drawing.Size(88, 13);
            this.lblPercentMatch.TabIndex = 4;
            this.lblPercentMatch.Text = "0%";
            this.lblPercentMatch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPercentMatch.Visible = false;
            this.lblPercentMatch.VisibleChanged += new System.EventHandler(this.lblPercentMatch_VisibleChanged);
            // 
            // listMatches
            // 
            this.listMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMatches.FormattingEnabled = true;
            this.listMatches.IntegralHeight = false;
            this.listMatches.Location = new System.Drawing.Point(826, 29);
            this.listMatches.Name = "listMatches";
            this.listMatches.Size = new System.Drawing.Size(176, 384);
            this.listMatches.TabIndex = 6;
            this.listMatches.SelectedIndexChanged += new System.EventHandler(this.listMatches_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(825, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Matches";
            // 
            // progbar
            // 
            this.progbar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progbar.Location = new System.Drawing.Point(576, 461);
            this.progbar.Name = "progbar";
            this.progbar.Size = new System.Drawing.Size(426, 23);
            this.progbar.TabIndex = 8;
            this.progbar.Visible = false;
            // 
            // lblFileCount
            // 
            this.lblFileCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileCount.Location = new System.Drawing.Point(842, 445);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(160, 13);
            this.lblFileCount.TabIndex = 4;
            this.lblFileCount.Text = "0";
            this.lblFileCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblFileCount.Visible = false;
            this.lblFileCount.VisibleChanged += new System.EventHandler(this.lblPercentMatch_VisibleChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(202, 420);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Scale size";
            // 
            // numScaleSize
            // 
            this.numScaleSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numScaleSize.Location = new System.Drawing.Point(203, 436);
            this.numScaleSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numScaleSize.Name = "numScaleSize";
            this.numScaleSize.Size = new System.Drawing.Size(88, 20);
            this.numScaleSize.TabIndex = 5;
            this.numScaleSize.ValueChanged += new System.EventHandler(this.numScaleSize_ValueChanged);
            // 
            // btnDelete1
            // 
            this.btnDelete1.ForeColor = System.Drawing.Color.Red;
            this.btnDelete1.Location = new System.Drawing.Point(107, 420);
            this.btnDelete1.Name = "btnDelete1";
            this.btnDelete1.Size = new System.Drawing.Size(22, 23);
            this.btnDelete1.TabIndex = 1;
            this.btnDelete1.Text = "X";
            this.btnDelete1.UseVisualStyleBackColor = true;
            this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
            // 
            // btnDelete2
            // 
            this.btnDelete2.ForeColor = System.Drawing.Color.Red;
            this.btnDelete2.Location = new System.Drawing.Point(514, 420);
            this.btnDelete2.Name = "btnDelete2";
            this.btnDelete2.Size = new System.Drawing.Size(22, 23);
            this.btnDelete2.TabIndex = 1;
            this.btnDelete2.Text = "X";
            this.btnDelete2.UseVisualStyleBackColor = true;
            this.btnDelete2.Click += new System.EventHandler(this.btnDelete2_Click);
            // 
            // btnWriteHashes
            // 
            this.btnWriteHashes.Location = new System.Drawing.Point(13, 461);
            this.btnWriteHashes.Name = "btnWriteHashes";
            this.btnWriteHashes.Size = new System.Drawing.Size(89, 23);
            this.btnWriteHashes.TabIndex = 1;
            this.btnWriteHashes.Text = "Write hashes";
            this.btnWriteHashes.UseVisualStyleBackColor = true;
            this.btnWriteHashes.Click += new System.EventHandler(this.btnWriteHashes_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 495);
            this.Controls.Add(this.progbar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listMatches);
            this.Controls.Add(this.numScaleSize);
            this.Controls.Add(this.numMinPercent);
            this.Controls.Add(this.lblFileCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPercentMatch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtImageFolder);
            this.Controls.Add(this.btnCompareFiles);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnLoad2);
            this.Controls.Add(this.btnDelete2);
            this.Controls.Add(this.btnDelete1);
            this.Controls.Add(this.btnWriteHashes);
            this.Controls.Add(this.btnLoad1);
            this.Controls.Add(this.pbox2);
            this.Controls.Add(this.pbox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScaleSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbox1;
        private System.Windows.Forms.PictureBox pbox2;
        private System.Windows.Forms.Button btnLoad1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnLoad2;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox txtImageFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMinPercent;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCompareFiles;
        private System.Windows.Forms.Label lblPercentMatch;
        private System.Windows.Forms.ListBox listMatches;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progbar;
        private System.Windows.Forms.Label lblFileCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numScaleSize;
        private System.Windows.Forms.Button btnDelete1;
        private System.Windows.Forms.Button btnDelete2;
        private System.Windows.Forms.Button btnWriteHashes;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

