namespace Minesweeper
{
    partial class Gameplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label _flagNumberLabel;

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
            //this.components = new System.ComponentModel.Container();
            this._flagNumberLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // flagNumberLabel
            // 
            this._flagNumberLabel.AutoSize = true;
            this._flagNumberLabel.Font = new System.Drawing.Font("Arial", 16F);
            //this._flagNumberLabel.Location = new System.Drawing.Point(200, 10);
            this._flagNumberLabel.Name = "_flagNumberLabel";
            this._flagNumberLabel.Size = new System.Drawing.Size(189, 25);
            this._flagNumberLabel.TabIndex = 0;
            // 
            // Gameplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.Add(this._flagNumberLabel);
            this.Name = "Gameplay";
            this.Text = "Minesweeper";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
    }
}

