﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Issue : BusinessObject
    {
        public string Description { get; set; }
        public int ProjectID { get; set; }
    }
}
