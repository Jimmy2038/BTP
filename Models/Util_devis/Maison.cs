using Cinepax.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTP.Models.Util_devis
{
    public class Maison
    {
        public string id { get; set; }
        public string designation { get; set; }
        public string description { get; set; }
        public double surface { get; set; }
        public int duree { get; set; }
        [NotMapped]
        public double? quantite { get; set; }
        public Maison()
        {
        }

        public int getDuree(string idMaison)
        {
                int duree = 0;
                DBPostgres db = new DBPostgres();
                using (NpgsqlConnection connection = db.Connection())
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("select *from maison where id = @id ", connection))
                    {
                        command.Parameters.AddWithValue("@id", idMaison);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                duree = reader.GetInt32(4);
                            }
                        }
                    }
                    connection.Close();
                }

            return duree;
            
        }

        public Maison getMaisonsByDevis(string idDevis)
        {
            Maison all = new Maison();
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select*from devis d join maison m on d.id_maison=m.id where d.id = @id ", connection))
                {
                    command.Parameters.AddWithValue("@id", idDevis);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Maison maison = new Maison
                            {
                                id = reader.GetString(9),
                                designation = reader.GetString(10),
                                description = reader.GetString(11),
                                surface = reader.GetDouble(12),
                                duree = reader.GetInt32(13)
                            };
                            all = maison;
                        }
                    }
                }
                connection.Close();
            }
            return all;
        }
    }
}
