using Nager.Date;
using Nager.Date.Model;
using System;
using System.Collections.Generic;

namespace WorkforceManagement.Services.Interfaces
{
    public interface IDateService
    {
        public int CheckforWorkingDays(DateTime startDate, DateTime endDate, CountryCode countryCode);

    }
}
