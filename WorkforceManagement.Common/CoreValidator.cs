using System;

namespace WorkforceManagement.Common
{
    public class CoreValidator
    {
        public static bool IsValid(int id)
        {
            if (id <= 0 || id > int.MaxValue)
            {
                return false;
            }

            return true;
        
        }

        public static bool isValid(string id)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                return false;
            }

            return true;
        }
    }
}
