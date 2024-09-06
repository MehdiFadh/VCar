using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace VCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butConnexion_Click(object sender, RoutedEventArgs e)
        {
            if(txtIdentifiant.Text == "mehdi" && txtPassword.Password == "toothless")
            {
                NpgsqlConnection connexion = new NpgsqlConnection();
                connexion.ConnectionString = "Server=ep-morning-wood-a27nq1on.eu-central-1.aws.neon.tech;" +
                                            "port=5432;" +
                                            "Database=eligiusdb;" +
                                            "uid=eligiusdb_owner;" +
                                            "password=tf4Ag8qwiJoT;";

                try
                {
                    connexion.Open();
                    MessageBox.Show("Connecté","Connexion",MessageBoxButton.OK,MessageBoxImage.Information);
                    gridConnexion.Visibility = Visibility.Collapsed;
                    window.Title = "Vcar - Accueil";
                }
                catch
                {
                    MessageBox.Show("Erreur de connexion à la base de données", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("L'identifiant ou le mot de passe est incorrect", "Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }
    }
}