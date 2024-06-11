using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Util_devis
{
    public class Finition
    {
        public string id {get; set;}
        public string type {get; set;}
        public double pourcentage { get; set; }

        public Finition()
        {        }

        public List<Finition> getAll()
        {
            List<Finition> finitions = new List<Finition>();
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" SELECT * FROM finition ", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Finition finition = new Finition
                            {
                                id = reader.GetString(0),
                                type = reader.GetString(1),
                                pourcentage = reader.GetDouble(2)
                            };
                            finitions.Add(finition);
                        }
                    }
                }
                connection.Close();
            }
            return finitions;
        }

        public void update(string id, double pourcentage)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("update finition set  pourcentage = @pourcentage where id=@id", connection))
                {

                    command.Parameters.AddWithValue("@pourcentage", pourcentage);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
