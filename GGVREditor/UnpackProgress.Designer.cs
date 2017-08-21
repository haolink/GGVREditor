namespace GGVREditor
{
    partial class UnpackProgress
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
            this.pbUnpackingProgress = new System.Windows.Forms.ProgressBar();
            this.lblUnpackFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pbUnpackingProgress
            // 
            this.pbUnpackingProgress.Location = new System.Drawing.Point(13, 13);
            this.pbUnpackingProgress.Name = "pbUnpackingProgress";
            this.pbUnpackingProgress.Size = new System.Drawing.Size(339, 23);
            this.pbUnpackingProgress.TabIndex = 0;
            // 
            // lblUnpackFile
            // 
            this.lblUnpackFile.Location = new System.Drawing.Point(13, 43);
            this.lblUnpackFile.Name = "lblUnpackFile";
            this.lblUnpackFile.Size = new System.Drawing.Size(339, 23);
            this.lblUnpackFile.TabIndex = 1;
            this.lblUnpackFile.Text = "Unpacking file 1 of 4000";
            this.lblUnpackFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UnpackProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 77);
            this.Controls.Add(this.lblUnpackFile);
            this.Controls.Add(this.pbUnpackingProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UnpackProgress";
            this.Text = "Unpacking progress";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UnpackProgress_FormClosing);
            this.Shown += new System.EventHandler(this.UnpackProgress_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbUnpackingProgress;
        private System.Windows.Forms.Label lblUnpackFile;
    }
}