using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class User : BusinessObject
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isActive { get; set; }
    }
}
