﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if(txtIdentifiant.Text == "mehdi" && txtPassword.Password == "password")
            {
                gridConnexion.Visibility = Visibility.Collapsed;
                this.Title = "Vcar - Accueil";
            }
        }
    }
}