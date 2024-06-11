using BTP.Models.Util_devis;
using Microsoft.AspNetCore.Mvc;

namespace BTP.Controllers
{
    public class PaiementController : Controller
    {
        public IActionResult Index(string id_devis)
        {
            ViewBag.idDevis = id_devis;
            return View("../User/Paiement");
        }

        public IActionResult validerPaiement(string idD,string reff, double montant,string date_paiement)
        {
            Paiement p = new Paiement();
            p.insert(reff,idD,montant,date_paiement);

            return RedirectToAction("ListDevis", "Client");
        }
    }
}
