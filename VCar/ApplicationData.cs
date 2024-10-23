using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace VCar
{

    public class ApplicationData
    {
        private ObservableCollection<Materiel> lesMateriels;
        private NpgsqlConnection connexion = null;   // futur lien à la BD

        

        public ObservableCollection<Materiel> LesMateriels
        {
            get { return this.lesMateriels; }
            set { this.lesMateriels = value; }
        }


        public NpgsqlConnection Connexion
        {
            get
            {
                return this.connexion;
            }

            set
            {
                this.connexion = value;
            }
        }

        public ApplicationData()
        {
            this.ConnexionBD();
            this.Read();
        }
        public void ConnexionBD()
        {
            try
            {
                Connexion = new NpgsqlConnection();
                Connexion.ConnectionString = "Server=ep-morning-wood-a27nq1on.eu-central-1.aws.neon.tech;" +
                                            "port=5432;" +
                                            "Database=eligiusdb;" +
                                            "uid=eligiusdb_owner;" +
                                            "password=tf4Ag8qwiJoT;";
                //à compléter dans les "" 
                // @ sert à enlever tout pb avec les caractères 
                Connexion.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("pb de connexion : " + e);
                // juste pour le debug : à transformer en MsgBox 
            }
        }

        public int Read()
        {
            LesMateriels = new ObservableCollection<Materiel>();
            String sql = "select CHEMINIMAGE, ID, MARQUE, MODELE, CATEGORIE, PRIX from VOITURE";

            try
            {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                foreach (DataRow res in dataTable.Rows)
                {
                    Materiel nouveau = new Materiel(res["CHEMINIMAGE"].ToString(), int.Parse(res["ID"].ToString()), res["MARQUE"].ToString(), res["MODELE"].ToString(),
                    res["CATEGORIE"].ToString(), int.Parse(res["PRIX"].ToString()));
                    LesMateriels.Add(nouveau);
                }

                return dataTable.Rows.Count;
            }
            catch (NpgsqlException e)
            { Console.WriteLine("pb de requete : " + e); return 0; }


        }


        public int Create()
        {
            String sql = "";
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, Connexion);
                int nb = cmd.ExecuteNonQuery();
                return nb;
                //nb permet de connaître le nb de lignes affectées par un insert, update, delete
            }
            catch (Exception sqlE)
            {
                Console.WriteLine("pb de requete : " + sql + "" + sqlE);
                // juste pour le debug : à transformer en MsgBox 
                return 0;
            }
        }

        public int Update()
        {
            String sql = $"";
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, Connexion);
                int nb = cmd.ExecuteNonQuery();
                return nb;
                //nb permet de connaître le nb de lignes affectées par un insert, update, delete
            }
            catch (Exception sqlE)
            {
                Console.WriteLine("pb de requete : " + sql + "" + sqlE);
                // juste pour le debug : à transformer en MsgBox 
                return 0;
            }

        }

        public int Delete()
        {
            String sql = $"";
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sql, Connexion);
                int nb = cmd.ExecuteNonQuery();
                return nb;
                //nb permet de connaître le nb de lignes affectées par un insert, update, delete
            }
            catch (Exception sqlE)
            {
                Console.WriteLine("pb de requete : " + sql + "" + sqlE);
                // juste pour le debug : à transformer en MsgBox 
                return 0;
            }

        }

        public void RefreshMateriels()
        {
            try
            {
                // Vider la collection actuelle
                LesMateriels.Clear();

                // Refaire la requête pour obtenir les matériels
                String sql = "select CHEMINIMAGE, ID, MARQUE, MODELE, CATEGORIE, PRIX from VOITURE";

                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connexion);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // Remplir à nouveau la collection
                foreach (DataRow res in dataTable.Rows)
                {
                    Materiel nouveau = new Materiel(res["CHEMINIMAGE"].ToString(), int.Parse(res["ID"].ToString()),
                                                    res["MARQUE"].ToString(), res["MODELE"].ToString(),
                                                    res["CATEGORIE"].ToString(), int.Parse(res["PRIX"].ToString()));
                    LesMateriels.Add(nouveau);
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("pb de requête lors du rafraîchissement : " + e);
                // Gestion d'erreur : à transformer en MsgBox
            }
        }




        public void DeconnexionBD()
        {
            try
            {
                Connexion.Close();
            }
            catch (Exception e)
            { Console.WriteLine("pb à la déconnexion : " + e); }
        }



    }
}
