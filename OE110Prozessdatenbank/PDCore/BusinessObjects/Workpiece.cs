using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class Workpiece : BusinessObject
    {
        public Workpiece()
        {
            Material = new Material();
            Quality = new WorkpieceQuality();
            Label = "";
            isActive = true;
            Status = "raw";
            
        }
        public string Label { get; set; }
        public Material Material { get; set; }
        public string Geometry { get; set; }
        public string KindOfProbe { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool isOneWay { get; set; }
        public int CurrentRefereneNumber { get; set; }
        public BusinessObjects.WorkpieceQuality Quality { get; set; }
        public string Status { get; set; }
        public bool isActive { get; set; }

        public Workpiece clone()
        {

            Workpiece wp = new Workpiece();
            wp.ID = this.ID;
            wp.Label = this.Label;
            wp.Material = this.Material;
            wp.CurrentRefereneNumber = this.CurrentRefereneNumber;

            return wp;
        }

    }
}
