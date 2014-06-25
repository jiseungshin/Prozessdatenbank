using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.ToshibaImport
{
    public class MonFile
    {
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public TimeSpan CycleTime { get; set; }

        public List<ProcessStep> Steps { get; set; }

        public MonFile()
        {
            Steps = new List<ProcessStep>();
        }

        public class ProcessStep
        {

            public ProcessStep(int stepNumber)
            {
                stepnumber = stepNumber;
                MeasuringPoints = new List<MeasuringPoint>();
            }
            public List<MeasuringPoint> MeasuringPoints { get; set; }
            public int stepnumber { get; private set; }

            public class MeasuringPoint
            {
                public int SequenceNumber { get; set; }
                public int TimePass { get; set; }
                public double LTemp1 { get; set; }
                public double UTemp1 { get; set; }
                public double PressZ { get; set; }
                public double PosZ { get; set; }
                public double LTemp2 { get; set; }
                public double UTemp2 { get; set; }
                public double PressW { get; set; }
                public double PosW { get; set; }
                public double Temp5 { get; set; }
                public double Temp6 { get; set; }
                public double Temp7 { get; set; }
                public double Temp8 { get; set; }
                public int StepNumber { get; set; }
                public double ChangePosZ { get; set; }
                public double CHangePressZ { get; set; }
                public string MarkerFlag { get; set; }
                public string UserFlag { get; set; }
                public string AlarmCode1 { get; set; }
                public string AlarmCode2 { get; set; }
                public string AlarmCode3 { get; set; }
                public string AlarmCode4 { get; set; }
                public string AlarmCode5 { get; set; }
                public string SelfAlarmCode1 { get; set; }
                public string SelfAlarmCode2 { get; set; }
                public string SelfAlarmCode3 { get; set; }
                public string SelfAlarmCode4 { get; set; }
                public string SelfAlarmCode5 { get; set; }
            }
        }


    }






}
