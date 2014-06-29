using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.BusinessObjects;

namespace PDCore.Manager
{
    static public class UserManager
    {
        public static User CurrentUser { get; set; }
        public static string CurrentPCUser
        {
            get { return getcurrentUser(); }
            set { }
        }

        private static string getcurrentUser()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }
    }
}
