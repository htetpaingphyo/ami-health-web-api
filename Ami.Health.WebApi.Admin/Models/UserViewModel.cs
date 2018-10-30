using Ami.Health.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ami.Health.WebApi.Admin.Models
{
    public class UserViewModel : EntityBase
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Policy No.")]
        public string PolicyNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string UserCode { get; set; }

        [StringLength(30)]
        [Display(Name = "NRC No.")]
        public string NRC { get; set; }
        
        [Display(Name = "Date Of Birth")]      
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime DOB { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
                        
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [StringLength(400)]
        public string Address { get; set; }
    }
}