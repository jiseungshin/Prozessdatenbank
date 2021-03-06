﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace OE110Prozessdatenbank
{
    public class NullableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? string.Empty : String.Format(culture, "{0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(String.Format(culture, "{0}", value)) ? null : value;
        }
    }

    public class IntToNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value) == -1 ? string.Empty : String.Format(culture, "{0}", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(String.Format(culture, "{0}", value)) ? -1 : value;
        }
    }

    public class NullToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? "--" : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class BoolToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value ==true)
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_ok_16xMD.png", UriKind.RelativeOrAbsolute));
            }
            else
                return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_Blocked_16xMD.png", UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class IntToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch((int)value)
            {
                case 1:
                    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/processBlue_16xLG.png", UriKind.RelativeOrAbsolute));
                case 2:
                    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_ok_16xMD.png", UriKind.RelativeOrAbsolute));
                default:
                    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_Blocked_16xMD.png", UriKind.RelativeOrAbsolute));
            }

            //if ((int)value == 2)
            //{
            //    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_ok_16xMD.png", UriKind.RelativeOrAbsolute));
            //}
            //if ((int)value == 1)
            //{
            //    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_ok_16xMD.png", UriKind.RelativeOrAbsolute));
            //}
            //if ((int)value == 0)
            //    return new BitmapImage(new Uri(@"pack://application:,,,/Icons/Status_Blocked_16xMD.png", UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    //public class LoadFactorToPercent : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return "Auslastung: " + (System.Convert.ToDouble(value) / 100).ToString("P2");
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        return string.IsNullOrEmpty(String.Format(culture, "{0}", value)) ? -1 : value;
    //    }
    //}



    public class PVCategoryToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((int)value == 0)
                {
                    return System.Windows.Media.Brushes.Green;
                }
                if ((int)value == 1)
                {
                    return System.Windows.Media.Brushes.Orange;
                }
                if ((int)value == 2)
                {
                    return System.Windows.Media.Brushes.Red;
                }
                else
                    return System.Windows.Media.Brushes.Green;
            }
            catch{return System.Windows.Media.Brushes.Green;}

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? dt = (DateTime?)value;
            if (dt != null)
                return dt.GetValueOrDefault().ToShortDateString();
            else
                return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }

    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = (DateTime)value;
            return dt.ToShortDateString() +" "+ dt.ToShortTimeString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }

    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _status = value.ToString();

            switch (_status)
            {
                case "polished":
                    return "vorbereitet";
                case "raw":
                    return "unbearbeitet";
                case "processed":
                    return "Versuch durchgeführt";
                case "coated":
                    return "Beschichtet";
                case "analysed":
                    return "Analysiert";
                case "decoated":
                    return "Decoated";
                case "terminated":
                    return "beendet";
                case "cancelled":
                    return "abgebrochen";
                case "INPROCESS":
                    return "im Durchlauf";
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }

    public class AnalysisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string _analysis = value.ToString();

            switch (_analysis)
            {
                case "REM":
                    return "REM";
                case "XPS":
                    return "XPS";
                case "WI":
                    return "Weißlicht";
                case "LIMI":
                    return "Lichtmikroskop";
                case "XRD":
                    return "XRD";
                case "EBSD":
                    return "EBSD";
                case "PROF":
                    return "Profilometer";
                case "PHOTO":
                    return "Fotodoku";
                default:
                    return value.ToString();
            }
        }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
        
    }



}
