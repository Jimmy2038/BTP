using BTP.Models;
using BTP.Models.truncate;
using BTP.Models.Util_devis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BTP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly BtpDBContext _context;

        public AdminController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Devis> devis = new List<Devis>();
            Devis de = new Devis();
            devis = de.getAll();
            List<Maison> maisons = new List<Maison>();
            List<double> montantTotal = new List<double>();
            List<double> montantPayer = new List<double>();
            List<double> pourcentagePayer = new List<double>();

            for (int i = 0; i < devis.Count; i++)
            {
                double pourcentage = de.getPourcentagePayerByDevis(devis[i].id);
                pourcentagePayer.Add(pourcentage);

                double payer = de.getMontantPayerByDevis(devis[i].id);
                montantPayer.Add(payer);
                double montantTota = de.getMontantTotalByDevis(devis[i].id);
                Console.WriteLine("Montant Total: "+ montantTota);
                montantTotal.Add(montantTota);
                Maison m = new Maison();
                m = m.getMaisonsByDevis(devis[i].id);
                maisons.Add(m);
            }

            ViewBag.allDevise = devis;
            ViewBag.maison = maisons;
            ViewBag.montantTotal = montantTotal;
            ViewBag.Montantpayer = montantPayer;
            ViewBag.Pourcentagepayer = pourcentagePayer;
            return View("../Admin/AccueilAdmin");
        }

        public IActionResult truncate()
        {
            Truncate truncate = new Truncate("Server=localhost;Port=5432;Database=btp;Username=postgres;Password=root");
            truncate.TruncateTablesAndResetSequences();
            return RedirectToAction("Index","Admin");
        }

        public IActionResult dashboard(string? annee)
        {
            Devis devis = new Devis();
            List<string> annees = devis.getAnneeInDevis();
            
            string taona = (annee ?? "2021");

            ViewBag.montantDevisTotal = devis.montantTotalDevis();
            List<Devis> devis1 = new List<Devis>();
            devis1 = devis.getListMontantDevisParMois(taona);
            double totalPayer = devis.getTotalPayement();
            //devis.insert("MAIS_00002", "FIN_00001", "User_00001", "31/03/2001", "31/03/2001");
            List<double> montantTotalParMoisList = new List<double>();
            for (int i = 0; i < devis1.Count; i++)
            {
                Console.WriteLine("Moi" + i + ": " + devis1[i].montantTotalParMoi);
                montantTotalParMoisList.Add((double)devis1[i].montantTotalParMoi); 

            }
            ViewBag.totalPayer = totalPayer;
            ViewBag.montantTotalParMoisList = montantTotalParMoisList;
            ViewBag.annee = annees;
            return View("../Admin/Dashboard");
        }

        public IActionResult detailDevis(string id_devis)
        {
            DetailDevis detail = new DetailDevis();
            List<DetailDevis> detailDevis = new List<DetailDevis>();
            detailDevis = detail.getAllIdTravauxByDevis(id_devis);

            return View("../Admin/DeatailDevis", detailDevis);
        }
    }
}
