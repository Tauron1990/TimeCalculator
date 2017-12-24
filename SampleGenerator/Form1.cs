using System;
using System.ComponentModel;
using System.Windows.Forms;
using SampleGenerator.Impl;

namespace SampleGenerator
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var gen = new Generator((RunData) e.Argument, progress1 => backgroundWorker.ReportProgress(0, progress1));

            gen.Generate();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var temp = (Progress) e.UserState;

            progress.Maximum = temp.Amount;
            progress.Value = temp.Generated;
            errorLabel.Text = temp.Errors.ToString();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            generate.Enabled = true;
            amountSamples.Enabled = true;
            dbName.Enabled = true;
        }

        private void generate_Click(object sender, EventArgs e)
        {
            generate.Enabled = false;
            amountSamples.Enabled = false;
            dbName.Enabled = false;

            backgroundWorker.RunWorkerAsync(new RunData { DatabaseName = dbName.Text, ToGenerate = (int)amountSamples.Value});
        }
    }
}
