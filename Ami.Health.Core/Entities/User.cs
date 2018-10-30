using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ami.Health.Core.Entities
{
    public class User : EntityBase
    {
        [Required]
        [StringLength(30)]
        public string PolicyNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(30)]
        [Display(Name = "NRC No.")]
        public string NRC { get; set; }

        [Column(TypeName = "DATE")]
        [Display(Name = "Date Of Birth")]
        public DateTime DOB { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string UserCode { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }

        [Required]
        [StringLength(10)]
        public string PasswordSalt { get; set; }
                
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string SecurityQuestion1 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion2 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion3 { get; set; }

        [StringLength(50)]
        public string SecurityQuestion4 { get; set; }

        [Required]
        [StringLength(400)]
        public string Address { get; set; }

        [Required]
        public bool IsFirstTimeLogin { get; set; }

        [Required]
        public AccountStatus AccountStatus { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? UpdatedDate { get; set; }
    }
}