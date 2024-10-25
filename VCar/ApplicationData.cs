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
using System.Windows;
using System.Windows.Media.Media3D;

namespace VCar
{

    public class ApplicationData
    {
        private NpgsqlConnection connexion = null;   // futur lien à la BD
        private string connectionString = "Server=ep-morning-wood-a27nq1on.eu-central-1.aws.neon.tech;" +
                                            "port=5432;" +
                                            "Database=eligiusdb;" +
                                            "uid=eligiusdb_owner;" +
                                            "password=tf4Ag8qwiJoT;";




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
            this.GetVoitures();
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
                MessageBox.Show(e.Message);
            }
        }

        public ObservableCollection<Voiture> GetVoitures()
        {
            var voitures = new ObservableCollection<Voiture>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT id, marque, modele, categorie, prix, cheminImage FROM voituree", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var voiture = new Voiture
                        {
                            Id = reader.GetInt32(0),
                            Marque = reader.GetString(1).Trim(),
                            Modele = reader.GetString(2).Trim(),
                            Categorie = reader.GetString(3).Trim(),
                            Prix = (float)reader.GetDouble(4),
                            CheminImage = reader["cheminImage"] as byte[] // Récupération du tableau d'octets
                        };

                        voitures.Add(voiture);
                    }
                }
            }

            return voitures;
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

        public int Delete(int id)
        {
            String sql = $"DELETE FROM VOITUREE WHERE ID =" + id;
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
