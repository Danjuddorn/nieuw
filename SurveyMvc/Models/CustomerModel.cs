using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Vereist")]
        [Display(Name = "Bedrijf")]
        public int CompanyId { get; set; }

        [Display(Name = "Bedrijfs naam")]
        public String CompanyName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vereist")]
        [Display(Name = "Naam klant")]
        public String CustomerName { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "vereist")]
        public String Email { get; set; }
        public int passcode { get; set; }
    }
}