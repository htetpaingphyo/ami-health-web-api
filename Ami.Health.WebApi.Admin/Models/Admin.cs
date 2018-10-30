using Ami.Health.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ami.Health.WebApi.Admin.Models
{
    public class Admin : EntityBase
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [StringLength(128)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? UpdatedDate { get; set; }
    }
}