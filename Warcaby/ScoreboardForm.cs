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
    public partial class ScoreboardForm : Form
    {
        private string difficulty;

        public ScoreboardForm(string difficulty)
        {
            InitializeComponent();
            this.difficulty = difficulty;
            LoadScores();
        }

        private void LoadScores()
        {
            // Tutaj kod do wczytywania wyników z pliku na podstawie trybu gry (trudność)
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            ScoreboardChoiceForm scoreboardChoice = new ScoreboardChoiceForm();
            scoreboardChoice.Show();
            this.Close();
        }
    }

}
