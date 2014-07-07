using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.FuellerImport
{
    public class FuellerProcess
    {
        private List<string> m_rawData;
        private List<MeasuringPoint> m_measuringPoints;

        private DateTime m_date;

        public FuellerProcess(string path)
        {
            m_measuringPoints = new List<MeasuringPoint>();
            m_rawData = PDCore.IO.SimpleIO.getClearText(path);
            
        }

        private void importData()
        {
            foreach (var data in m_rawData)
            {
                //separate data from string
                string[] parameters = data.Split(';');

                if (parameters[0] == "H")
                    break;
                if (parameters[0] == "R")
                {
                    MeasuringPoint mp = new MeasuringPoint();
                    mp.TimeStamp = Convert.ToDateTime(parameters[1] + " " + parameters[2]);
                    m_measuringPoints.Add(mp);

                }
            }
        }

        public class MeasuringPoint
        {
            public DateTime TimeStamp { get; set; }
        }
    }
}
