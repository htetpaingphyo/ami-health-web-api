using System;
using System.Collections.Generic;
using System.Text;

namespace Ami.Health.Core.Entities
{
    public enum AccountStatus
    {
        ACTIVE,
        INACTIVE,
        LOCKED
    }

    public enum ClaimStatus
    {
        Pending = 1,
        InProgress = 2,
        Finished = 3,
        Rejected = 4,
        CancelledByCustomer = 5
    }
}
