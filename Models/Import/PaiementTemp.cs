using BTP.Models.Util_devis;
using Cinepax.Models;
using Npgsql;

namespace BTP.Models.Import
{
    public class PaiementTemp
    {
        public string ref_devis {get; set;}
        public string ref_paiement {get; set;}
        public string date_paiement {get; set;}
        public string montant { get; set; }

        public PaiementTemp()
        {
        }

        public PaiementTemp(string ref_devis, string ref_paiement, string date_paiement, string montant)
        {
            this.ref_devis = ref_devis;
            this.ref_paiement = ref_paiement;
            this.date_paiement = date_paiement;
            this.montant = montant;
        }

        public void inserteChaqueTable()
        {
            DBPostgres db = new DBPostgres();


            using (NpgsqlConnection con = db.Connection())
            {
                con.Open();
                //finition
                List<Paiement> allpayemet = Paiement.getAll();
                try
                {
                    //INSERT INTO payement_devis(ref_paiement,id_devis,date_payement,montant) SELECT ref_paiement ,d.id, date_paiement::DATE, montant::DOUBLE PRECISION FROM payement_temp pt join devis d on d.ref_devis = pt.ref_devis group by ref_paiement, d.id, date_paiement::DATE, montant::DOUBLE PRECISION; 
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT d.id, ref_paiement, date_paiement ,montant FROM payement_temp pt join devis d on d.ref_devis=pt.ref_devis", con))
                    {
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bool inserer = true;
                                for(int i=0; i < allpayemet.Count; i++)
                                {
                                    Console.WriteLine("paiement temp: "+ reader.GetString(1)+", paiement devis: "+ allpayemet[i].ref_paiement);
                                    if (allpayemet[i].ref_paiement == reader.GetString(1))
                                    {
                                        inserer = false;
                                    }
                                }
                                if (inserer == true)
                                {
                                    Console.WriteLine("etat: " + inserer+" ampidirina: "+ reader.GetString(1)+" devis: "+ reader.GetString(0));
                                    Paiement p = new Paiement();

                                    p.insert(reader.GetString(1), reader.GetString(0),Double.Parse(reader.GetString(3)), reader.GetString(2));
                                    /*using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO payement_devis(ref_paiement,id_devis,date_payement,montant) VALUES ('"+reader.GetString(1)+ "','" + reader.GetString(0) + "','" + reader.GetString(2) + "'," + reader.GetString(3) + "::DOUBLE PRECISION)", con))
                                    {
                                        command.ExecuteNonQuery();
                                    }*/
                                }
                            }
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
            }
        }


    }   
}
