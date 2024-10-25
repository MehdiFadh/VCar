using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;

namespace VCar
{
    public class Voiture
    {
        public int Id { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public string Categorie { get; set; }
        public float Prix { get; set; }
        public byte[] CheminImage { get; set; }

        // Méthode pour convertir le tableau d'octets en BitmapImage
        public BitmapImage GetImage()
        {
            if (CheminImage == null || CheminImage.Length == 0) return null;

            try
            {
                BitmapImage image = null;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    using (var ms = new MemoryStream(CheminImage))
                    {
                        image = new BitmapImage();
                        ms.Position = 0;
                        image.BeginInit();
                        image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.StreamSource = ms;
                        image.EndInit();
                        image.Freeze();
                    }
                    Console.WriteLine("bonjour");
                });
                return image;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la conversion de l'image : {ex.Message}");
                return null;
            }
        }

        public BitmapImage ImageSource
        {
            get { return GetImage(); }
        }

        public Voiture(byte[] cheminImage, int id, string marque, string modele, string categorie, float prix)
        {
            CheminImage = cheminImage;
            Id = id;
            Marque = marque;
            Modele = modele;
            Categorie = categorie;
            Prix = prix;
        }

        public Voiture()
        {
        }
    }
}
