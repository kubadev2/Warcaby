﻿namespace Warcaby
{
    partial class MainMenuForm
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnVsComputer = new System.Windows.Forms.Button();
            this.btnMultiplayer = new System.Windows.Forms.Button();
            this.btnScoreboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVsComputer
            // 
            this.btnVsComputer.Location = new System.Drawing.Point(338, 113);
            this.btnVsComputer.Name = "btnVsComputer";
            this.btnVsComputer.Size = new System.Drawing.Size(136, 23);
            this.btnVsComputer.TabIndex = 0;
            this.btnVsComputer.Text = "Graj z komputerem";
            this.btnVsComputer.UseVisualStyleBackColor = true;
            this.btnVsComputer.Click += new System.EventHandler(this.btnVsComputer_Click);
            // 
            // btnMultiplayer
            // 
            this.btnMultiplayer.Location = new System.Drawing.Point(338, 188);
            this.btnMultiplayer.Name = "btnMultiplayer";
            this.btnMultiplayer.Size = new System.Drawing.Size(136, 23);
            this.btnMultiplayer.TabIndex = 1;
            this.btnMultiplayer.Text = "Graj z innym graczem";
            this.btnMultiplayer.UseVisualStyleBackColor = true;
            this.btnMultiplayer.Click += new System.EventHandler(this.btnMultiplayer_Click);
            // 
            // btnScoreboard
            // 
            this.btnScoreboard.Location = new System.Drawing.Point(338, 259);
            this.btnScoreboard.Name = "btnScoreboard";
            this.btnScoreboard.Size = new System.Drawing.Size(136, 23);
            this.btnScoreboard.TabIndex = 2;
            this.btnScoreboard.Text = "Tabele wyników";
            this.btnScoreboard.UseVisualStyleBackColor = true;
            this.btnScoreboard.Click += new System.EventHandler(this.btnScoreboard_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnScoreboard);
            this.Controls.Add(this.btnMultiplayer);
            this.Controls.Add(this.btnVsComputer);
            this.Name = "MainMenuForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVsComputer;
        private System.Windows.Forms.Button btnMultiplayer;
        private System.Windows.Forms.Button btnScoreboard;
    }
}

