using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;
using LiveChartsCore.SkiaSharpView.WinForms;
using System.Globalization;

namespace RadioheadSalesDashboard
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=RadioheadSales;Trusted_Connection=True";
        // Objet Random pour générer des ventes aléatoires.
        private Random random = new Random();
        // Compteur des ventes récentes.
        private int recentSalesCount = 0;

        // Dictionnaire statique des albums disponibles.
        private static readonly Dictionary<string, string> albums = new Dictionary<string, string>
        {
            { "OK Computer", "OK Computer" },
            { "Kid A", "Kid A" },
            { "In Rainbows", "In Rainbows" }
        };

        // Initialise les graphiques et charge les données de revenu total.
        public Form1()
        {
            InitializeComponent();  // Initialise les composants de l'interface graphique.
            InitializeCharts();     // Initialise les graphiques de ventes et de stock.
            updateTimer.Start();    // Démarre le timer de mise à jour périodique des données.

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    double totalRevenue = GetTotalRevenue(connection);
                    totalRevenueLabel.Text = $"Revenu total : {totalRevenue:F2} €";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue lors de l'initialisation du revenu total : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogError(ex);
            }
        }

        // Méthode pour calculer le revenu total des ventes d'albums.
        private double GetTotalRevenue(SqlConnection connection)
        {
            string query = "SELECT ISNULL(SUM(s.Sales * a.Price), 0) AS TotalRevenue " +
                           "FROM AlbumSales s " +
                           "JOIN Albums a ON s.AlbumName = a.AlbumName";
            using (var command = new SqlCommand(query, connection))
            {
                object result = command.ExecuteScalar();
                return Convert.ToDouble(result);
            }
        }

        // Méthode pour initialiser les graphiques (ventes et stock).
        private void InitializeCharts()
        {
            salesChart.Series = new ISeries[] {
                new ColumnSeries<int> { Values = new int[] { 0, 0, 0 } }
            };

            salesChart.XAxes = new Axis[] {
                new Axis {
                    Labels = new[] { "OK Computer", "Kid A", "In Rainbows" },
                    Name = "Album",
                    LabelsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White),
                    NamePaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White)
                }
            };

            salesChart.YAxes = new Axis[] {
                new Axis {
                    Labeler = value => value.ToString("N"),
                    Name = "Sales",
                    LabelsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White),
                    NamePaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White)
                }
            };

            stockChart.Series = new ISeries[] {
                new ColumnSeries<int> { Values = new int[] { 0, 0, 0 } }
            };

            stockChart.XAxes = new Axis[] {
                new Axis {
                    Labels = new[] { "OK Computer", "Kid A", "In Rainbows" },
                    Name = "Album",
                    LabelsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White),
                    NamePaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White)
                }
            };

            stockChart.YAxes = new Axis[] {
                new Axis {
                    Labeler = value => value.ToString("N"),
                    Name = "Stock",
                    LabelsPaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White),
                    NamePaint = new LiveChartsCore.SkiaSharpView.Painting.SolidColorPaint(SKColors.White)
                }
            };
        }

        // Méthode qui se déclenche à chaque "tic" du timer. Effectue une vente aléatoire et met à jour les graphiques.
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            PerformRandomPurchase();
            UpdateChartData();
        }

        // Méthode qui met à jour les données des graphiques en interrogeant la base de données.
        private void UpdateChartData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    UpdateSalesData(connection);
                    UpdateStockData(connection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue lors de la mise à jour des graphiques : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogError(ex);
                }
            }
        }

        // Méthode qui met à jour les données des ventes dans le graphique des ventes.
        private void UpdateSalesData(SqlConnection connection)
        {
            var salesData = GetAlbumData(connection, "SELECT AlbumName, SUM(Sales) AS TotalSales FROM AlbumSales GROUP BY AlbumName");
            UpdateChart(salesChart, salesData);
        }

        // Méthode qui met à jour les données du stock dans le graphique du stock.
        private void UpdateStockData(SqlConnection connection)
        {
            var stockData = GetAlbumData(connection, "SELECT AlbumName, Stock FROM Albums");
            UpdateChart(stockChart, stockData); // Met à jour le graphique avec les nouvelles données.
        }

        // Méthode générique pour récupérer les données des albums à partir de la base de données.
        private Dictionary<string, int> GetAlbumData(SqlConnection connection, string query)
        {
            var data = new Dictionary<string, int> { { "OK Computer", 0 }, { "Kid A", 0 }, { "In Rainbows", 0 } };

            using (var command = new SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                // Parcourt les résultats de la requête et met à jour le dictionnaire des données.
                while (reader.Read())
                {
                    var albumName = reader.GetString(0);
                    var value = reader.GetInt32(1);
                    if (data.ContainsKey(albumName))
                        data[albumName] = value;
                }
            }

            return data;
        }

        // Méthode pour mettre à jour un graphique en fonction des données.
        private void UpdateChart(CartesianChart chart, Dictionary<string, int> data)
        {
            var series = chart.Series.FirstOrDefault() as ColumnSeries<int>;
            if (series != null)
            {
                series.Values = data.Values.ToArray();
                chart.Invalidate();
            }
        }

        // Méthode pour effectuer une vente aléatoire en choisissant un album.
        private void PerformRandomPurchase()
        {
            if (albums.Count > 0)
            {
                string randomAlbum = albums.Keys.ElementAt(random.Next(albums.Count));
                AddSale(randomAlbum);
            }
            else
            {
                MessageBox.Show("Aucun album n'est disponible pour un achat aléatoire.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Méthode pour ajouter une vente dans la base de données et mettre à jour le stock.
        private void AddSale(string albumName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int stock = GetAlbumStock(albumName, connection);
                    if (stock > 0)
                    {
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            InsertSale(albumName, connection, transaction);
                            UpdateStock(albumName, connection, transaction);
                            transaction.Commit();

                            recentSalesCount++;
                            recentSalesLabel.Text = $"Ventes recentes : {recentSalesCount}";

                            double albumPrice = GetAlbumPrice(albumName, connection);
                            string revenueText = totalRevenueLabel.Text.Split('€')[0].Trim();
                            string revenueValueText = revenueText.Split(':')[1].Trim();

                            double totalRevenue = Convert.ToDouble(revenueValueText, CultureInfo.InvariantCulture);

                            totalRevenue += albumPrice;
                            totalRevenueLabel.Text = $"Revenu total : {totalRevenue:F2} €";

                            MessageBox.Show($"La vente de l'album {albumName} a été effectuée avec succès.", "Vente traitée", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Stock insuffisant pour l'album {albumName}.", "Alerte stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur s'est produite lors de la vente : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogError(ex);
                }
            }
        }

        // Méthode pour récupérer le prix d'un album à partir de la base de données.
        private double GetAlbumPrice(string albumName, SqlConnection connection)
        {
            string query = "SELECT Price FROM Albums WHERE AlbumName = @AlbumName";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AlbumName", albumName);
                object result = command.ExecuteScalar();
                return Convert.ToDouble(result);
            }
        }

        // Méthode pour récupérer le stock d'un album à partir de la base de données.
        private int GetAlbumStock(string albumName, SqlConnection connection)
        {
            string query = "SELECT Stock FROM Albums WHERE AlbumName = @AlbumName";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AlbumName", albumName);
                return (int)command.ExecuteScalar();
            }
        }

        // Méthode pour insérer une vente dans la base de données.
        private void InsertSale(string albumName, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "INSERT INTO AlbumSales (AlbumName, Sales) VALUES (@AlbumName, 1)";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@AlbumName", albumName);
                command.ExecuteNonQuery();
            }
        }

        // Méthode pour mettre à jour le stock après une vente.
        private void UpdateStock(string albumName, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "UPDATE Albums SET Stock = Stock - 1 WHERE AlbumName = @AlbumName";
            using (var command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@AlbumName", albumName);
                command.ExecuteNonQuery();
            }
        }

        // Bouton pour recharger le stock d'albums.
        private void ReloadStockButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    ReloadStock(connection);
                    MessageBox.Show("Le stock a été rechargé avec succès.", "Stock rechargé", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Une erreur est survenue lors du rechargement du stock : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogError(ex);
                }
            }
        }

        // Méthode pour recharger le stock à 50 pour chaque album.
        private void ReloadStock(SqlConnection connection)
        {
            string query = "UPDATE Albums SET Stock = 50";
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // Méthode pour enregistrer les erreurs dans un fichier log.
        private void LogError(Exception ex)
        {
            string logFilePath = "error_log.txt";
            string logMessage = $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n";
            File.AppendAllText(logFilePath, logMessage);
        }
    }
}
