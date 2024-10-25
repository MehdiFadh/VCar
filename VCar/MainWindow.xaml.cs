using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        private const double FacteurDefilement = 1;

        public ObservableCollection<Voiture> Voitures { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            txtIdentifiant.Focus();
            LoadData();
            DataContext = this;
        }

        private void LoadData()
        {
            var service = new ApplicationData();
            Voitures = service.GetVoitures();
        }

        private void EquipmentList_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            // Obtenir le ScrollViewer associé à la ListBox
            ScrollViewer scrollViewer = GetScrollViewer(listBox);
            if (scrollViewer != null)
            {
                // Calculer la distance de défilement en utilisant le facteur
                double offsetChange = e.Delta * FacteurDefilement / Mouse.MouseWheelDeltaForOneLine;

                // Mettre à jour l'offset vertical
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - offsetChange);

                // Empêcher l'événement de se propager
                e.Handled = true;
            }
        }

        private ScrollViewer GetScrollViewer(DependencyObject dependencyObject)
        {
            if (dependencyObject is ScrollViewer) return (ScrollViewer)dependencyObject;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                var scrollViewer = GetScrollViewer(child);
                if (scrollViewer != null) return scrollViewer;
            }

            return null;
        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (txtIdentifiant.Text == "mehdi" && txtPassword.Password == "toothless")
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
                    gridConnexion.Visibility = Visibility.Collapsed;
                    window.Title = "Vcar - Accueil";
                    gridAccueil.Visibility = Visibility.Visible;
                    window.Width = 1200;
                    window.Height = 850;
                    Window_AuMilieu();
                }
                catch
                {
                    MessageBox.Show("Erreur de connexion à la base de données", "Echec base de données", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("L'identifiant ou le mot de passe est incorrect", "Echec de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private void butRecherche_Click(object sender, RoutedEventArgs e)
        {
            gridDemande.Visibility = Visibility.Collapsed;
            gridContact.Visibility = Visibility.Collapsed;
            gridRecherche.Visibility = Visibility.Visible;
            gridInfoVoiture.Visibility = Visibility.Collapsed;
        }

        private void butDemande_Click(object sender, RoutedEventArgs e)
        {
            gridRecherche.Visibility = Visibility.Collapsed;
            gridContact.Visibility = Visibility.Collapsed;
            gridDemande.Visibility = Visibility.Visible;
            gridInfoVoiture.Visibility = Visibility.Collapsed;
        }

        private void Window_AuMilieu()
        {
            if (window != null)
            {
                window.Left = (SystemParameters.WorkArea.Width - window.ActualWidth) / 2;
                window.Top = (SystemParameters.WorkArea.Height - window.ActualHeight) / 2;
            }
        }

        private void butContact_Click(object sender, RoutedEventArgs e)
        {
            gridRecherche.Visibility = Visibility.Collapsed;
            gridDemande.Visibility = Visibility.Collapsed;
            gridContact.Visibility = Visibility.Visible;
            gridInfoVoiture.Visibility = Visibility.Collapsed;
        }

        private void txtRecherche_GotFocus(object sender, RoutedEventArgs e)
        {
            txtRecherche.Text = "";
        }

        private void txtRecherche_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtRecherche.Text == "")
            {
                txtRecherche.Text = "Rechercher...";
            }
        }

        public void RefreshMateriels()
        {
            var service = new ApplicationData();

            Voitures = service.GetVoitures();
            VoitureListBox.ItemsSource = null; // Déconnecter la source de données pour forcer le rafraîchissement
            VoitureListBox.ItemsSource = Voitures; // Réaffecter la nouvelle collection
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshMateriels();
        }

        private void butVoirVoiture_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var voiture = button.DataContext as Voiture;
                if (voiture != null)
                {
                    // Afficher les détails de la voiture sélectionnée
                    AfficherVoitureSelectionnee(voiture);
                }
            }
        }

        private void AfficherVoitureSelectionnee(Voiture voiture)
        {
            // Mettre à jour les contrôles dans gridInfoVoiture avec les informations de la voiture sélectionnée
            CarBrand.Text = voiture.Marque;
            CarModel.Text = voiture.Modele;
            CarCategory.Text = voiture.Categorie;
            CarPrice.Text = voiture.Prix.ToString("C");

            // Mettre à jour l'image de la voiture
            CarImage.Source = voiture.GetImage();

            // Afficher la gridInfoVoiture et masquer la gridRecherche
            gridInfoVoiture.Visibility = Visibility.Visible;
            gridRecherche.Visibility = Visibility.Collapsed;
        }

        private void butSupprimerVoiture_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult reponse = MessageBox.Show("Etes vous sur de vouloir supprimer cette voiture", "Attention", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (reponse == MessageBoxResult.Yes)
            {
                var button = sender as Button;
                if (button == null) return;

                // Récupérer les données associées au bouton
                var selectedCar = button.DataContext as Voiture;
                if (selectedCar != null)
                {
                    // Supprimer la voiture de la base de données
                    data.Delete(selectedCar.Id);

                    // Supprimer la voiture de la liste des matériels
                    var carList = VoitureListBox.ItemsSource as ObservableCollection<Voiture>;
                    if (carList != null)
                    {
                        carList.Remove(selectedCar);
                    }

                }
                else
                {
                    MessageBox.Show("La voiture n'a pas pu être supprimée.");
                }
            }
        }


        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // Permet de rendre l'image accessible depuis d'autres threads
                return image;
            }
        }

        public static ImageSource ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }
}
