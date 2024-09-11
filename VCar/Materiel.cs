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

		private string nom;

		public string Nom
		{
			get { return this.nom; }
			set { this.nom = value; }
		}

		private string categorie;

		public string Categorie
		{
			get { return this.categorie; }
			set { this.categorie = value; }
		}

		private double prix;

		public double Prix
		{
			get { return this.prix; }
			set { this.prix = value; }
		}

        public Materiel(int id, string nom, string categorie, double prix)
        {
            Id = id;
            Nom = nom;
            Categorie = categorie;
            Prix = prix;
        }

        public Materiel()
        {
        }
    }
}
