using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        public ObservableCollection<Materiel> LesVoitures = new ObservableCollection<Materiel>();
        
        public MainWindow()
        {
            InitializeComponent();
            txtIdentifiant.Focus();
            EquipmentList.ItemsSource = data.LesMateriels;
            LesVoitures = data.LesMateriels;
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

        private bool ContientMotClef(object obj)
        {
            Materiel unMateriel = obj as Materiel;

            if (String.IsNullOrEmpty(txtRecherche.Text))
            {
                return true;
            }

            else
                return (unMateriel.Marque.StartsWith(txtRecherche.Text, StringComparison.OrdinalIgnoreCase)
                || unMateriel.Modele.StartsWith(txtRecherche.Text, StringComparison.OrdinalIgnoreCase));
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

        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtRecherche.Text.ToLower();

            // Assurez-vous que 'LesMateriels' est une ObservableCollection ou une liste que vous pouvez filtrer
            var filteredList = data.LesMateriels
                .Where(m => m.Marque.ToLower().Contains(searchText) ||
                             m.Modele.ToLower().Contains(searchText) ||
                             m.Categorie.ToLower().Contains(searchText))
                .ToList();

            // Mettre à jour la source de données de la ListBox
            if (filteredList.Count > 0) 
            {
                EquipmentList.ItemsSource = filteredList;
            }
            
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            data.RefreshMateriels();
        }

        private void butVoirVoiture_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            // Récupérer les données associées au bouton
            var selectedCar = button.DataContext as Materiel;

            if (selectedCar != null)
            {
                // Mettre à jour les détails de la voiture dans la grille "CarDetailsGrid"
                CarImage.Source = new BitmapImage(new Uri(selectedCar.CheminImage, UriKind.RelativeOrAbsolute));
                CarBrand.Text = selectedCar.Marque;
                CarModel.Text = selectedCar.Modele;
                CarCategory.Text = selectedCar.Categorie;
                CarPrice.Text = selectedCar.Prix.ToString("C");

                // Afficher la grille "CarDetailsGrid" et masquer la grille "gridRecherche"
                gridInfoVoiture.Visibility = Visibility.Visible;
                gridRecherche.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une voiture valide.");
            }
        }
    }
}