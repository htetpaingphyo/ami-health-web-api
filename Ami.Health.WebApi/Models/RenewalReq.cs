using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ami.Health.WebApi.Models
{
    public class RenewalReq
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}
