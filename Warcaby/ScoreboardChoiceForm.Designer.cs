namespace Warcaby
{
    partial class ScoreboardChoiceForm
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
            this.btnEasyScores = new System.Windows.Forms.Button();
            this.btnHardScores = new System.Windows.Forms.Button();
            this.btnMultiplayerScores = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEasyScores
            // 
            this.btnEasyScores.Location = new System.Drawing.Point(353, 87);
            this.btnEasyScores.Name = "btnEasyScores";
            this.btnEasyScores.Size = new System.Drawing.Size(75, 23);
            this.btnEasyScores.TabIndex = 0;
            this.btnEasyScores.Text = "Łatwy";
            this.btnEasyScores.UseVisualStyleBackColor = true;
            this.btnEasyScores.Click += new System.EventHandler(this.btnEasyScores_Click);
            // 
            // btnHardScores
            // 
            this.btnHardScores.Location = new System.Drawing.Point(353, 126);
            this.btnHardScores.Name = "btnHardScores";
            this.btnHardScores.Size = new System.Drawing.Size(75, 23);
            this.btnHardScores.TabIndex = 1;
            this.btnHardScores.Text = "Trudny";
            this.btnHardScores.UseVisualStyleBackColor = true;
            // 
            // btnMultiplayerScores
            // 
            this.btnMultiplayerScores.Location = new System.Drawing.Point(353, 165);
            this.btnMultiplayerScores.Name = "btnMultiplayerScores";
            this.btnMultiplayerScores.Size = new System.Drawing.Size(75, 23);
            this.btnMultiplayerScores.TabIndex = 2;
            this.btnMultiplayerScores.Text = "Z graczem";
            this.btnMultiplayerScores.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(353, 236);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 3;
            this.btnBack.Text = "Powrót";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // ScoreboardChoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnMultiplayerScores);
            this.Controls.Add(this.btnHardScores);
            this.Controls.Add(this.btnEasyScores);
            this.Name = "ScoreboardChoiceForm";
            this.Text = "ScoreboardChoiceForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEasyScores;
        private System.Windows.Forms.Button btnHardScores;
        private System.Windows.Forms.Button btnMultiplayerScores;
        private System.Windows.Forms.Button btnBack;
    }
}