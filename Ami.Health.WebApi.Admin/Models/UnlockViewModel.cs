using Ami.Health.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ami.Health.WebApi.Admin.Models
{
    public class UnlockViewModel
    {
        [Required]
        [StringLength(10)]
        public string UserCode { get; set; }

        [StringLength(30)]
        [Display(Name = "NRC No.")]
        public string NRC { get; set; }

        [Column(TypeName = "DATE")]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [StringLength(50)]
        public string SecurityQuestion1 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion2 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion3 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion4 { get; set; }

        [Required]
        public AccountStatus AccountStatus { get; set; }
    }
}
