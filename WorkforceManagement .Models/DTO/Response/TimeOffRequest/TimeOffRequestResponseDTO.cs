using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkforceManagement.Models.DTO.Response
{
    public class TimeOffRequestResponseDTO
    {
        public int TimeOffRequestId { get; set; }

        public string Requester { get; set; }

        public string RequestType { get; set; }

        public string Reason { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string CreatedOn { get; set; }

        public string RequestStatus { get; set; }

        public List<string> Approvals { get; set; }
    }
}
