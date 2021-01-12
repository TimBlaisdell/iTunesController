using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CompareImages.Properties;
using iTunesControllerLib;

namespace CompareImages {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            if (!string.IsNullOrEmpty(Settings.Default.image1) && File.Exists(Settings.Default.image1)) LoadImage1(Settings.Default.image1);
            if (!string.IsNullOrEmpty(Settings.Default.image2) && File.Exists(Settings.Default.image2)) LoadImage2(Settings.Default.image2);
            numMinPercent.Value = Settings.Default.minpct == 0 ? 100 : Settings.Default.minpct;
            numScaleSize.Value = Settings.Default.scalesize == 0 ? 16 : Settings.Default.scalesize;
            if (!string.IsNullOrEmpty(Settings.Default.imagefolder)) txtImageFolder.Text = Settings.Default.imagefolder;
        }
        private void btnCompare_Click(object sender, EventArgs e) {
            _bmp1.Save("bmp1.png");
            _bmp2.Save("bmp2.png");
            var pct = HashCollection.CompareHashes(_bmp1.GetHash(_scaleSize, "bmp1"), _bmp2.GetHash(_scaleSize, "bmp2"));
            lblPercentMatch.Text = Math.Round(pct, 3) + "%";
            lblPercentMatch.Visible = true;
        }
        private void btnCompareFiles_Click(object sender, EventArgs e) {
            if (btnCompareFiles.Text == "Stop") {
                btnCompareFiles.Text = "Compare files";
                _stop = true;
                return;
            }
            btnCompareFiles.Text = "Stop";
            _stop = false;
            listMatches.Items.Clear();
            lblFileCount.Text = "0";
            lblFileCount.Visible = true;
            var files = Directory.GetFiles(txtImageFolder.Text, "*.*", SearchOption.AllDirectories).Where(isImage).ToArray();
            progbar.Maximum = files.Length;
            progbar.Value = 0;
            progbar.Visible = true;
            new Thread(() => ProcessingThread(files)).Start();
        }
        private void btnDelete1_Click(object sender, EventArgs e) {
            pbox1.Image = null;
            GC.Collect();
            File.Delete(_file1);
        }
        private void btnDelete2_Click(object sender, EventArgs e) {
            pbox2.Image = null;
            GC.Collect();
            File.Delete(_file2);
        }
        private void btnLoad1_Click(object sender, EventArgs e) {
            openFileDialog.Title = "Pick the first image";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK) LoadImage1(openFileDialog.FileName);
            lblPercentMatch.Visible = false;
        }
        private void btnLoad2_Click(object sender, EventArgs e) {
            openFileDialog.Title = "Pick the second image";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK) LoadImage2(openFileDialog.FileName);
            lblPercentMatch.Visible = false;
        }
        private void btnWriteHashes_Click(object sender, EventArgs e) {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                var hashes = new HashCollection();
                var files = Directory.GetFiles(txtImageFolder.Text);
                foreach (var file in files) {
                    int[] hash;
                    using (Bitmap bmp = new Bitmap(file)) {
                        hash = bmp.GetHash(_scaleSize);
                    }
                    hashes.Add(new HashEntry {Filename = file, Hash = hash});
                }
                hashes.Write(saveFileDialog.FileName);
            }
        }
        private void Form1_Load(object sender, EventArgs e) {
        }
        private void lblPercentMatch_VisibleChanged(object sender, EventArgs e) {
        }
        private void listMatches_SelectedIndexChanged(object sender, EventArgs e) {
            var lbe = listMatches.SelectedItem as ListBoxEntry;
            if (lbe == null) return;
            LoadImage1(Path.Combine(lbe.Hashes[0].Filename));
            LoadImage2(Path.Combine(lbe.Hashes[1].Filename));
        }
        private void numMinPercent_ValueChanged(object sender, EventArgs e) {
            Settings.Default.minpct = (int) numMinPercent.Value;
            Settings.Default.Save();
        }
        private void numScaleSize_ValueChanged(object sender, EventArgs e) {
            Settings.Default.scalesize = _scaleSize = (int) numScaleSize.Value;
            Settings.Default.Save();
        }
        private void txtImageFolder_TextChanged(object sender, EventArgs e) {
            Settings.Default.imagefolder = txtImageFolder.Text;
            Settings.Default.Save();
        }
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            _closed = true;
        }
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            _closed = true;
        }
        private bool isImage(string file) {
            string f = file.ToUpper();
            return f.EndsWith(".JPG") || f.EndsWith(".BMP") || f.EndsWith(".PNG") || f.EndsWith(".JPEG") || f.EndsWith(".TIF") || f.EndsWith(".TIFF");
        }
        private Bitmap LoadBitmap(string file) {
            Bitmap bmpreturn;
            using (var bmp = new Bitmap(file)) {
                int w = bmp.Width;
                int h = bmp.Height;
                bmpreturn = new Bitmap(w, h);
                using (var gfx = Graphics.FromImage(bmpreturn)) {
                    gfx.FillRectangle(Brushes.White, 0, 0, w, h);
                    gfx.DrawImage(bmp, 0, 0, w, h);
                }
            }
            return bmpreturn;
        }
        private void LoadImage1(string file) {
            try {
                _bmp1 = LoadBitmap(file);
                pbox1.Image = _bmp1;
                Settings.Default.image1 = file;
                Settings.Default.Save();
                _file1 = file;
            }
            catch (Exception ex) {
                MessageBox.Show(this, "Can't load " + file + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
            }
        }
        private void LoadImage2(string file) {
            try {
                _bmp2 = LoadBitmap(file);
                pbox2.Image = _bmp2;
                Settings.Default.image2 = file;
                Settings.Default.Save();
                _file2 = file;
            }
            catch (Exception ex) {
                MessageBox.Show(this, "Can't load " + file + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
            }
        }
        private void ProcessingThread(string[] files) {
            void UpdateProgBar(int c, int newmax = -1, int secondarycount = -1) {
                this.AsyncInvokeIfRequired(() => {
                                               if (newmax >= 0) progbar.Maximum = newmax;
                                               progbar.Value = c;
                                               lblFileCount.Text = c + (secondarycount >= 0 ? ": " + secondarycount : "");
                                           });
            }
            int counter = 0;
            void UpdateProgBarCounter() {
                ++counter;
                if (counter % 10 == 0) {
                    this.AsyncInvokeIfRequired(() => {
                                                   progbar.Value = counter;
                                                   lblFileCount.Text = counter.ToString();
                                               });
                }
            }
            int started = 0;
            int finished = 0;
            void LoadSomeFiles(object data) {
                ++started;
                var f = (string[]) ((object[])data)[0];
                var start = (int) ((object[])data)[1];
                var len = (int) ((object[])data)[2];
                var hashcoll = (HashCollection) ((object[])data)[3];
                for (int j = 0; j < len; ++j) {
                    if (_closed || _stop) return;
                    using (Bitmap bmp = LoadBitmap(f[j+start])) {
                        var hash = bmp.GetHash(_scaleSize);
                        lock (hashcoll) {
                            hashcoll.Add(new HashEntry {Filename = f[j + start], Hash = hash});
                        }
                    }
                    UpdateProgBarCounter();
                    Thread.Sleep(0);
                }
                ++finished;
            }
            var hashes = new HashCollection();
            for (int k = 0; k < files.Length; k += 50) {
                new Thread(LoadSomeFiles).Start(new object[]{files, k, Math.Min(50, files.Length-k), hashes});
            }
            while (started == 0) {
                if (_closed || _stop) return;
                Thread.Sleep(0);
            }
            while (started != finished) {
                if (_closed || _stop) return;
                Thread.Sleep(0);
            }
            //int i = 0;
            //foreach (var file in files) {
            //    if (_closed || _stop) return;
            //    using (Bitmap bmp = LoadBitmap(file)) {
            //        var hash = bmp.GetHash(_scaleSize);
            //        hashes.Add(new HashEntry {Filename = file, Hash = hash});
            //        UpdateProgBar(++i);
            //    }
            //    Thread.Sleep(0);
            //}
            UpdateProgBar(0, hashes.Count);
            var minpct = (double) numMinPercent.Value - 0.0001;
            for (int i = 0; i < hashes.Count; ++i) {
                var hash1 = hashes[i];
                for (int j = i + 1; j < hashes.Count; ++j) {
                    if (_closed || _stop) return;
                    var hash2 = hashes[j];
                    var pct = HashCollection.CompareHashes(hash1.Hash, hash2.Hash);
                    if (pct >= minpct) {
                        this.AsyncInvokeIfRequired(() => {
                                                       string s = hash1.FilenameNoPath + " = " + hash2.FilenameNoPath;
                                                       if (listMatches.Items.Cast<object>().ToArray().Any(o => o.ToString() == s)) return;
                                                       listMatches.Items.Add(new ListBoxEntry {Text = s, Hashes = new[] {hash1, hash2}});
                                                   });
                    }
                    if (j % 100 == 0) {
                        UpdateProgBar(i, -1, j);
                        Thread.Sleep(10);
                    }
                }
                UpdateProgBar(i + 1);
            }
            this.AsyncInvokeIfRequired(() => {
                                           progbar.Visible = false;
                                           lblFileCount.Visible = false;
                                       });
        }
        private Bitmap _bmp1;
        private Bitmap _bmp2;
        private bool _closed;
        private string _file1;
        private string _file2;
        private int _scaleSize = 16;
        private bool _stop;
        private class ListBoxEntry {
            public override string ToString() {
                return Text;
            }
            public HashEntry[] Hashes;
            public string Text;
        }
    }
}