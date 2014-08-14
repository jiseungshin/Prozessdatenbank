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

            Reference = new Reference();
            
        }
        public Reference Reference { get; set; }
        public string Label { get; set; }
        public Material Material { get; set; }
        public string Geometry { get; set; }
        public string KindOfProbe { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public bool isOneWay { get; set; }
        public int CurrentReferenceNumber { get; set; }
        public BusinessObjects.WorkpieceQuality Quality { get; set; }
        public string Status { get; set; }
        public bool isActive { get; set; }
        public int? InitiatorID { get; set; }

        public string Remark { get; set; }

        public Workpiece clone()
        {

            Workpiece wp = new Workpiece();
            wp.ID = this.ID;
            wp.Label = this.Label;
            wp.Material = this.Material;
            wp.CurrentReferenceNumber = this.CurrentReferenceNumber;
            wp.Geometry = this.Geometry;
            wp.PurchaseDate = this.PurchaseDate;
            wp.BatchNumber = this.BatchNumber;
            wp.InitiatorID = this.InitiatorID;
            wp.isActive = this.isActive;
            wp.isOneWay = this.isOneWay;
            wp.KindOfProbe = this.KindOfProbe;

            return wp;
        }

    }
}
