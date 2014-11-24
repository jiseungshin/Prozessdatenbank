using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDCore.Processes;

namespace PDCore.ToshibaImport
{
    public static class Analytics
    {
        private static int PressRound = 1;

        public static PToshiba AnalyseProcess(MonFile File)
        {
            AnalyseResults result;
            PToshiba process = new PToshiba();

            process.Date = File.StartTime;

            foreach (var step in File.Steps)
            {
                
                switch(step.stepnumber)
                {
                    case 1:
                        //result = analyse_123(step);
                        process.InputData.V1 = null;
                        break;
                    case 2:
                        result = analyse_123(step);

                        process.MachinaData.T_start_lower = result.TminLower;
                        process.MachinaData.T_start_upper = result.TminUpper;

                        process.MachinaData.Tv_lower = result.TmaxLower;
                        process.MachinaData.Tv_upper = result.TmaxUpper;

                        process.MachinaData.Gv_lower = result.TRateLower;
                        process.MachinaData.Gv_upper = result.TRateUpper;

                        process.MachinaData.Gv_lower_quali = result.TRateLowerQuality;
                        process.MachinaData.Gv_upper_quali = result.TRateUpperQuality;

                        process.InputData.Gv = Math.Round((((result.TUpper_Last + result.TLower_Last) / 2) - ((result.TUpper_First + result.TLower_First) / 2)) / result.Duration,2);
                        process.InputData.Tv = ((result.TUpper_Last + result.TLower_Last) / 2);
                        process.InputData.Z1 = result.Posmax;

                        break;
                    case 3:
                        result = analyse_123(step);
                        process.MachinaData.Tvu_lower = result.TminLower;
                        process.MachinaData.Tvu_upper = result.TminUpper;

                        process.MachinaData.T1u_lower = result.TmaxLower;
                        process.MachinaData.T1u_upper = result.TmaxUpper;

                        process.MachinaData.G1_lower = result.TRateLower;
                        process.MachinaData.G1_upper = result.TRateUpper;

                        process.MachinaData.G1_lower_quali = result.TRateLowerQuality;
                        process.MachinaData.G1_upper_quali = result.TRateUpperQuality;

                        process.InputData.G1 =  Math.Round((((result.TUpper_Last + result.TLower_Last) / 2) - ((result.TUpper_First + result.TLower_First) / 2)) / result.Duration,2);
                        process.InputData.Tvu = ((result.TUpper_First + result.TLower_First) / 2);
                        process.InputData.T1u = ((result.TUpper_Last + result.TLower_Last) / 2);

                        break;
                    case 4:
                        result = analyse_4567(step);

                        process.MachinaData.T1_lower = result.TmaxLower;
                        process.MachinaData.T1_upper = result.TmaxUpper;
                        process.MachinaData.ST1 = result.Duration;

                        process.InputData.T1 = ((result.TUpper_Last + result.TLower_Last) / 2);
                        process.InputData.ST1 = result.Duration;

                        break;
                    case 5:
                        result = analyse_4567(step);

                        process.MachinaData.T_press_av = (result.TAverageUpper + result.TAverageLower) / 2;
                        process.MachinaData.T_press_G = (result.TRateUpper + result.TRateLower) / 2;
                        process.MachinaData.T_press_G_quali = (result.TRateUpperQuality + result.TRateLowerQuality) / 2;

                        process.MachinaData.P1 = result.PAverage;
                        process.MachinaData.PT1 = result.PressTime;
                        process.MachinaData.ST2 = result.Duration;

                        process.InputData.P1 = Math.Round(result.PAverage, PressRound);
                        process.InputData.PT1 = result.PressTime;
                        process.InputData.ST2 = result.Duration;
                        process.InputData.V2 = null;
                        process.InputData.Z2 = result.Posmax;
                        break;
                    case 6:
                        result = analyse_4567(step);

                        process.MachinaData.P2 = result.PAverage;
                        process.MachinaData.PT1 = result.PressTime;

                        process.MachinaData.G3_lower = result.TRateLower;
                        process.MachinaData.G3_upper = result.TRateUpper;

                        process.MachinaData.G3_lower_quali = result.TRateLowerQuality;
                        process.MachinaData.G3_upper_quali = result.TRateUpperQuality;

                        process.MachinaData.T2_lower = result.TChangeLower;
                        process.MachinaData.T2_upper = result.TChangeUpper;

                        process.InputData.G2 = Math.Round((((result.TUpper_Last + result.TLower_Last) / 2) - ((result.TUpper_First + result.TLower_First) / 2)) / result.Duration,2);
                        process.InputData.T2 = (result.TChangeLower + result.TChangeUpper) / 2;
                        process.InputData.P2 = Math.Round(result.PAverage, PressRound);

                        break;
                    case 7:
                        result = analyse_7(step);
                        process.MachinaData.T5_lower = result.TminLower;
                        process.MachinaData.T5_upper = result.TminUpper;

                        process.MachinaData.T3_lower = result.TmaxLower;
                        process.MachinaData.T3_upper = result.TmaxLower;

                        process.MachinaData.T4_lower = result.TChangeLower;
                        process.MachinaData.T4_upper = result.TChangeUpper;

                        process.MachinaData.G4_lower = result.TRateLower;
                        process.MachinaData.G4_upper = result.TRateUpper;

                        process.MachinaData.G4_lower_quali = result.TRateLowerQuality;
                        process.MachinaData.G4_upper_quali = result.TRateUpperQuality;

                        process.MachinaData.P3 = result.PAverage;

                        process.InputData.T3 = (result.TLower_First + result.TUpper_First) / 2;
                        process.InputData.T4 = (result.TChangeUpper + result.TChangeLower) / 2;
                        process.InputData.T5 = (result.TLower_Last + result.TUpper_Last) / 2;

                        process.InputData.P3 = Math.Round(result.PAverage, 2);

                        break;


                }

                if (process.InputData.Tvu == null)
                    process.InputData.Tvu = process.InputData.Tv;



            }





            return process;
        
        
        
        }

        private static AnalyseResults analyse_123(MonFile.ProcessStep step)
        {
            AnalyseResults _result = new AnalyseResults();

            double[] time = new double[step.MeasuringPoints.Count];
            double[] tUpper = new double[step.MeasuringPoints.Count];
            double[] tLower = new double[step.MeasuringPoints.Count];
            double[] press = new double[step.MeasuringPoints.Count];
            double[] pos = new double[step.MeasuringPoints.Count];

            for (int i = 0; i < step.MeasuringPoints.Count; i++)
            {
                time[i] = step.MeasuringPoints[i].TimePass;
                tUpper[i] = step.MeasuringPoints[i].UTemp1;
                tLower[i] = step.MeasuringPoints[i].LTemp1;
                press[i] = step.MeasuringPoints[i].PressZ;
                pos[i] = step.MeasuringPoints[i].PosZ;
            }

            _result.TmaxUpper = Enumerable.Range(0, tUpper.Length).Max(i => tUpper[i]);
            _result.TminUpper = Enumerable.Range(0, tUpper.Length).Min(i => tUpper[i]);
            _result.TAverageUpper = tUpper.Average();

            _result.TmaxLower = Enumerable.Range(0, tLower.Length).Max(i => tLower[i]);
            _result.TminLower = Enumerable.Range(0, tLower.Length).Min(i => tLower[i]);
            _result.TAverageLower = tLower.Average();

            _result.Pmin = Enumerable.Range(0, press.Length).Min(i => press[i]);
            _result.Pmax = Enumerable.Range(0, press.Length).Max(i => press[i]);

            _result.Posmin = Enumerable.Range(0, pos.Length).Min(i => pos[i]);
            _result.Posmax = Enumerable.Range(0, pos.Length).Max(i => pos[i]);

            double a1, a2, a3;
            double b1, b2, b3;
            double c1, c2, c3;

            LinearRegression(time, tUpper, 0, time.Length, out c1, out b1, out a1);
            _result.TRateUpper = Math.Round(a1, 4); _result.TRateUpperQuality = Math.Round(c1, 4);

            LinearRegression(time, tLower, 0, time.Length, out c2, out b2, out a2);
            _result.TRateLower = Math.Round(a2, 4); _result.TRateLowerQuality = Math.Round(c2, 4);

            LinearRegression(time, pos, 0, time.Length, out c3, out b3, out a3);
            _result.PosRate = Math.Round(a3, 4);

            _result.Duration = Convert.ToInt32(time[time.Length - 1] - time[0]);


            _result.TUpper_First = tUpper[0]; _result.TUpper_Last = tUpper[tUpper.Length - 1];
            _result.TLower_First = tLower[0]; _result.TLower_Last = tLower[tLower.Length - 1];

            return _result;
        }

        private static AnalyseResults analyse_4567(MonFile.ProcessStep step)
        {
            AnalyseResults _result = new AnalyseResults();

            double[] time = new double[step.MeasuringPoints.Count];
            double[] tUpper = new double[step.MeasuringPoints.Count];
            double[] tLower = new double[step.MeasuringPoints.Count];
            double[] press = new double[step.MeasuringPoints.Count];
            double[] pos = new double[step.MeasuringPoints.Count];

            for (int i = 0; i < step.MeasuringPoints.Count; i++)
            {
                time[i] = step.MeasuringPoints[i].TimePass;
                tUpper[i] = step.MeasuringPoints[i].UTemp1;
                tLower[i] = step.MeasuringPoints[i].LTemp1;
                press[i] = step.MeasuringPoints[i].PressZ;
                pos[i] = step.MeasuringPoints[i].PosZ;
            }

            _result.TmaxUpper = Enumerable.Range(0, tUpper.Length).Max(i => tUpper[i]);
            _result.TminUpper = Enumerable.Range(0, tUpper.Length).Min(i => tUpper[i]);
            _result.TAverageUpper = tUpper.Average();

            _result.TmaxLower = Enumerable.Range(0, tLower.Length).Max(i => tLower[i]);
            _result.TminLower = Enumerable.Range(0, tLower.Length).Min(i => tLower[i]);
            _result.TAverageLower = tLower.Average();

            _result.Pmin = Enumerable.Range(0, press.Length).Min(i => press[i]);
            _result.Pmax = Enumerable.Range(0, press.Length).Max(i => press[i]);

            _result.Posmin = Enumerable.Range(0, pos.Length).Min(i => pos[i]);
            _result.Posmax = Enumerable.Range(0, pos.Length).Max(i => pos[i]);

            double a1, a2, a3;
            double b1, b2, b3;
            double c1, c2, c3;

            LinearRegression(time, tUpper, 0, time.Length, out c1, out b1, out a1);
            _result.TRateUpper = Math.Round(a1, 4); _result.TRateUpperQuality = Math.Round(c1, 4);

            LinearRegression(time, tLower, 0, time.Length, out c2, out b2, out a2);
            _result.TRateLower = Math.Round(a2, 4); _result.TRateLowerQuality = Math.Round(c2, 4);

            LinearRegression(time, pos, 0, time.Length, out c3, out b3, out a3);
            _result.PosRate = Math.Round(a3, 4);

            _result.TUpper_First = tUpper[0]; _result.TUpper_Last = tUpper[tUpper.Length - 1];
            _result.TLower_First = tLower[0]; _result.TLower_Last = tLower[tLower.Length - 1];

            _result.Duration = Convert.ToInt32(time[time.Length - 1] - time[0]);

            //int cutCount = Convert.ToInt32(step.MeasuringPoints.Count * pAverageEdgeCutFactor);
            //_result.PAverage = Enumerable.Range(cutCount, press.Length - 2 * cutCount).Select(i => press[i]).Average();

            _result.PAverage = press.GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

            _result.PressTime = press.Where(item => Math.Abs(item) > (_result.PAverage - (_result.PAverage * 0.2))).Count();    
            
            
            try 
            {
                int changetime = _result.Duration - _result.PressTime;
                _result.TChangeUpper = tUpper[changetime];
                _result.TChangeLower = tLower[changetime];
            }
            catch 
            {
                _result.TChangeUpper = _result.TUpper_First;
                _result.TChangeLower = _result.TUpper_First;
            }


            return _result;
        }

        private static AnalyseResults analyse_7(MonFile.ProcessStep step)
        {
            AnalyseResults _result = new AnalyseResults();

            double[] time = new double[step.MeasuringPoints.Count];
            double[] tUpper = new double[step.MeasuringPoints.Count];
            double[] tLower = new double[step.MeasuringPoints.Count];
            double[] press = new double[step.MeasuringPoints.Count];
            double[] pos = new double[step.MeasuringPoints.Count];

            for (int i = 0; i < step.MeasuringPoints.Count; i++)
            {
                time[i] = step.MeasuringPoints[i].TimePass;
                tUpper[i] = step.MeasuringPoints[i].UTemp1;
                tLower[i] = step.MeasuringPoints[i].LTemp1;
                press[i] = step.MeasuringPoints[i].PressZ;
                pos[i] = step.MeasuringPoints[i].PosZ;
            }

            _result.TmaxUpper = Enumerable.Range(0, tUpper.Length).Max(i => tUpper[i]);
            _result.TminUpper = Enumerable.Range(0, tUpper.Length).Min(i => tUpper[i]);
            _result.TAverageUpper = tUpper.Average();

            _result.TmaxLower = Enumerable.Range(0, tLower.Length).Max(i => tLower[i]);
            _result.TminLower = Enumerable.Range(0, tLower.Length).Min(i => tLower[i]);
            _result.TAverageLower = tLower.Average();

            _result.Pmin = Enumerable.Range(0, press.Length).Min(i => press[i]);
            _result.Pmax = Enumerable.Range(0, press.Length).Max(i => press[i]);

            _result.Posmin = Enumerable.Range(0, pos.Length).Min(i => pos[i]);
            _result.Posmax = Enumerable.Range(0, pos.Length).Max(i => pos[i]);

            double a1, a2, a3;
            double b1, b2, b3;
            double c1, c2, c3;

            LinearRegression(time, tUpper, 0, time.Length, out c1, out b1, out a1);
            _result.TRateUpper = Math.Round(a1, 4); _result.TRateUpperQuality = Math.Round(c1, 4);

            LinearRegression(time, tLower, 0, time.Length, out c2, out b2, out a2);
            _result.TRateLower = Math.Round(a2, 4); _result.TRateLowerQuality = Math.Round(c2, 4);

            LinearRegression(time, pos, 0, time.Length, out c3, out b3, out a3);
            _result.PosRate = Math.Round(a3, 4);

            _result.TUpper_First = tUpper[0]; _result.TUpper_Last = tUpper[tUpper.Length - 1];
            _result.TLower_First = tLower[0]; _result.TLower_Last = tLower[tLower.Length - 1];

            _result.Duration = Convert.ToInt32(time[time.Length - 1] - time[0]);

            //int cutCount = Convert.ToInt32(step.MeasuringPoints.Count * pAverageEdgeCutFactor);
            //_result.PAverage = Enumerable.Range(cutCount, press.Length - 2 * cutCount).Select(i => press[i]).Average();

            _result.PAverage = press.GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

            _result.PressTime = press.Where(item => Math.Abs(item) > (_result.PAverage - (_result.PAverage * 0.05))).Count();

            int abovePAverageCount = press.Where(item => item > _result.PAverage).Count();


            try
            {
                int changetime = _result.Duration - (_result.Duration - _result.PressTime);
                _result.TChangeUpper = tUpper[changetime];
                _result.TChangeLower = tLower[changetime];
            }
            catch
            {
                _result.TChangeUpper = _result.TUpper_First;
                _result.TChangeLower = _result.TLower_First;
            }

            if (abovePAverageCount <= 15)
            {
                _result.TChangeUpper = _result.TUpper_First;
                _result.TChangeLower = _result.TLower_First;
                _result.PressTime = 0;
                _result.PAverage = 0;
            }


            return _result;
        }

        public class AnalyseResults
        {
            public double TminUpper { get; set; }
            public double TmaxUpper { get; set; }
            public double TminLower { get; set; }
            public double TmaxLower { get; set; }
            public double TAverageUpper { get; set; }
            public double TAverageLower { get; set; }
            public double Pmin { get; set; }
            public double Pmax { get; set; }
            public double Posmin { get; set; }
            public double Posmax { get; set; }

            public double TRateUpper { get; set; }
            public double TRateUpperQuality { get; set; }
            public double TRateLower { get; set; }
            public double TRateLowerQuality { get; set; }
            public double Prate { get; set; }
            public double PrateQuality { get; set; }
            public double PAverage { get; set; }
            public int Duration { get; set; }

            public int PressTime { get; set; }
            public double TUpper_First { get; set; }
            public double TUpper_Last { get; set; }
            public double TLower_First { get; set; }
            public double TLower_Last { get; set; }
            public double PosRate { get; set; }
            //Temperature changePress
            public double TChangeUpper { get; set; }
            public double TChangeLower { get; set; }
        }

        #region MathFuncions

        /// <summary>
        /// Fits a line to a collection of (x,y) points.
        /// </summary>
        /// <param name="xVals">The x-axis values.</param>
        /// <param name="yVals">The y-axis values.</param>
        /// <param name="inclusiveStart">The inclusive inclusiveStart index.</param>
        /// <param name="exclusiveEnd">The exclusive exclusiveEnd index.</param>
        /// <param name="rsquared">The r^2 value of the line.</param>
        /// <param name="yintercept">The y-intercept value of the line (i.e. y = ax + b, yintercept is b).</param>
        /// <param name="slope">The slop of the line (i.e. y = ax + b, slope is a).</param>
        public static void LinearRegression(double[] xVals, double[] yVals,
        int inclusiveStart, int exclusiveEnd,
        out double rsquared, out double yintercept,
        out double slope)
        {
            double sumOfX = 0;
            double sumOfY = 0;
            double sumOfXSq = 0;
            double sumOfYSq = 0;
            double ssX = 0;
            double ssY = 0;
            double sumCodeviates = 0;
            double sCo = 0;
            double count = exclusiveEnd - inclusiveStart;

            for (int ctr = inclusiveStart; ctr < exclusiveEnd; ctr++)
            {
                double x = xVals[ctr];
                double y = yVals[ctr];
                sumCodeviates += x * y;
                sumOfX += x;
                sumOfY += y;
                sumOfXSq += x * x;
                sumOfYSq += y * y;
            }
            ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
            ssY = sumOfYSq - ((sumOfY * sumOfY) / count);
            double RNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
            double RDenom = (count * sumOfXSq - (sumOfX * sumOfX))
            * (count * sumOfYSq - (sumOfY * sumOfY));
            sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

            double meanX = sumOfX / count;
            double meanY = sumOfY / count;
            double dblR = RNumerator / Math.Sqrt(RDenom);
            rsquared = dblR * dblR;
            yintercept = meanY - ((sCo / ssX) * meanX);
            slope = sCo / ssX;
        }

        #endregion

    }
}
