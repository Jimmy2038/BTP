using BTP.Models;
using BTP.Models.Import;
using BTP.Models.Validation;
using Microsoft.AspNetCore.Mvc;

namespace BTP.Controllers
{
    public class ImportController : Controller
    {
        private readonly BtpDBContext _context;

        public ImportController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.erreur = null;
            ViewBag.succes = null;
            return View("../Import/ImportCsv");
        }

        [HttpPost]
        public async Task<IActionResult> importCsv(IFormFile file,IFormFile devisClient)
        {


            var filePath = await UploadPic.TransferFile(file, "./wwwroot/fichierImport");
            ImportCsv import = new ImportCsv();
            List<string[]> allMaison = ImportCsv.ReadCsvFile(filePath);

            List<string> erreurTravau = new List<string>();
            erreurTravau = import.verifErreurMaisonTravauxTemp(allMaison);

            var filePathDevis = await UploadPic.TransferFile(devisClient, "./wwwroot/fichierImport");
            List<string[]> allDevis = ImportCsv.ReadCsvFile(filePathDevis);
            List<string> erreurDevis = new List<string>();
            erreurDevis = import.verifErreurClientDevis(allDevis);
            erreurDevis.AddRange(erreurTravau);

            if (erreurDevis != null || erreurTravau != null)
            {
                //import maison
                import.insertMaisonTravauTemp(allMaison);
                Console.WriteLine("tafiditra");
                MaisonTravauxTemp maisonTravauxTemp = new MaisonTravauxTemp();
                maisonTravauxTemp.inserteChaqueTable();

                //imporrt finition
                import.insertDevisTempFinition(allDevis);
                ClientDevisFinition clientDevisFinition  = new ClientDevisFinition(_context);
                clientDevisFinition.inserteChaqueTable();
                ViewBag.succes = "succes";
                ViewBag.erreur = null;
                return View("../Import/ImportCsv");
            }
            /*if (erreur != null)
            {

                ViewBag.succes = "succes";
                ViewBag.erreur = null;
                return View("../Import/ImportCsv");
            }*/

            ViewBag.erreur = erreurDevis;
            ViewBag.succes = null;
            ViewBag.path = filePath;
            return View("../Import/ImportCsv");
        }

        public IActionResult payementImport()
        {
            return View("../Import/ImportPaiement");
        }
        [HttpPost]
        public async Task<IActionResult> importCsvPayement(IFormFile file)
        {


            var filePath = await UploadPic.TransferFile(file, "./wwwroot/fichierImport");
            ImportCsv import = new ImportCsv();
            List<string[]> allSeanceTemp = ImportCsv.ReadCsvFile(filePath);
            List<string> erreur = new List<string>();
            erreur = import.verifErreurPaiement(allSeanceTemp);
            if (erreur != null)
            {

                import.insertPayement_temp(allSeanceTemp);
                PaiementTemp paiement = new PaiementTemp();
                paiement.inserteChaqueTable();
                ViewBag.succes = "succes";
                ViewBag.erreur = null;
                return View("../Import/ImportPaiement");
            }
            ViewBag.erreur = erreur;
            ViewBag.succes = null;
            ViewBag.path = filePath;
            return View("../Import/ImportPaiement");
        }

    }
}
