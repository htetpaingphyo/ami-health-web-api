using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ami.Health.WebApi.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(10)]
        public string UserCode { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
