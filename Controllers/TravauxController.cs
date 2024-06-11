using BTP.Models;
using BTP.Models.Util_devis;
using Microsoft.AspNetCore.Mvc;

namespace BTP.Controllers
{
    public class TravauxController : Controller
    {

        private readonly BtpDBContext _context;

        public TravauxController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult update(string id,string type_travaux,string unite, double pu)
        {
            Travaux trav = new Travaux();
            trav.update(id,type_travaux,unite,pu);
            return RedirectToAction("Index","CRUD");
        }

        public IActionResult updateFinition(string id,double pourcentage)
        {
            Finition f = new Finition();
            f.update(id, pourcentage);
            return RedirectToAction("listFinition", "CRUD");
        }
    }
}
