using System;

namespace Ami.Health.WebApi.Models
{
    public class PolicyRenewal
    {
        public string POLICY_NO { get; set; }

        public string CUSTOMER_NAME { get; set; }

        public string NIC_NO { get; set; }

        public DateTime POLICY_PERIOD { get; set; }

        public decimal SUM_INSURED { get; set; }

        public decimal TOTAL_PREMIUM { get; set; }

        public string PAYMENT_TYPE { get; set; }
    }
}