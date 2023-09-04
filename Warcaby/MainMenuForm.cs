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
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
        }

        private void btnVsComputer_Click(object sender, EventArgs e)
        {
            DifficultyForm difficultyForm = new DifficultyForm();
            difficultyForm.Show();
            this.Hide();
        }

        private void btnMultiplayer_Click(object sender, EventArgs e)
        {
            GameForm gameForm = new GameForm("Multiplayer");
            gameForm.Show();
            this.Hide();
        }

        private void btnScoreboard_Click(object sender, EventArgs e)
        {
            ScoreboardChoiceForm scoreboardChoiceForm = new ScoreboardChoiceForm();
            scoreboardChoiceForm.Show();
            this.Hide();
        }
    }

}
