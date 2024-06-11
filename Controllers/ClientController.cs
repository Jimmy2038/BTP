using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using BTP.Models;
using BTP.Models.Util_devis;

namespace BTP.Controllers
{
    //[Authorize(Roles = "Client")]
    [Authorize]
    public class ClientController : Controller
    {
        private readonly BtpDBContext _context;

        public ClientController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Maison> maisons = _context.maisons.ToList();
            List<double> prixMaison = new List<double>();
            for(int i = 0; i<maisons.Count; i++)
            {
                Devis devis = new Devis();
                double prix = devis.getPrixTotalMaison(maisons[i].id);
                prixMaison.Add(prix);
            }
            ViewBag.prixMaison = prixMaison;
            ViewBag.allMaison = maisons;
            List<Finition> finitions = new List<Finition>();
            Finition f = new Finition();
            finitions = f.getAll();
            ViewBag.finition = finitions;
            return View("../User/AccueilClient");
        }

        public IActionResult ListDevis()
        {
            string iduser = HttpContext.Session.GetString("idUser");
            List<Devis> devis = new List<Devis>();
            Devis dev = new Devis();
            devis = dev.GetDevisByIdClient(iduser);
            List<Maison> maisons = new List<Maison>();
            for(int i=0; i< devis.Count; i++)
            {
                Maison m = new Maison();
                m = m.getMaisonsByDevis(devis[i].id);
                maisons.Add(m);
            }
            ViewBag.allDevise = devis;
            ViewBag.maison = maisons;
            
            return View("../User/ListeDevis");
        }

        public IActionResult insertDevis(string radioValueMaison,string typeFinition,string date_debut,string lieu)
        {
            DateTime dateActuelle = DateTime.Now;

            // Convertir la date en chaîne de caractères
            string dateTimeActuelleString = dateActuelle.ToString("yyyy-MM-dd HH:mm:ss");
            string datedevis = dateActuelle.ToShortDateString();
            // Convertir la chaîne en objet DateTime
            DateTime date = DateTime.ParseExact(date_debut, "yyyy-MM-dd", null);

            // Formater la date en chaîne dans le format 'dd/MM/yyyy'
            date_debut = date.ToString("dd/MM/yyyy");

            string id_client = HttpContext.Session.GetString("idUser");
            Devis devis = new Devis();
            string refe= "ref0021301";
            devis.insert(refe, radioValueMaison, typeFinition, id_client, datedevis, date_debut,lieu);
            DetailMaison detail = new DetailMaison(_context);
            detail.insert();
            return RedirectToAction("ListDevis","Client");
        }

        public IActionResult detailDevis(string id_devis)
        {
            DetailDevis detail = new DetailDevis();
            List<DetailDevis> detailDevis = new List<DetailDevis>();
            detailDevis = detail.getAllIdTravauxByDevis(id_devis);

            return View("../User/DetailDevis",detailDevis);
        }
    }
}
