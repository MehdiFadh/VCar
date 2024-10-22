using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCar
{
    public class Materiel
    {
		private int id;

		public int Id
		{
			get { return this.id; }
			set { this.id = value; }
		}

		private string cheminImage;

		public string CheminImage
		{
			get { return this.cheminImage; }
			set { this.cheminImage = value; }
		}


		private string marque;

		public string Marque
		{
			get { return this.marque; }
			set { this.marque = value; }
		}

		private string categorie;

		public string Categorie
		{
			get { return this.categorie; }
			set { this.categorie = value; }
		}

		private string modele;

		public string Modele
		{
			get { return this.modele; }
			set { this.modele = value; }
		}


		private double prix;

		public double Prix
		{
			get { return this.prix; }
			set { this.prix = value; }
		}

        public Materiel(int id, string marque, string modele, string categorie, double prix, string cheminImage)
        {
            Id = id;
            Marque = marque;
            Modele = modele;
            Categorie = categorie;
            Prix = prix;
            CheminImage = cheminImage;
        }

        public Materiel(string cheminImage, int id, string marque, string modele, string categorie, double prix)
        {
            CheminImage = cheminImage;
            Id = id;
            Marque = marque;
            Modele = modele;
            Categorie = categorie;
            Prix = prix;
        }

        public Materiel()
        {
        }

        public Materiel(int id, string cheminImage, string marque, string categorie, string modele, double prix)
        {
            Id = id;
            CheminImage = cheminImage;
            Marque = marque;
            Categorie = categorie;
            Modele = modele;
            Prix = prix;
        }
    }
}
