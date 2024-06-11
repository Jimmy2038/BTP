using System.ComponentModel.DataAnnotations;

namespace BTP.Models.Client
{
    public class ValidationClietn
    {

        [Required(ErrorMessage = "Cette champ est requis")]
        [RegularExpression(@"^0[23][2348]\d{7}$", ErrorMessage = "Le numéro ne correspond pas au critère necessaire")]
        public string numero { get; set; }

        public bool resterConnecter { get; set; }
    }
}
