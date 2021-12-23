using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkforceManagement.Data.Entities.Common;

namespace WorkforceManagement.Data.Entities
{
    public class User : IdentityUser, IBaseDeletableModel
    {
        public User()
        {
            Teams = new HashSet<Team>();
            TimeOffRequests = new HashSet<TimeOffRequest>();
            Approvals = new HashSet<Approval>();
        }

        public TimeSpan? OutOfOffice { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [Range(0, int.MaxValue)]
        public int PaidDaysOff { get; set; }

        [Range(0, 40)]
        public int SickDaysOff { get; set; }

        [Range(0, 90)]
        public int UnpaidDaysOff { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

        public virtual ICollection<TimeOffRequest> TimeOffRequests { get; set; }

        public virtual ICollection<Approval> Approvals { get; set; }
    }
}
