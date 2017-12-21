#region Copyright Syncfusion Inc. 2001-2017.
// Copyright Syncfusion Inc. 2001-2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
namespace CompressionHelper
{
    partial class Form1
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
            this.TargetBox = new System.Windows.Forms.TextBox();
            this.ZipBox = new System.Windows.Forms.TextBox();
            this.Comporess = new System.Windows.Forms.Button();
            this.Decompress = new System.Windows.Forms.Button();
            this.TargetSearch = new System.Windows.Forms.Button();
            this.ZipSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RunView = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // TargetBox
            // 
            this.TargetBox.Location = new System.Drawing.Point(107, 16);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(368, 20);
            this.TargetBox.TabIndex = 0;
            // 
            // ZipBox
            // 
            this.ZipBox.Location = new System.Drawing.Point(107, 53);
            this.ZipBox.Name = "ZipBox";
            this.ZipBox.Size = new System.Drawing.Size(368, 20);
            this.ZipBox.TabIndex = 1;
            // 
            // Comporess
            // 
            this.Comporess.Location = new System.Drawing.Point(468, 116);
            this.Comporess.Name = "Comporess";
            this.Comporess.Size = new System.Drawing.Size(94, 23);
            this.Comporess.TabIndex = 2;
            this.Comporess.Text = "Komprimieren";
            this.Comporess.UseVisualStyleBackColor = true;
            this.Comporess.Click += new System.EventHandler(this.Comporess_Click);
            // 
            // Decompress
            // 
            this.Decompress.Location = new System.Drawing.Point(364, 116);
            this.Decompress.Name = "Decompress";
            this.Decompress.Size = new System.Drawing.Size(98, 23);
            this.Decompress.TabIndex = 3;
            this.Decompress.Text = "Dekomprimieren";
            this.Decompress.UseVisualStyleBackColor = true;
            this.Decompress.Click += new System.EventHandler(this.Decompress_Click);
            // 
            // TargetSearch
            // 
            this.TargetSearch.Location = new System.Drawing.Point(487, 14);
            this.TargetSearch.Name = "TargetSearch";
            this.TargetSearch.Size = new System.Drawing.Size(75, 23);
            this.TargetSearch.TabIndex = 4;
            this.TargetSearch.Text = "Suche";
            this.TargetSearch.UseVisualStyleBackColor = true;
            this.TargetSearch.Click += new System.EventHandler(this.TargetSearch_Click);
            // 
            // ZipSearch
            // 
            this.ZipSearch.Location = new System.Drawing.Point(487, 51);
            this.ZipSearch.Name = "ZipSearch";
            this.ZipSearch.Size = new System.Drawing.Size(75, 23);
            this.ZipSearch.TabIndex = 5;
            this.ZipSearch.Text = "Suche";
            this.ZipSearch.UseVisualStyleBackColor = true;
            this.ZipSearch.Click += new System.EventHandler(this.ZipSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Ziel:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Archiv:";
            // 
            // RunView
            // 
            this.RunView.Location = new System.Drawing.Point(15, 116);
            this.RunView.MarqueeAnimationSpeed = 30;
            this.RunView.Name = "RunView";
            this.RunView.Size = new System.Drawing.Size(343, 23);
            this.RunView.TabIndex = 8;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 151);
            this.Controls.Add(this.RunView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ZipSearch);
            this.Controls.Add(this.TargetSearch);
            this.Controls.Add(this.Decompress);
            this.Controls.Add(this.Comporess);
            this.Controls.Add(this.ZipBox);
            this.Controls.Add(this.TargetBox);
            this.Name = "Form1";
            this.Text = "Compression Helper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TargetBox;
        private System.Windows.Forms.TextBox ZipBox;
        private System.Windows.Forms.Button Comporess;
        private System.Windows.Forms.Button Decompress;
        private System.Windows.Forms.Button TargetSearch;
        private System.Windows.Forms.Button ZipSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar RunView;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

