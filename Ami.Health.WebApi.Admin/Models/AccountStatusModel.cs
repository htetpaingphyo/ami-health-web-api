using Ami.Health.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ami.Health.WebApi.Admin.Models
{
    public class AccountStatusModel : EntityBase
    {
        [Display(Name = "Policy No.")]
        public string PolicyNo { get; set; }

        public string Name { get; set; }

        public string UserCode { get; set; }

        [Display(Name = "NRC No.")]
        public string NRC { get; set; }

        [Display(Name = "D.O.B")]
        public DateTime DOB { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Status")]
        public AccountStatus AccountStatus { get; set; }
    }
}
