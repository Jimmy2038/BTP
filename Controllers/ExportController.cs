using BTP.Models.Util_devis;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace BTP.Controllers
{
    public class ExportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult listTravauPdf(string id_devis)
        {
            DetailDevis detail = new DetailDevis();
            List<DetailDevis> detailDevis = new List<DetailDevis>();
            detailDevis = detail.getAllIdTravauxByDevis(id_devis);

            Devis d = new Devis();
            List<Paiement> allPaiement = new List<Paiement>();
            allPaiement = d.getPayementByDevis(id_devis);
            double prixtotalMaison = 0;
            for(int i=0;i< detailDevis.Count; i++)
            {
                prixtotalMaison = detailDevis[i].prixTotal + prixtotalMaison;
               Console.WriteLine(prixtotalMaison);
            }

            double totalPay = d.getSommePayementByDevis(id_devis);

            DetailDevisExportPDF detailPdf = new DetailDevisExportPDF();
            detailPdf.allPaiement = allPaiement;
            detailPdf.detailDevis = detailDevis;
            detailPdf.totalpaiement = totalPay;
            detailPdf.totaldevis = prixtotalMaison;

            ViewBag.total = totalPay;
            return new ViewAsPdf("../Export/DetailDevisPDF", detailPdf );
        }
    }
}
