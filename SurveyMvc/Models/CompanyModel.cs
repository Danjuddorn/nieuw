using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MtsSurvey.Models
{
    public class CompanyModel
    {
            public int CompanyId { get; set; }
            [StringLength(50)]
            [Required(ErrorMessage = "Dit is een vereist veld.")]
            [Display(Name ="Bedrijfs naam")]
            public String CompanyName { get; set; }     
    }
}