namespace Warcaby
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel boardTable;
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
        private void InitializeComponent()
        {
            this.boardTable = new System.Windows.Forms.TableLayoutPanel();
            this.lblCurrentPlayer = new System.Windows.Forms.Label();
            this.lblMoveCount = new System.Windows.Forms.Label();
            this.btnBackToMenu = new System.Windows.Forms.Button();
            this.lblCounter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // boardTable
            // 
            this.boardTable.Location = new System.Drawing.Point(0, 0);
            this.boardTable.Name = "boardTable";
            this.boardTable.Size = new System.Drawing.Size(200, 100);
            this.boardTable.TabIndex = 0;
            // 
            // lblCurrentPlayer
            // 
            this.lblCurrentPlayer.AutoSize = true;
            this.lblCurrentPlayer.Location = new System.Drawing.Point(723, 14);
            this.lblCurrentPlayer.Name = "lblCurrentPlayer";
            this.lblCurrentPlayer.Size = new System.Drawing.Size(35, 13);
            this.lblCurrentPlayer.TabIndex = 2;
            this.lblCurrentPlayer.Text = "label1";
            // 
            // lblMoveCount
            // 
            this.lblMoveCount.AutoSize = true;
            this.lblMoveCount.Location = new System.Drawing.Point(723, 601);
            this.lblMoveCount.Name = "lblMoveCount";
            this.lblMoveCount.Size = new System.Drawing.Size(35, 13);
            this.lblMoveCount.TabIndex = 3;
            this.lblMoveCount.Text = "label2";
            // 
            // btnBackToMenu
            // 
            this.btnBackToMenu.Location = new System.Drawing.Point(828, 220);
            this.btnBackToMenu.Name = "btnBackToMenu";
            this.btnBackToMenu.Size = new System.Drawing.Size(75, 23);
            this.btnBackToMenu.TabIndex = 4;
            this.btnBackToMenu.Text = "Zakończ grę";
            this.btnBackToMenu.UseVisualStyleBackColor = true;
            this.btnBackToMenu.Click += new System.EventHandler(this.btnBackToMenu_Click);
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(1002, 496);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(35, 13);
            this.lblCounter.TabIndex = 5;
            this.lblCounter.Text = "label1";
            this.lblCounter.Click += new System.EventHandler(this.label1_Click);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 652);
            this.Controls.Add(this.lblCounter);
            this.Controls.Add(this.btnBackToMenu);
            this.Controls.Add(this.lblMoveCount);
            this.Controls.Add(this.lblCurrentPlayer);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label lblCurrentPlayer;
        private System.Windows.Forms.Label lblMoveCount;
        private System.Windows.Forms.Button btnBackToMenu;
        private System.Windows.Forms.Label lblCounter;
    }
}