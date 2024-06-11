using BTP.Models;
using System.Linq;

namespace BTP.Models.Auth
{
    public class Authentif
    {
        public string? id { get; set; }
        public string login { get; set; }
        public string mdp { get; set; }
        public string role { get; set; }

        private readonly BtpDBContext _context;

        public Authentif(BtpDBContext context)
        {
            _context = context;
        }

        public Authentif(string? id, string login, string mdp, string role)
        {
            this.id = id;
            this.login = login;
            this.mdp = mdp;
            this.role = role;
        }

        public Authentif()
        {
        }

        public void insert(string log,string pass,string rol)
        {
            string hasher = Encodage.HashPassword(pass);
            Authentif newAuth = new Authentif
            {
                id = Functions_Comm.generateId("auth_seq", "Auth_"),
                login = log,
                mdp = hasher,
                role = rol
            };

            _context.auth.Add(newAuth);
            _context.SaveChanges();
        }

        public string[] verifWithRole(string login,string mdp)
        {
            string[] IdAndrole = new string[2];
            
            List<Authentif> auth = _context.auth.ToList();
            for (int i=0; i<auth.Count; i++)
            {
                bool hasher = Encodage.VerifyPassword(mdp,auth[i].mdp);
                if (hasher==true && login.GetHashCode() == auth[i].login.GetHashCode())
                {
                    Console.WriteLine("id: "+ auth[i].id+" role:"+ auth[i].role+" mdp: "+mdp);
                    IdAndrole[0] = auth[i].id;
                    IdAndrole[1] = auth[i].role;
                }
            }
            
            return IdAndrole;
        }
    }
}
