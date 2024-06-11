using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using BTP.Models.Auth;
using BTP.Models;
using BTP.Models.Client;
using BTP.Views.User;
using BTP.Models.Util_devis;

namespace Cinepax.Controllers
{
    public class AuthController : Controller
    {
        private readonly BtpDBContext _context;

        public AuthController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            Authentif auth = new Authentif(_context);

            Devis devis = new Devis();
            //double datFin = devis.prixTotalDevis("MAIS_00002");
            List<Devis> devis1 = new List<Devis>();

            //devis.insert("MAIS_00002", "FIN_00001", "User_00001", "31/03/2001", "31/03/2001");
            //Console.WriteLine("Prix Total Devis: ");
            string role = HttpContext.Session.GetString("Role");
            
            if (claimUser.Identity.IsAuthenticated)
            {
                if(role == "Admin")
                {
                    return RedirectToAction("Index","Admin");
                }
                else if (role == "client")
                {
                    return RedirectToAction("Index", "Client");

                }
            }
            string numero = HttpContext.Session.GetString("numero");
            if (numero != null)
            {
                return View("../User/ClientLogin");
            }
            return View("../Auth/auth-login");
        }

        public IActionResult LoginClient()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            /*if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Client");*/

            return View("../User/ClientLogin");
        }

        [HttpPost]
        public async Task<IActionResult> LoginClient(ValidationClietn modelLogin)
        {
            if (!ModelState.IsValid)
            {
                return View("../User/ClientLogin");
            }

            Utilisateur user = new Utilisateur(_context);
            user = _context.user
            .Where(u => u.numero == modelLogin.numero).FirstOrDefault();
            //Console.WriteLine("User: "+user.numero);
            if (user == null)
            {
                user = new Utilisateur(_context);
                user.insertNumero(modelLogin.numero);
            }
            List<Claim> reclamation = new List<Claim>()
                        {
                            new Claim(ClaimTypes.StreetAddress, modelLogin.numero),
                            new Claim(ClaimTypes.Role, "Client")
                        };
            
            HttpContext.Session.SetString("Role", "client");
            HttpContext.Session.SetString("numero", modelLogin.numero);
            HttpContext.Session.SetString("idUser", user.id);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(reclamation,
                    CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties propriete = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = modelLogin.resterConnecter
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), propriete);

            return RedirectToAction("Index", "Client");
        }
            [HttpPost]
        public async Task<IActionResult> Login(VMLogin modelLogin)
        {
            Authentif auth = new Authentif(_context);
            string[] test = auth.verifWithRole(modelLogin.Email, modelLogin.mdp);
            //auth.insert("admin@gmail.com","admin","Admin");
            test[1] = "Admin";
            if (test[1] != null)
            {
                if (test[1].GetHashCode() == "Admin".GetHashCode())
                {
                    Console.WriteLine("tafiditra");
                    List<Claim> reclamation = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, modelLogin.Email),
                            new Claim(ClaimTypes.Role, "Admin")
                        };
                    HttpContext.Session.SetString("Role", "Admin");
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(reclamation,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties propriete = new AuthenticationProperties()
                    {
                        AllowRefresh = true,
                        IsPersistent = modelLogin.resterConnecter
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), propriete);

                    return RedirectToAction("Index", "Admin");
                }
            }
            ViewData["ErreurLogin"] = "Utilisateur non trouver";
            return View("../Auth/auth-login");
        }
            /*[HttpPost]
            public async Task<IActionResult> Login(VMLogin modelLogin)
            {
                Authentif auth = new Authentif(_context);
                string[] test = auth.verifWithRole(modelLogin.Email, modelLogin.mdp);

                Console.WriteLine("Resultat verif:" + test[1]);
                if (test.Length >0)
                {
                    if (test[1].GetHashCode() == "Admin".GetHashCode())
                    {
                        Console.WriteLine("tafiditra");
                        List<Claim> reclamation = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, modelLogin.Email),
                            new Claim(ClaimTypes.Role, "Admin")
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(reclamation,
                                CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties propriete = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = modelLogin.resterConnecter
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity), propriete);

                        return View("../Admin/AccueilAdmin");
                    }
                    else
                    {
                        List<Claim> reclamation = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Email, modelLogin.Email),
                            new Claim(ClaimTypes.Role, "Client")
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(reclamation,
                                CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties propriete = new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = modelLogin.resterConnecter
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity), propriete);
                        return RedirectToAction("Index","Client");
                    }

                }

                ViewData["ErreurLogin"] = "Utilisateur non trouver";
                return View("../Auth/auth-login");
            }*/

        public IActionResult Inscription()
        {
            return View("../Auth/auth-register");
        }

        [HttpGet]
        public async Task<IActionResult> InscriptionModel(Utilisateur user)
        {
           /* if (!ModelState.IsValid)
            {
                // Imprimez les erreurs de validation dans la console
                foreach (var error in ModelState.Values)
                {
                    foreach (var key in error.Errors)
                    {
                        Console.WriteLine(key.ErrorMessage);
                    }
                }
                return View("../Auth/auth-register");
            }*/
            if (ModelState.IsValid)
            {
                Utilisateur u = new Utilisateur(_context);
                
               
                u.insert(user.nom,user.prenom, user.date_naissance, user.adresse,user.numero);
                return View("../Auth/auth-login");
            }
            return View("../Auth/auth-register");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string numero = HttpContext.Session.GetString("numero");
            if (numero != null)
            {
                HttpContext.Session.Remove("numero");
                return View("../User/ClientLogin");
            }
            return RedirectToAction("Login", "Auth");
        }
    }
}
