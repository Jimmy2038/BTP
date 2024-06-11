using BTP.Models;
using BTP.Models.Util_devis;
using Microsoft.AspNetCore.Mvc;

namespace BTP.Controllers
{
   
    public class CRUDController : Controller
    {
        private readonly BtpDBContext _context;

        public CRUDController(BtpDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Travaux> trav = _context.travaux.ToList();
            ViewBag.allTrav = trav;
            return View("../Admin/Travaux",trav);
        }

        public IActionResult listFinition()
        {
            List<Finition> finitions = new List<Finition>();
            Finition f = new Finition();
            finitions = f.getAll();
            ViewBag.finition = finitions;
            return View("../Admin/Finition");
        }
    }
}
