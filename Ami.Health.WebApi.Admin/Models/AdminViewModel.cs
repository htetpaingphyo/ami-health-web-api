using Ami.Health.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ami.Health.WebApi.Admin.Models
{
    public class AdminViewModel : EntityBase
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
    }
}