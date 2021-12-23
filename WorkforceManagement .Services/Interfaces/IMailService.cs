using WorkforceManagement.Data.Entities;

namespace WorkforceManagement.Services.Interfaces
{
    public interface IMailService
    {
        bool SendEmail(User sender, string toAddress, TimeOffRequest requestToBeSend);
    }
}