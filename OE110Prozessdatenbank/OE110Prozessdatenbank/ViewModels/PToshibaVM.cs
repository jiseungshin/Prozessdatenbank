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

namespace OE110Prozessdatenbank.ViewModels
{
    public class PToshibaVM : BaseViewModel
    {
        public PToshiba m_process;
        public PlotModel plotModelT;
        public PlotModel plotModelP;

        public PToshibaVM(PToshiba process)
        {
            m_process = process;
            plotModelT = new OxyPlot.PlotModel();
            plotModelP = new OxyPlot.PlotModel();

            WP_UpperControl = new Controls.CQuality(process.Workpieces.Find(item => item.ID == m_process.UpperWorkpiece));
            PV_Control = new Controls.CPVControl();

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

        }

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
        public Controls.CPVControl PV_Control { get; set; }

        public double? P1
        { get { return m_process.InputData.P1; } set { m_process.InputData.P1 = value; } }
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
