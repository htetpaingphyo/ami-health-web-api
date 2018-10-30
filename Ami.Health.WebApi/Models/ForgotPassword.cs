using Ami.Health.Core.Entities;
using System;

namespace Ami.Health.WebApi.Models
{
    public class ForgotPassword
    {
        public string UserCode { get; set; }

        public string SecurityQuestion1 { get; set; }

        public string SecurityQuestion2 { get; set; }

        public string SecurityQuestion3 { get; set; }

        public string SecurityQuestion4 { get; set; }

        public string Password { get; set; }
    }
}
