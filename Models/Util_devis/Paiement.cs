using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Util_devis
{
    public class Paiement
    {
        public string id {get; set;}
        public string ref_paiement {get; set;}
        public string id_devis {get; set;}
        public double montant {get; set;}
        public string date_payement { get; set; }

        public Paiement(string id, string ref_paiement, string id_devis, double montant, string date_payement)
        {
            this.id = id;
            this.ref_paiement = ref_paiement;
            this.id_devis = id_devis;
            this.montant = montant;
            this.date_payement = date_payement;
        }

        public Paiement()
        {
        }

        public static List<Paiement> getAll()
        {
            List<Paiement> paiements = new List<Paiement>();
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" select * from payement_devis  ", connection))
                {
                    

                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Paiement p = new Paiement();
                            p.id = reader.GetString(0);
                            p.ref_paiement = reader.GetString(1);
                            p.id_devis = reader.GetString(2);
                            p.montant = reader.GetDouble(3);
                            p.date_payement = reader.GetDateTime(4).ToString("yyyy-MM-dd");

                            paiements.Add(p);
                        }
                    }
                }
                connection.Close();
            }
            return paiements;
        }
        public void insert(string ref_paiement,string id_devis, double montant, string date_payement)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO payement_devis(ref_paiement,id_devis,montant,date_payement) VALUES (@ref,@devis,@montant,'"+ date_payement + "')", connection))
                {

                    command.Parameters.AddWithValue("@ref", ref_paiement);
                    command.Parameters.AddWithValue("@devis", id_devis);
                    command.Parameters.AddWithValue("@montant", montant);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public double getResteByDevis(string idDevis)
        {
            double sommeTotal = 0;
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(" select reste from v_payement where id_devis=@devis ", connection))
                {
                    command.Parameters.AddWithValue("@devis", id_devis);

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

    }
}
