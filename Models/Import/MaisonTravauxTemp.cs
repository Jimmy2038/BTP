using Cinepax.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace BTP.Models.Import
{
    public class MaisonTravauxTemp
    {
        [Required(ErrorMessage = "La colonne type_maison ne doit pas être vide")]
        public string type_maison {get; set;}


        [Required(ErrorMessage = "La colonne description ne doit pas être vide")]
        public string description {get; set;}


        [Required(ErrorMessage = "La colonne surface ne doit pas être vide")]
        public string surface {get; set;}


        [Required(ErrorMessage = "La colonne code_travaux ne doit pas être vide")]
        public string code_travaux {get; set;}


        [Required(ErrorMessage = "La colonne type_travaux ne doit pas être vide")]
        public string type_travaux {get; set;}


        [Required(ErrorMessage = "La colonne unite ne doit pas être vide")]
        public string unite {get; set;}

        [Required(ErrorMessage = "La colonne prix_unitaire ne doit pas être vide")]
        [Range(0, double.MaxValue, ErrorMessage = "La colonne prix_unitaire doit être supérieure ou égale à zéro")]
        public string prix_unitaire { get; set; }

        [Required(ErrorMessage = "La colonne quantite ne doit pas être vide")]
        [Range(0, double.MaxValue, ErrorMessage = "La colonne quantite doit être supérieure ou égale à zéro")]
        public string quantite { get; set; }

        [Required(ErrorMessage = "La colonne duree_travaux ne doit pas être vide")]
        [Range(0, int.MaxValue, ErrorMessage = "La colonne duree_travaux doit être supérieure ou égale à zéro")]
        public string duree_travaux { get; set; }

        public MaisonTravauxTemp(string type_maison, string description, string surface, string code_travaux, string type_travaux, string unite, string prix_unitaire, string quantite, string duree_travaux)
        {
            this.type_maison = type_maison;
            this.description = description;
            this.surface = surface;
            this.code_travaux = code_travaux;
            this.type_travaux = type_travaux;
            this.unite = unite;
            this.prix_unitaire = prix_unitaire;
            this.quantite = quantite;
            this.duree_travaux = duree_travaux;
        }

        public MaisonTravauxTemp()
        {
        }

        public void inserteChaqueTable()
        {
            DBPostgres db = new DBPostgres();


            using (NpgsqlConnection con = db.Connection())
            {
                con.Open();
                //maison
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO maison (designation,description,surface,duree) SELECT DISTINCT type_maison,description,surface::DOUBLE PRECISION,duree_travaux::INT FROM maison_traveau_temp", con))
                    {
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
                //travaux
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO travaux (code_travaux,type_travaux,unite,pu) SELECT DISTINCT code_travaux,type_travaux,unite,prix_unitaire::DOUBLE PRECISION from maison_traveau_temp", con))
                    {

                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreura: " + e.Message);
                }
                //
                try
                {
                    
                    using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO maison_traveau (id_maison,id_traveau,quantite)  SELECT m.id, t.id, mi.quantite::DOUBLE PRECISION FROM maison_traveau_temp mi join maison m on m.designation = mi.type_maison join travaux t on t.code_travaux = mi.code_travaux group by m.id, t.id, mi.quantite; ", con))
                    {

                        cmd.ExecuteNonQuery();
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
