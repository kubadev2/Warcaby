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

        public ScoreboardForm()
        {
            InitializeComponent();
            LoadScores();
        }

        private void LoadScores()
        {
            // Wczytaj wyniki z pliku "wyniki.txt" i wyświetl tylko 10 najlepszych
            dataGridView1.Columns.Add("PlayerName", "Nazwa Gracza");
            dataGridView1.Columns.Add("Moves", "Liczba Ruchów");

            try
            {
                string[] lines = System.IO.File.ReadAllLines("wyniki.txt");

                // Utwórz listę wyników
                List<Tuple<string, int>> allScores = new List<Tuple<string, int>>();

                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string playerName = parts[0].Trim();
                        int moves;
                        if (int.TryParse(parts[1].Trim(), out moves))
                        {
                            allScores.Add(new Tuple<string, int>(playerName, moves));
                        }
                    }
                }

                // Sortuj wyniki od najlepszego do najgorszego
                allScores.Sort((a, b) => a.Item2.CompareTo(b.Item2));

                // Wyświetl tylko 10 najlepszych wyników
                int count = Math.Min(10, allScores.Count);
                for (int i = 0; i < count; i++)
                {
                    dataGridView1.Rows.Add(allScores[i].Item1, allScores[i].Item2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd podczas wczytywania wyników: " + ex.Message);
            }
        }


        private void btnBack_Click_1(object sender, EventArgs e)
        {
            MainMenuForm mainMenuForm = new MainMenuForm();
            mainMenuForm.Show();
            this.Close();
        }
    }

}
