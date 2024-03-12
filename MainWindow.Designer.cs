namespace JIPipeInstaller
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            progressBar = new ProgressBar();
            statusLabel = new Label();
            cancelButton = new Button();
            titleLabel = new Label();
            SuspendLayout();
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 76);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(679, 23);
            progressBar.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.Location = new Point(12, 54);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(760, 19);
            statusLabel.TabIndex = 1;
            statusLabel.Text = "Please wait ...";
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(697, 76);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            titleLabel.Location = new Point(12, 9);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(183, 30);
            titleLabel.TabIndex = 3;
            titleLabel.Text = "Preparing JIPipe ...";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 111);
            Controls.Add(titleLabel);
            Controls.Add(cancelButton);
            Controls.Add(statusLabel);
            Controls.Add(progressBar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainWindow";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "JIPipe Installer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label statusLabel;
        private Button cancelButton;
        private Label titleLabel;
    }
}
