using Cinepax.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTP.Models.Util_devis
{
    public class Devis
    {
        public string? id { get; set; }
        public string? ref_devis { get; set; }
        public string? id_maison { get; set; }
        public string? id_finition { get; set; }
        public string? id_client { get; set; }
        public string? date_devis { get; set; }
        public string? date_debut { get; set; }
        public string? date_fin { get; set; }
        public string? lieu { get; set; }
        [NotMapped]
        public double? montantTotalParMoi { get; set; }
        [NotMapped]
        public Maison? maison { get; set; }

        internal double getgetSommePayementByDevis(string id_devis)
        {
            throw new NotImplementedException();
        }

        [NotMapped]
        public Finition? finition { get; set; }
        [NotMapped]
        public List<Travaux>? travaux { get; set; }
        public Devis()
        {
        }

        private readonly BtpDBContext _context;

        public Devis(BtpDBContext context)
        {
            _context = context;
        }

        public double getSommePayementByDevis(string id_devis)
        {
            double monantTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select * from v_totalpayementdevis where id_devis=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", id_devis);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            monantTotal = reader.GetDouble(1);

                        }
                    }
                }
                connection.Close();
            }
            return monantTotal;
        }
        public List<Paiement> getPayementByDevis(string idDevis)
        {
            List<Paiement> allAnnee = new List<Paiement>();
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select* from v_listpaiement where id_devis=@id", connection))
                {
                    command.Parameters.AddWithValue("@id", idDevis);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Paiement p = new Paiement();
                            p.id = reader.GetString(2);
                            p.id_devis = reader.GetString(0);
                            p.montant = reader.GetDouble(1);
                            allAnnee.Add(p);

                        }
                    }
                }
                connection.Close();
            }
            return allAnnee;
        }
        public List<string> getAnneeInDevis()
        {
            List<string> allAnnee= new List<string>();
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT EXTRACT(YEAR FROM date_devis) AS year FROM devis GROUP BY year ORDER BY year", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            int year = reader.GetInt32(0);
                            allAnnee.Add(year.ToString());

                        }
                    }
                }
                connection.Close();
            }
            return allAnnee;
        }
        public double getTotalPayement()
        {
            double monantTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select sum(payer) from v_payer", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Montant anaty fonction: " + reader.GetDouble(0));
                            monantTotal = reader.GetDouble(0);

                        }
                    }
                }
                connection.Close();
            }
            return monantTotal;
        }
        public double getPourcentagePayerByDevis(string id_devis)
        {
            double monantTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select*from  v_pourcentagePayer where id_devis ='" + id_devis + "'", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Montant anaty fonction: " + reader.GetDouble(1));
                            monantTotal = reader.GetDouble(1);

                        }
                    }
                }
                connection.Close();
            }
            return monantTotal;
        }

        public double getMontantPayerByDevis(string id_devis)
        {
            double monantTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select*from v_payer where id_devis ='" + id_devis + "'", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Montant anaty fonction: " + reader.GetDouble(1));
                            monantTotal = reader.GetDouble(1);

                        }
                    }
                }
                connection.Close();
            }
            return monantTotal;
        }

        public double getMontantTotalByDevis(string id_devis)
        {
            double monantTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("select*from v_prixTotal_devis where devis ='" + id_devis + "'", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Montant anaty fonction: "+ reader.GetDouble(1));
                            monantTotal = reader.GetDouble(1);
                            
                        }
                    }
                }
                connection.Close();
            }
            return monantTotal;
        }
        public List<Devis> getListMontantDevisParMois(string annee)
        {
            List<Devis> devis = new List<Devis>();
            double sommeTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" WITH all_months as (select generate_series(date_trunc('year',   '"+ annee + "-01-01'::date), date_trunc('year',  '" + annee + "-01-01'::date) + INTERVAL '1 year -1 day', '1 month') as month)select to_char(all_months.month,'YYYY-MM') as month,coalesce(sum(v_detail_devis.prixtotaldevis), 0) as nb from all_months left join v_detail_devis on date_trunc('month', v_detail_devis.date_devis) = all_months.month group by month order by month ", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Devis devis1 = new Devis
                            {
                               montantTotalParMoi = reader.GetDouble(1)
                            };
                            devis.Add(devis1);
                        }
                    }
                }
                connection.Close();
            }
            return devis;
        }
        public double montantTotalDevis()
        {

            double sommeTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" select sum(prixtotalDevis) from v_detail_devis ", connection))
                {

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sommeTotal = reader.GetDouble(0);
                        }
                    }
                }
                connection.Close();
            }
            return sommeTotal;
        }
        public void insert(string ref_devis, string id_maison, string id_finition, string id_client, string date_devis, string date_debut,string lieu)
        {
            string date_fins = this.previsionFinConstruction(id_maison, date_debut);
            DBPostgres db = new DBPostgres();
            Console.WriteLine(date_fin);
            Console.WriteLine("date debut: " + date_debut);
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO devis(ref_devis,id_maison,id_finition,id_client,date_devis,date_debut,date_fin,lieu) VALUES (@ref_devis,@id_maison,@id_finition,@id_client,'" + date_devis + "','" + date_debut + "','" + date_fins + "',@lieu)", connection))
                {
                    command.Parameters.AddWithValue("@id_maison", id_maison);
                    command.Parameters.AddWithValue("@id_finition", id_finition);
                    command.Parameters.AddWithValue("@id_client", id_client); 
                    command.Parameters.AddWithValue("@ref_devis", ref_devis); 
                    command.Parameters.AddWithValue("@lieu", lieu);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public double prixTotalDevis(string idMaison)
        {
            double prixTotalDevis = 0;

            double prixMaison = this.getPrixTotalMaison(idMaison);
            double prixFinition = this.coutFinition(idMaison);

            prixTotalDevis = prixMaison + prixFinition;
            return prixTotalDevis;
        }
        public string previsionFinConstruction(string idMaison,string date)
        {
            Maison maison = new Maison();

            int duree = maison.getDuree(idMaison); 
            date = AdditionDate(date, duree);
            return date;
        }

        public double getPrixTotalMaison(string idMaison)
        {
            double prixMaisonTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" SELECT * FROM v_prix_total_maison WHERE maison = @id ", connection))
                {
                    command.Parameters.AddWithValue("@id", idMaison);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prixMaisonTotal = reader.GetDouble(1);
                        }
                    }
                }
                connection.Close();
            }
            return prixMaisonTotal;
        }

        public double coutFinition(string idMaison) 
        {
            double prixMaisonTotal = this.getPrixTotalMaison(idMaison);
            double pourcentage = this.getPourcentage(idMaison);
            
            double coutFinition = (prixMaisonTotal * pourcentage) / 100;

            return coutFinition;
        }

        public string AdditionDate(string dateStr, int duree)
        {

            DateTime date = DateTime.ParseExact(dateStr, "dd/MM/yyyy", null);
            DateTime nouveauDate = date.AddDays(duree);
            string nouveauDateStr = nouveauDate.ToString("dd/MM/yyyy");

            return nouveauDateStr;
        }

        public double getPourcentage(string idMaison)
        {
            double pourcentage = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(" select*from v_pourcentage_maison WHERE id_maison = @id ", connection))
                {
                    cmd.Parameters.AddWithValue("@id", idMaison);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pourcentage = reader.GetDouble(2);
                        }
                    }
                }
                connection.Close();
            }
            return pourcentage;
        }

        public List<Devis> GetDevisByIdClient(string idClient)
        {
            List<Devis> devisList = new List<Devis>();

            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM devis WHERE id_client = @idClient", connection))
                {
                    command.Parameters.AddWithValue("@idClient", idClient);

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Devis devis = new Devis
                            {
                                id = reader.GetString(0),
                                ref_devis = reader.GetString(1),
                                id_maison = reader.GetString(2),
                                id_finition = reader.GetString(3),
                                id_client = reader.GetString(4),
                                date_devis = reader.GetDateTime(5).ToString("yyyy-MM-dd"),
                                date_debut = reader.GetDateTime(6).ToString("yyyy-MM-dd"),
                                date_fin = reader.GetDateTime(7).ToString("yyyy-MM-dd"),
                                lieu = reader.GetString(8)
                            };

                            devisList.Add(devis);
                        }
                    }
                }

                connection.Close();
            }

            return devisList;
        }

        public List<Devis> getAll()
        {
            List<Devis> devisList = new List<Devis>();

            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM devis ", connection))
                {


                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Devis devis = new Devis
                            {
                                id = reader.GetString(0),
                                ref_devis = reader.GetString(1),
                                id_maison = reader.GetString(2),
                                id_finition = reader.GetString(3),
                                id_client = reader.GetString(4),
                                date_devis = reader.GetDateTime(5).ToString("yyyy-MM-dd"),
                                date_debut = reader.GetDateTime(6).ToString("yyyy-MM-dd"),
                                date_fin = reader.GetDateTime(7).ToString("yyyy-MM-dd"),
                                lieu = reader.GetString(8)
                            };

                            devisList.Add(devis);
                        }
                    }
                }

                connection.Close();
            }

            return devisList;
        }
    }
}
