using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PDCore.ToshibaImport
{
    public class IO
    {
        public static MonFile getMonFileData(string path)
        {
            List<string> m_data = new List<string>();

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    m_data.Add(reader.ReadLine());
                }
            }

            MonFile m_file = new MonFile();
            string valueString;

            valueString = m_data.Find(item => item.Contains("StartTime"));
            valueString = valueString.Remove(0, valueString.IndexOf(',') + 1);
            m_file.StartTime = Convert.ToDateTime(valueString);

            valueString = m_data.Find(item => item.Contains("StopTime"));
            valueString = valueString.Remove(0, valueString.IndexOf(',') + 1);
            m_file.StopTime = Convert.ToDateTime(valueString);

            m_file.CycleTime = m_file.StopTime - m_file.StartTime;

            int dataIndex = m_data.FindIndex(item => item.Contains("SeqNo")) + 1;

            int _stepIndex = 0;             //stepIndex 
            int _currentListIndex = -1;     //ListIndex of steplist

            for (int i = dataIndex; i < m_data.Count; i++)
            {
                //separate data from string
                string[] parameters = m_data[i].Split(',');

                //identify current step
                int currentStep = Convert.ToInt32(parameters[14]);
                if (currentStep != _stepIndex)
                {
                    _stepIndex = currentStep;
                    m_file.Steps.Add(new MonFile.ProcessStep(currentStep));
                    _currentListIndex = m_file.Steps.Count - 1;
                }

                //fill step with parameter data
                MonFile.ProcessStep.MeasuringPoint _mp = new MonFile.ProcessStep.MeasuringPoint();

                _mp.SequenceNumber = Convert.ToInt32(parameters[0]);
                _mp.TimePass = Convert.ToInt32(parameters[1]);
                _mp.LTemp1 = Convert.ToDouble(parameters[2].Replace('.', ','));
                _mp.UTemp1 = Convert.ToDouble(parameters[3].Replace('.', ','));
                _mp.PressZ = Convert.ToDouble(parameters[4].Replace('.', ','));
                _mp.PosZ = Convert.ToDouble(parameters[5].Replace('.', ','));
                _mp.LTemp2 = Convert.ToDouble(parameters[6].Replace('.', ','));
                _mp.UTemp2 = Convert.ToDouble(parameters[7].Replace('.', ','));
                _mp.PressW = Convert.ToDouble(parameters[8].Replace('.', ','));
                _mp.PosW = Convert.ToDouble(parameters[9].Replace('.', ','));
                _mp.Temp5 = Convert.ToDouble(parameters[10].Replace('.', ','));
                _mp.Temp6 = Convert.ToDouble(parameters[11].Replace('.', ','));
                _mp.Temp7 = Convert.ToDouble(parameters[12].Replace('.', ','));
                _mp.Temp8 = Convert.ToDouble(parameters[13].Replace('.', ','));
                _mp.StepNumber = Convert.ToInt32(parameters[14]);

                _mp.ChangePosZ = Convert.ToDouble(parameters[15].Replace('.', ','));
                _mp.CHangePressZ = Convert.ToDouble(parameters[16].Replace('.', ','));
                _mp.MarkerFlag = parameters[17];
                _mp.UserFlag = parameters[18];
                _mp.AlarmCode1 = parameters[19];
                _mp.AlarmCode2 = parameters[20];
                _mp.AlarmCode3 = parameters[21];
                _mp.AlarmCode4 = parameters[22];
                _mp.AlarmCode5 = parameters[23];
                _mp.SelfAlarmCode1 = parameters[24];
                _mp.SelfAlarmCode2 = parameters[25];
                _mp.SelfAlarmCode3 = parameters[26];
                _mp.SelfAlarmCode4 = parameters[27];
                _mp.SelfAlarmCode5 = parameters[28];

                m_file.Steps[_currentListIndex].MeasuringPoints.Add(_mp);
            }

            return m_file;
        }

        public static List<MonFile> getMonFileData(List<string> path)
        {
            List<MonFile> m_MonList = new List<MonFile>();

            for (int i = 0; i < path.Count; i++)
            {
                m_MonList.Add(getMonFileData(path[i]));
            }

            return m_MonList;
        }

    }
}
