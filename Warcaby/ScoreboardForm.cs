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
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            LoadScores();
        }

        private void LoadScores()
        {
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

                // Wyczyść tabelę i przygotuj ją na nowe dane
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                // Dodaj kolumny
                dataGridView1.Columns.Add("Rank", "Ranking");
                dataGridView1.Columns.Add("PlayerName", "Nazwa Gracza");
                dataGridView1.Columns.Add("Moves", "Liczba Ruchów");

                // Wyświetl tylko 10 najlepszych wyników
                int count = Math.Min(10, allScores.Count);
                for (int i = 0; i < count; i++)
                {
                    dataGridView1.Rows.Add((i + 1), allScores[i].Item1, allScores[i].Item2);
                }

                // Dostosuj wygląd tabeli
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false; // Wyłącz możliwość dodawania nowych wierszy
                dataGridView1.AllowUserToDeleteRows = false; // Wyłącz możliwość usuwania wierszy
                dataGridView1.ReadOnly = true; // Ustaw tabelę jako tylko do odczytu

                // Dostosuj kolory i wygląd
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.DefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.DarkBlue;
                dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.LightGray;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.RowHeadersVisible = true;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
