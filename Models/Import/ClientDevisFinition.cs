using BTP.Models.Util_devis;
using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Import
{
    public class ClientDevisFinition
    {
        public string client {get; set;}
        public string ref_devis {get; set;}
        public string type_maison {get; set;}
        public string finition {get; set;}
        public string taux_finition {get; set;}
        public string date_devis {get; set;}
        public string date_debut {get; set;}
        public string lieu { get; set; }

        public ClientDevisFinition(string client, string ref_devis, string type_maison, string finition, string taux_finition, string date_devis, string date_debut, string lieu)
        {
            this.client = client;
            this.ref_devis = ref_devis;
            this.type_maison = type_maison;
            this.finition = finition;
            this.taux_finition = taux_finition;
            this.date_devis = date_devis;
            this.date_debut = date_debut;
            this.lieu = lieu;
        }

        public ClientDevisFinition()
        {
        }

        private readonly BtpDBContext _context;

        public ClientDevisFinition(BtpDBContext context)
        {
            _context = context;
        }
        public void inserteChaqueTable()
        {
            DBPostgres db = new DBPostgres();


            using (NpgsqlConnection con = db.Connection())
            {
                con.Open();
                //finition
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO finition(type,pourcentage) SELECT DISTINCT finition,taux_finition::DOUBLE PRECISION FROM devis_temp", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
                //client
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO client (numero) SELECT DISTINCT client from devis_temp", con))
                    {

                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
                //devis et detail devis
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT dt.ref_devis,m.id,f.id,c.id,dt.date_devis,dt.date_debut,dt.lieu FROM devis_temp dt join maison m on m.designation = dt.type_maison join finition f on f.type = dt.finition join client c on c.numero = dt.client group by dt.ref_devis, m.id, f.id, c.id, dt.date_devis, dt.date_debut, dt.lieu ", con))
                    {

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Devis devis = new Devis();
                                devis.insert(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4),reader.GetString(5),reader.GetString(6));
                                DetailMaison detail = new DetailMaison(_context);
                                detail.insert();
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
                con.Close();
            }
        }

        
    }
}
