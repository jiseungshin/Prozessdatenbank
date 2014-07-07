using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using PDCore.BusinessObjects;
using PDCore.Manager;
using OE110Prozessdatenbank.Commands;

namespace OE110Prozessdatenbank.ViewModels
{
    public class PToshibaVM : BaseViewModel
    {
        public PToshiba m_process;
        public PlotModel plotModelT;
        public PlotModel plotModelP;

        public ViewModels.MonFileVM m_vm;

        public PToshibaVM(PToshiba process)
        {
            m_process = process;
            plotModelT = new OxyPlot.PlotModel();
            plotModelP = new OxyPlot.PlotModel();

            //Inint QualityControls
            if (m_process.UpperWorkpiece!=null)
                WP_UpperControl = new Controls.CQuality(process.Workpieces.Find(item => item.ID == m_process.UpperWorkpiece));
            if (m_process.LowerWorkpiece != null)
                WP_LowerControl = new Controls.CQuality(process.Workpieces.Find(item => item.ID == m_process.LowerWorkpiece));
            PV_Control = new Controls.CPVControl(m_process);
            ProcessQualityControl = new Controls.CProcessQuality(m_process);

            #region Temp
            LineSeries ls = new LineSeries();

            foreach (var step in process.File.Steps)
            {
                ls.StrokeThickness = 0.8;
                ls.Smooth = true;
                ls.Color = OxyColors.Red;


                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].LTemp1));
                }
            }

            Temperature.Series.Add(ls);

            plotModelT.PlotMargins = new OxyThickness(3, 3, 3, 3);
            plotModelT.Padding = new OxyThickness(3, 3, 3, 3);
            plotModelT.TitleFontSize = 0;
            plotModelT.SubtitleFontSize = 0;
            plotModelT.TitlePadding = 0;
            plotModelT.Axes.Clear();

            plotModelT.Axes.Add(new InvisibleAxis { Position = AxisPosition.Bottom });
            plotModelT.Axes.Add(new InvisibleAxis { Position = AxisPosition.Left });

            #endregion

            #region force
            ls = new LineSeries();
            foreach (var step in process.File.Steps)
            {
                ls.StrokeThickness = 0.8;
                ls.Smooth = true;
                ls.Color = OxyColors.Blue;

                for (int i = 0; i < step.MeasuringPoints.Count; i += 1)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].PressZ));
                }
            }

            Force.Series.Add(ls);

            plotModelP.PlotMargins = new OxyThickness(3, 3, 3, 3);
            plotModelP.Padding = new OxyThickness(3, 3, 3, 3);
            plotModelP.TitleFontSize = 0;
            plotModelP.SubtitleFontSize = 0;
            plotModelP.TitlePadding = 0;
            plotModelP.Axes.Clear();

            plotModelP.Axes.Add(new InvisibleAxis { Position = AxisPosition.Bottom });
            plotModelP.Axes.Add(new InvisibleAxis { Position = AxisPosition.Left });

            #endregion

            m_vm = new MonFileVM(process.File);

        }

        public PToshibaVM(int PID)
        {
            SaveProcess = new RelayCommand(Save, CanSave);

            m_process = ProcessManager.Instance.getProcess(PID, 34) as PToshiba;
            
            //try getting mon-file from local directory
            try
            {
                m_process.File = PDCore.ToshibaImport.IO.getMonFileData(@"Data\Toshiba\" + PID + ".mon");
            }
            catch
            {
                m_process.File = new PDCore.ToshibaImport.MonFile();
            }

            plotModelT = new OxyPlot.PlotModel();
            plotModelP = new OxyPlot.PlotModel();

            //Inint QualityControls
            if (m_process.UpperWorkpiece != null)
                WP_UpperControl = new Controls.CQuality(ObjectManager.Instance.Workpieces.Find(item => item.ID == m_process.UpperWorkpiece));
            if (m_process.LowerWorkpiece != null)
                WP_LowerControl = new Controls.CQuality(ObjectManager.Instance.Workpieces.Find(item => item.ID == m_process.LowerWorkpiece));
            PV_Control = new Controls.CPVControl(m_process);
            ProcessQualityControl = new Controls.CProcessQuality(m_process);

            #region Temp
            LineSeries ls = new LineSeries();

            foreach (var step in m_process.File.Steps)
            {
                ls.StrokeThickness = 0.8;
                ls.Smooth = true;
                ls.Color = OxyColors.Red;


                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].LTemp1));
                }
            }

            Temperature.Series.Add(ls);

            plotModelT.PlotMargins = new OxyThickness(3, 3, 3, 3);
            plotModelT.Padding = new OxyThickness(3, 3, 3, 3);
            plotModelT.TitleFontSize = 0;
            plotModelT.SubtitleFontSize = 0;
            plotModelT.TitlePadding = 0;
            plotModelT.Axes.Clear();

            plotModelT.Axes.Add(new InvisibleAxis { Position = AxisPosition.Bottom });
            plotModelT.Axes.Add(new InvisibleAxis { Position = AxisPosition.Left });

            #endregion

            #region force
            ls = new LineSeries();
            foreach (var step in m_process.File.Steps)
            {
                ls.StrokeThickness = 0.8;
                ls.Smooth = true;
                ls.Color = OxyColors.Blue;

                for (int i = 0; i < step.MeasuringPoints.Count; i += 1)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].PressZ));
                }
            }

            Force.Series.Add(ls);

            plotModelP.PlotMargins = new OxyThickness(3, 3, 3, 3);
            plotModelP.Padding = new OxyThickness(3, 3, 3, 3);
            plotModelP.TitleFontSize = 0;
            plotModelP.SubtitleFontSize = 0;
            plotModelP.TitlePadding = 0;
            plotModelP.Axes.Clear();

            plotModelP.Axes.Add(new InvisibleAxis { Position = AxisPosition.Bottom });
            plotModelP.Axes.Add(new InvisibleAxis { Position = AxisPosition.Left });

            #endregion

            m_vm = new MonFileVM(m_process.File);

        }


        public PToshiba Process
        { get { return m_process; } }
        public ViewModels.MonFileVM MonVM
        { get { return m_vm; } }

        public int? ProjectID
        { get { return m_process.ProjectID; } set { m_process.ProjectID = value; } }

        public int? IssueID
        { get { return m_process.IssueID; } set { m_process.IssueID = value; } }

        public DateTime Date
        { get { return m_process.Date; } }

        public PlotModel Temperature
        { get { return plotModelT; } }

        public PlotModel Force
        { get { return plotModelP; } }

        public string LensName
        { get { return m_process.GlassName; } set { m_process.GlassName = value; } }

        public Glass Glass
        {
            get
            {
                try
                {
                    return ObjectManager.Instance.Glasses.Single(item => item.ID == m_process.GlassID) as Glass;
                }
                catch { return null; }
            }

            set
            {
                //   m_process.GlassID = value.ID;
            }
        }

        public Controls.CQuality WP_UpperControl { get; set; }
        public Controls.CQuality WP_LowerControl { get; set; }
        public Controls.CPVControl PV_Control { get; set; }
        public Controls.CProcessQuality ProcessQualityControl { get; set; }

        public double? P1
        { get { return m_process.InputData.P1; } set { m_process.InputData.P1 = value; } }

        public double? P2
        { get { return m_process.InputData.P2; } set { m_process.InputData.P2 = value; } }

        public double? P3
        { get { return m_process.InputData.P3; } set { m_process.InputData.P3 = value; } }

        public double? PT1
        { get { return m_process.InputData.PT1; } set { m_process.InputData.PT1 = value; } }

        public double? Tv
        { get { return m_process.InputData.Tv; } set { m_process.InputData.Tv = value; } }

        public double? Tvu
        { get { return m_process.InputData.Tvu; } set { m_process.InputData.Tvu = value; } }

        public double? T1
        { get { return m_process.InputData.T1; } set { m_process.InputData.T1 = value; } }

        public double? T1u
        { get { return m_process.InputData.PT1; } set { m_process.InputData.T1u = value; } }

        public double? T2
        { get { return m_process.InputData.T2; } set { m_process.InputData.T2 = value; } }

        public double? T3
        { get { return m_process.InputData.T3; } set { m_process.InputData.T3 = value; } }

        public double? T4
        { get { return m_process.InputData.T4; } set { m_process.InputData.T4 = value; } }

        public double? T5
        { get { return m_process.InputData.T5; } set { m_process.InputData.T5 = value; } }

        public double? Gv
        { get { return m_process.InputData.Gv; } set { m_process.InputData.Gv = value; } }

        public double? G1
        { get { return m_process.InputData.G1; } set { m_process.InputData.G1 = value; } }

        public double? G2
        { get { return m_process.InputData.G2; } set { m_process.InputData.G2 = value; } }

        public double? ST1
        { get { return m_process.InputData.ST1; } set { m_process.InputData.ST1 = value; } }

        public double? ST2
        { get { return m_process.InputData.ST2; } set { m_process.InputData.ST2 = value; } }

        public double? V1
        { get { return m_process.InputData.V1; } set { m_process.InputData.V1 = value; } }

        public double? V2
        { get { return m_process.InputData.V2; } set { m_process.InputData.V2 = value; } }

        public double? V3
        { get { return m_process.InputData.V3; } set { m_process.InputData.V3 = value; } }

        public double? Z1
        { get { return m_process.InputData.Z1; } set { m_process.InputData.Z1 = value; } }

        public double? Z2
        { get { return m_process.InputData.Z2; } set { m_process.InputData.Z2 = value; } }

        #region Command functions

        public RelayCommand SaveProcess { get; set; }
        public void Save()
        {
            
                ProcessManager.Instance.saveProcess(m_process, true);
            
            

            Updater.Instance.forceUpdate();
        }

        public bool CanSave()
        {
            if (m_process.UserID != -1 && m_process.ProjectID != -1 && m_process.IssueID != -1)
                return true;
            else
                return false;
        }

        #endregion


    }

    public class InvisibleAxis : Axis
    {
        public override bool IsXyAxis()
        {
            return true;
        }

        public override OxySize Measure(IRenderContext rc) { return new OxySize(0, 0); }
    }
}
