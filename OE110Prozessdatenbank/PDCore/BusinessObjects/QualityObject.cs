using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.BusinessObjects
{
    public class QualityObject
    {

        private ProcessQuality m_pQ;
        private List<WorkpieceQuality> m_wQ;

        public QualityObject()
        {
            m_wQ = new List<WorkpieceQuality>();
            m_pQ = new ProcessQuality();
        }

        public ProcessQuality PQuality
        { get { return m_pQ; } set { m_pQ = value; } }

        public List<WorkpieceQuality> WorkpieceQuelities
        {
            get { return m_wQ; }
            set { m_wQ = value; }
        }

        public class ProcessQuality
        {
            public int ID { get; set; }
            public bool GlassSratches { get; set; }
            public bool GlassPeeling { get; set; }
            public bool GlassBreakage { get; set; }
            public int OverallResult { get; set; }

        }

        public class WorkpieceQuality
        {
            public int ID { get; set; }
            public int MoldScratches { get; set; }
            public int Corrosion { get; set; }
            public int GlassAdherence { get; set; }
        }
    }
}
