using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OE110Prozessdatenbank.ViewModels
{
    public class MonFileVM : BaseViewModel
    {
        public MonFileVM(PDCore.ToshibaImport.MonFile File)
        {


            plotModelT = new OxyPlot.PlotModel();
            plotModelP = new OxyPlot.PlotModel();
            plotModelS = new OxyPlot.PlotModel();

            SetUpModel();

            LoadData(File);

        }

        private PlotModel plotModelT;
        public PlotModel PlotModelT
        {
            get { return plotModelT; }
            set { plotModelT = value; }
        }

        private PlotModel plotModelP;
        public PlotModel PlotModelP
        {
            get { return plotModelP; }
            set { plotModelP = value; }
        }

        private PlotModel plotModelS;
        public PlotModel PlotModelS
        {
            get { return plotModelS; }
            set { plotModelS = value; }
        }
        private void SetUpModel()
        {

            var dateAxis = new TimeSpanAxis(AxisPosition.Bottom, "Zeitpunkt", "mm:ss") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            var dateAxis2 = new TimeSpanAxis(AxisPosition.Bottom, "Zeitpunkt", "mm:ss") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            var dateAxis3 = new TimeSpanAxis(AxisPosition.Bottom, "Zeitpunkt", "mm:ss") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
            var valueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Temperatur" };
            var valueAxis2 = new LinearAxis(AxisPosition.Left, -3) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Presskraft" };
            var valueAxis3 = new LinearAxis(AxisPosition.Left, -3) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Position" };


            PlotModelT.Axes.Add(dateAxis);



            PlotModelT.Axes.Add(valueAxis);

            plotModelP.Axes.Add(dateAxis2);
            plotModelP.Axes.Add(valueAxis2);

            plotModelS.Axes.Add(dateAxis3);
            plotModelS.Axes.Add(valueAxis3);
        }

        public void LoadData(PDCore.ToshibaImport.MonFile File)
        {

            PlotModelT.Series.Clear();
            PlotModelP.Series.Clear();
            PlotModelS.Series.Clear();

            var m_data = File;//.da//IO.getMonFileData(path);


            double[] stepPoints = new double[m_data.Steps.Count - 1];
            for (int i = 0; i < m_data.Steps.Count - 1; i++)
            {
                stepPoints[i] = m_data.Steps[i].MeasuringPoints[m_data.Steps[i].MeasuringPoints.Count - 1].TimePass;
            }
            PlotModelT.Axes[0].ExtraGridlines = stepPoints;
            PlotModelP.Axes[0].ExtraGridlines = stepPoints;
            PlotModelS.Axes[0].ExtraGridlines = stepPoints;

            PlotModelT.Axes[0].ExtraGridlineColor = OxyColors.DeepPink;
            PlotModelT.Axes[0].ExtraGridlineStyle = LineStyle.Dash;
            PlotModelT.Axes[0].ExtraGridlineThickness = 1;

            PlotModelP.Axes[0].ExtraGridlineColor = OxyColors.DeepPink;
            PlotModelP.Axes[0].ExtraGridlineStyle = LineStyle.Dash;
            PlotModelP.Axes[0].ExtraGridlineThickness = 1;

            PlotModelS.Axes[0].ExtraGridlineColor = OxyColors.DeepPink;
            PlotModelS.Axes[0].ExtraGridlineStyle = LineStyle.Dash;
            PlotModelS.Axes[0].ExtraGridlineThickness = 1;


            LineSeries ls = new LineSeries();
            LineSeries vertical = new LineSeries();

            vertical.Color = OxyColors.DarkGray;
            vertical.LineStyle = LineStyle.LongDash;
            vertical.StrokeThickness = 1;

            foreach (var step in m_data.Steps)
            {
                ls.StrokeThickness = 0.5;
                ls.Smooth = true;
                ls.Color = OxyColors.Red;
                ls.Title = "TLower";


                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].LTemp1));
                }
            }

            PlotModelT.Series.Add(ls);

            ls = new LineSeries();
            foreach (var step in m_data.Steps)
            {
                ls.StrokeThickness = 1;
                ls.Smooth = true;
                ls.Color = OxyColors.Blue;
                ls.Title = "TUpper";

                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].UTemp1));
                }
            }

            PlotModelT.Series.Add(ls);

            ls = new LineSeries();
            foreach (var step in m_data.Steps)
            {
                ls.StrokeThickness = 1.5;
                ls.Smooth = true;
                ls.Color = OxyColors.Green;


                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].PressZ));
                }
            }

            PlotModelP.Series.Add(ls);

            ls = new LineSeries();
            foreach (var step in m_data.Steps)
            {
                ls.StrokeThickness = 1.5;
                ls.Smooth = true;
                ls.Color = OxyColors.DarkTurquoise;


                for (int i = 0; i < step.MeasuringPoints.Count; i++)
                {
                    ls.Points.Add(new DataPoint(step.MeasuringPoints[i].TimePass, step.MeasuringPoints[i].PosZ));
                }
            }

            PlotModelS.Series.Add(ls);

        }

       


    }
}
