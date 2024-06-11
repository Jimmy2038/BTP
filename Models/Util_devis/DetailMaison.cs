using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Util_devis
{
    public class DetailMaison
    {
        public string id { get; set; }
        public string id_devis {get; set;}
        public string id_travau {get; set;}
        public double qantite {get; set;}
        public int duree {get; set;}
        public double pu {get; set;}
        public double pourcentage { get; set; }

        private readonly BtpDBContext _context;

        public DetailMaison(BtpDBContext context)
        {
            _context = context;
        }
        public DetailMaison()
        {
        }
        
        public void insert()
        {
            string dernierDevisId = _context.devis.OrderByDescending(m => m.id).Select(m => m.id).FirstOrDefault();
            string idMaison = _context.devis.OrderByDescending(m => m.id).Select(m => m.id_maison).FirstOrDefault();
            Devis devis = new Devis();
            Console.WriteLine("dernier devis:"+ dernierDevisId);
            Console.WriteLine("IdMaison: "+ idMaison);
            double pourcentage = devis.getPourcentage(idMaison);
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" SELECT * FROM  v_info_Maison_travail WHERE maison = @id ", connection))
                {
                    command.Parameters.AddWithValue("@id", idMaison);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //typetravail | maison | id_travaux | unite | pu | quantite | duree | prixtot
                            DetailMaison detail = new DetailMaison
                            {
                                id = Functions_Comm.generateId("deatail_maison_seq","DETAIL_"),
                                id_devis = dernierDevisId,
                                id_travau = reader.GetString(1),
                                duree = reader.GetInt32(5),
                                pourcentage = pourcentage,
                                pu = reader.GetDouble(3),
                                qantite = reader.GetDouble(4)
                            };

                            _context.detailMaisons.Add(detail);
                            _context.SaveChanges();
                        }
                    }
                }
                connection.Close();
            }
        }


    }
}
