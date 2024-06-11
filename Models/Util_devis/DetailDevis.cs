using Cinepax.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTP.Models.Util_devis
{
    public class DetailDevis
    {

    
        public string id_devis { get; set; }
        public string id_travaux { get; set; }
        public string code_travaux { get; set; }
        public string type_travaux { get; set; }
        public double pu { get; set; }
        public string unite { get; set; }
        public double prixTotal { get; set; }

        public double quantite { get; set; }
        

        private readonly BtpDBContext _context;

        public DetailDevis(BtpDBContext context)
        {
            _context = context;
        }

        public DetailDevis(string id_devis, string id_travaux, string code_travaux, string type_travaux, double pu, string unite, double prixTotal, double quantite)
        {
            this.id_devis = id_devis;
            this.id_travaux = id_travaux;
            this.code_travaux = code_travaux;
            this.type_travaux = type_travaux;
            this.pu = pu;
            this.unite = unite;
            this.prixTotal = prixTotal;
            this.quantite = quantite;
        }

        public DetailDevis()
        {
        }

        public List<DetailDevis> getAllIdTravauxByDevis(string idDevis)
        {
            List<DetailDevis> vs = new List<DetailDevis>();

            
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM v_devis where devis = '"+idDevis+"'", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DetailDevis de = new DetailDevis
                            {
                                code_travaux = reader.GetString(0),
                                type_travaux = reader.GetString(1),
                                id_travaux = reader.GetString(2),
                                id_devis = reader.GetString(3),
                                pu = reader.GetDouble(4),
                                unite = reader.GetString(5),
                                quantite = reader.GetDouble(6),
                                prixTotal = reader.GetDouble(7)
                            };
                            vs.Add(de);
                        }
                    }
                }
                connection.Close();
            }
            return vs;
        }
    }
}
