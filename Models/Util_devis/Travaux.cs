using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Util_devis
{
    public class Travaux
    {
        public string id {get; set;}
        public string code_travaux {get; set;}
        public string type_travaux { get; set;}
        public string unite {get; set;}
        public double pu {get; set;}

        public Travaux()
        {
        }

        private readonly BtpDBContext _context;

        public Travaux(BtpDBContext context)
        {
            _context = context;
        }

        public void update(string id, string type_travaux, string unite, double pu)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("update travaux set  type_travaux = @type, unite=@unite,pu=@pu where id=@id", connection))
                {
                    
                    command.Parameters.AddWithValue("@type", type_travaux);
                    command.Parameters.AddWithValue("@unite", unite);
                    command.Parameters.AddWithValue("@pu", pu);
                    command.Parameters.AddWithValue("@id", id);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        
    }
}
