using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using SevenZip;

namespace CompressionHelper
{
    public partial class Form1 : Form
    {
        private enum WorkType
        {
            Compress, 
            Decompress
        }

        public Form1()
        {
            InitializeComponent();

            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            SevenZipBase.SetLibraryPath(Path.Combine(basePath, Environment.Is64BitProcess ? "x64\\7z.dll" : "x86\\7z.dll"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TargetBox.Text = Properties.Settings.Default.Target;
            ZipBox.Text = Properties.Settings.Default.Archive;
        }

        private void TargetSearch_Click(object sender, EventArgs e) => TargetBox.Text = ShowFolderDialog(TargetBox.Text);
 
        private void ZipSearch_Click(object sender, EventArgs e) => ZipBox.Text = ShowFolderDialog(ZipBox.Text);

        private string ShowFolderDialog(string root)
        {
            var dag = new Ookii.Dialogs.VistaFolderBrowserDialog()
            {
                Description = "Bitter Verzeichniss wählen.",
                RootFolder = Environment.SpecialFolder.MyDocuments,
                SelectedPath = root,
                ShowNewFolderButton = true,
                UseDescriptionForTitle = true
            };

            if (dag.ShowDialog(this) == DialogResult.OK)
                return dag.SelectedPath;

            return root;
        }

        private void ActivateAll()
        {
            RunView.Value = 0;
            //RunView.Style = ProgressBarStyle.Blocks;
            TargetBox.Enabled = true;
            ZipBox.Enabled = true;
            TargetSearch.Enabled = true;
            ZipSearch.Enabled = true;
            Comporess.Enabled = true;
            Decompress.Enabled = true;
        }

        private void DeactivateAll()
        {
            //RunView.Style = ProgressBarStyle.Marquee;
            TargetBox.Enabled = false;
            ZipBox.Enabled = false;
            TargetSearch.Enabled = false;
            ZipSearch.Enabled = false;
            Comporess.Enabled = false;
            Decompress.Enabled = false;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(new Action(DeactivateAll));

            var set = Properties.Settings.Default;
            set.Archive = ZipBox.Text;
            set.Target = TargetBox.Text;
            set.Save();

            switch ((WorkType)e.Argument)
            {
                case WorkType.Compress:
                    Compress();
                    break;
                case WorkType.Decompress:
                    DeCompress();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ActivateAll();
        }

        private void Comporess_Click(object sender, EventArgs e) => backgroundWorker.RunWorkerAsync(WorkType.Compress);

        private void Decompress_Click(object sender, EventArgs e) => backgroundWorker.RunWorkerAsync(WorkType.Decompress);

        private void Compress()
        {
            var ar = new SevenZipCompressor
            {
                ArchiveFormat = OutArchiveFormat.SevenZip,
                CompressionLevel = CompressionLevel.Ultra,
                CompressionMethod = CompressionMethod.Lzma2,
                CompressionMode = CompressionMode.Create,
                DirectoryStructure = true,
                IncludeEmptyDirectories = true,
                PreserveDirectoryRoot = false
            };
            ar.Compressing += (sender, args) => { Invoke(new Action(() => RunView.Value = args.PercentDone)); };
            ar.CompressionFinished += (sender, args) => Invoke(new Action(ActivateAll));

            if (!Directory.Exists(ZipBox.Text))
                Directory.CreateDirectory(ZipBox.Text);

            string file = Path.Combine(ZipBox.Text, "files.7z");
            if(File.Exists(file))
                File.Delete(file);

            ar.BeginCompressDirectory(TargetBox.Text, file, string.Empty, "*.*", true);
        }

        private void DeCompress()
        {
            var ar = new SevenZipExtractor(Path.Combine(ZipBox.Text, "files.7z"));

            ar.Extracting += (sender, args) => { Invoke(new Action(() => RunView.Value = args.PercentDone)); };
            ar.ExtractionFinished += (sender, args) => Invoke(new Action(() =>
            {
                ActivateAll();
                ar.Dispose();
            }));

            Directory.Delete(TargetBox.Text, true);
            if (!Directory.Exists(TargetBox.Text))
                Directory.CreateDirectory(TargetBox.Text);

            ar.BeginExtractArchive(TargetBox.Text);
        }
    }
}
