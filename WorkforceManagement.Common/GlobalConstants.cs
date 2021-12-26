namespace WorkforceManagement.Common
{
    public class GlobalConstants
    {
        // Roles
        public const string AdministratorRoleName = "Administrator";

        // Administrator credentials
        public const string AdminUsername = "admin";
        public const string AdminPassword = "adminpass";
        public const string AdminEmail = "admin@demo.com";

        // Mail settings
        public const string Host = "demo.com";
        public const int  Port = 25;

        //Team member actions
        public const string AddMemberToTeam = "AddMember";
        public const string RemoveMemberFromTeam = "RemoveMember";

        //Days vacation
        public const int PaidOffDays = 20;
        public const int SickDays = 40;
        public const int UnpaidDays = 90;
    }
}