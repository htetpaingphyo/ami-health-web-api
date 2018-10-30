using System;

namespace Ami.Health.WebApi.Models
{
    public class Policy
    {
        public string POLICY_NO { get; set; }

        public string USERNAME { get; set; }

        public string ADDRESS { get; set; }

        public string NRC { get; set; }

        public DateTime? PERIOD_FROM { get; set; }

        public DateTime? PERIOD_TO { get; set; }

        public string INS_PLAN { get; set; }

        public string INS_COLOR_CODE { get; set; }

        public string PAYMENT_METHOD { get; set; }

        public decimal? TOTAL_PREMIUM { get; set; }
    }
}