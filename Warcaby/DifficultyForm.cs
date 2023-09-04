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
    public partial class DifficultyForm : Form
{
    public DifficultyForm()
    {
        InitializeComponent();
    }

    private void btnEasy_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm("Easy");
        gameForm.Show();
        this.Hide();
    }

    private void btnHard_Click(object sender, EventArgs e)
    {
        GameForm gameForm = new GameForm("Hard");
        gameForm.Show();
        this.Hide();
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
        MainMenuForm mainMenu = new MainMenuForm();
        mainMenu.Show();
        this.Close();
    }
        private void button3_Click(object sender, EventArgs e)
        {
            // Tutaj możesz dodać logikę dla przycisku Powrót, np. zamknięcie tego formularza.
            this.Close();
        }
    }

}
