using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warcaby
{
    public partial class ScoreboardChoiceForm : Form
    {
        public ScoreboardChoiceForm()
        {
            InitializeComponent();
        }

        private void btnEasyScores_Click(object sender, EventArgs e)
        {
            ScoreboardForm scoreboard = new ScoreboardForm("Easy");
            scoreboard.Show();
            this.Hide();
        }

        // Analogiczne metody dla przycisków trudny i multiplayer

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainMenuForm mainMenu = new MainMenuForm();
            mainMenu.Show();
            this.Close();
        }
    }

}
