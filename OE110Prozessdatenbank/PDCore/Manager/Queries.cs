using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Manager
{
    static class Queries
    {
        static string QueryRaw
        {
            get { return "SELECT Workpieces.Label, Materials.Name, Workpieces.WorkPiece_ID FROM Workpieces "+
                            "INNER JOIN Materials ON Workpieces.Material_ID = Materials.Material_ID "+
                            "WHERE Workpieces.Status='raw';";
}
        }
    }


}
