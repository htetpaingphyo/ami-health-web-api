using System;

namespace Ami.Health.WebApi.Models
{
    public class ClaimsViewModel
    {
        public string PolicyNo { get; set; }

        public string InsuredName { get; set; }

        public string NIC { get; set; }

        public string Phone { get; set; }

        public string CauseOfLoss { get; set; }

        public string NameOfTreatment { get; set; }

        public DateTime FromHospitalization { get; set; }

        public DateTime? ToHospitalization { get; set; }

        public decimal? ReinvestmentAmt { get; set; }
    }
}
