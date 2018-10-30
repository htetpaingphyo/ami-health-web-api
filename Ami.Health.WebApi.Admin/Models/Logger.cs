using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ami.Health.WebApi.Admin.Models
{
    public class Logger
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AdminId { get; set; }

        [Required]
        [StringLength(500)]
        public string LogInfo { get; set; }

        [Required]
        [Column(TypeName = "DateTime2")]
        public DateTime LoggedDate { get; set; }
    }
}
