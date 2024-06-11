using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BTP.Views.User
{
    public class ClientLoginModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientLoginModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        internal bool resterConnecter;

        [Required(ErrorMessage = "Cette champ est requis")]
        [RegularExpression(@"^0[23][2348]\d{7}$", ErrorMessage = "Le numéro ne correspond pas au critère necessaire")]
        public string numero { get; set; }

        public void OnGet()
        {
        }
    }
}
