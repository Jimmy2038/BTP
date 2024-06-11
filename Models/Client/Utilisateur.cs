using BTP.Models.Auth;
using Cinepax.Models;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTP.Models.Client
{
    public class Utilisateur
    {
        public string? id { get; set; }

        //[Required(ErrorMessage = "Cette champ nom est requis")]
        public string? nom { get; set; }

        //[Required(ErrorMessage = "Cette champ prenom est requis")]
        public string? prenom { get; set; }

        //[Required(ErrorMessage = "Cette champ date est requis")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Date de Naissance")]
        public string? date_naissance { get; set; }

        //[Required(ErrorMessage = "Cette champ addresse est requis")]
        public string? adresse { get; set; }

        [Required(ErrorMessage = "Cette champ est requis")]
        [RegularExpression(@"^0[23][2348]\d{7}$", ErrorMessage = "Le numéro ne correspond pas au critère necessaire")]
        public string numero { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "Cette champ est requis")]
        //[EmailAddress(ErrorMessage = "Veuillez entrer une adresse email valide")]
        public string? email { get; set; }


        [NotMapped]
        //[Required(ErrorMessage = "Cette champ est requis")]
        //[MinLength(5, ErrorMessage = "Le mot de passe doit contenir au moins 5 caractères")]
        public string? mdp { get; set; }

        private readonly BtpDBContext _context;

        public Utilisateur(BtpDBContext context)
        {
            _context = context;
        }

        public Utilisateur(string? id, string nom, string prenom, string date_naissance, string adresse, string numero, string email, string mdp)
        {
            this.id = id;
            this.nom = nom;
            this.prenom = prenom;
            this.date_naissance = date_naissance;
            this.adresse = adresse;
            this.numero = numero;
            this.email = email;
            this.mdp = mdp;
        }

        public Utilisateur()
        {
        }

        public void insertUserAuth(string nom, string prenom, string date_naissances, string adresse, string numero, string mail, string mdp)
        {
            Authentif auth = new Authentif(_context);
            auth.insert(mail, mdp, "client");
            auth = _context.auth.OrderBy(a => a.id).Last();
            
            this.insert(nom, prenom, date_naissances, adresse, numero);
            /*Utilisateur newUser = new Utilisateur
            {
                id = Functions_Comm.generateId("auth_seq", "User_"),
                nom = nom,
                prenom = prenom,
                date_naissance = date_naissance,
                adresse = adresse,
                numero = numero,
                id_auth = auth.id
            };
            _context.user.Add(newUser);
            _context.SaveChanges();*/
        }

        public void insert(string nom, string prenom, string date_naissances, string adresse, string numero)
        {

            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO utilisateur(nom,prenom,date_naissance,adresse,numero) VALUES(@nom,@prenom,'"+ date_naissances + "',@adresse,@num)", connection))
                {
                    command.Parameters.AddWithValue("@nom", nom);
                    command.Parameters.AddWithValue("@prenom", prenom);
                    command.Parameters.AddWithValue("@adresse", adresse);
                    command.Parameters.AddWithValue("@num", numero);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void insertNumero(string numero)
        {
            DBPostgres db = new DBPostgres();
            using (NpgsqlConnection connection = db.Connection())
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO client(numero) VALUES(@num)", connection))
                {
                    command.Parameters.AddWithValue("@num", numero);

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
