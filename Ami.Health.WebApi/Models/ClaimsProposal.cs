using Ami.Health.Core.Entities;
using System;

namespace Ami.Health.WebApi.Models
{
    public class ClaimsProposal : EntityBase
    {
        public string PolicyNo { get; set; }

        public string InsuredName { get; set; }

        public string NIC { get; set; }

        public string Phone { get; set; }

        public DateTime InitimatedDate { get; set; }

        public string CauseOfLoss { get; set; }

        public string NameOfTreatment { get; set; }

        public DateTime FromHospitalization { get; set; }

        public DateTime? ToHospitalization { get; set; }

        public ClaimStatus ClaimStatus { get; set; }

        public decimal? ReinvestmentAmt { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
