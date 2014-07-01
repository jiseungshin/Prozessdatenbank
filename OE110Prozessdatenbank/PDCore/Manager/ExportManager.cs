using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace PDCore.Manager
{
    public static class ExportManager
    {

        private static void exportTurningMoore()
        { 
        
        }

        public static void foo()
        {
            // Variablen deklarieren 
            Excel.Application myExcelApplication;
            Excel.Workbook myExcelWorkbook;
            Excel.Worksheet myExcelWorkSheet;
            myExcelApplication = null;

            try
            {
                // First Contact: Excel Prozess initialisieren
                myExcelApplication = new Excel.Application();
                myExcelApplication.Visible = true;
                myExcelApplication.ScreenUpdating = true;

                // Excel Datei anlegen: Workbook
                var myCount = myExcelApplication.Workbooks.Count;
                myExcelWorkbook = (Excel.Workbook)(myExcelApplication.Workbooks.Add(System.Reflection.Missing.Value));
                myExcelWorkSheet = (Excel.Worksheet)myExcelWorkbook.ActiveSheet;

                // Überschriften eingeben
                myExcelWorkSheet.Cells[2, 2] = "Hamburg";    // Zelle B2
                myExcelWorkSheet.Cells[2, 3] = "Nürnberg";   // Zelle C2
                myExcelWorkSheet.Cells[2, 4] = "Hamburg";    // Zelle D2

                // Formatieren der Überschrift
                Excel.Range myRangeHeadline;
                myRangeHeadline = myExcelWorkSheet.get_Range("B2", "D2");
                myRangeHeadline.Font.Bold = true;
                myRangeHeadline.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                myRangeHeadline.Borders.Weight = Excel.XlBorderWeight.xlThick;

                // Daten eingeben
                myExcelWorkSheet.Cells[3, 2] = "18";
                myExcelWorkSheet.Cells[3, 3] = "21";
                myExcelWorkSheet.Cells[3, 4] = "11";
                myExcelWorkSheet.Cells[4, 2] = "21";
                myExcelWorkSheet.Cells[4, 3] = "32";
                myExcelWorkSheet.Cells[4, 4] = "22";
                myExcelWorkSheet.Cells[5, 2] = "12";
                myExcelWorkSheet.Cells[5, 3] = "56";
                myExcelWorkSheet.Cells[5, 4] = "14";
                myExcelWorkSheet.Name = "Kunden";


                // Chart erzeugen
                Excel.Range myRangeValues;
                myRangeValues = myExcelWorkSheet.get_Range("B3", "D5");

                Excel.Chart myChart = (Excel.Chart)myExcelWorkbook.Charts.Add(
                  System.Reflection.Missing.Value,
                  System.Reflection.Missing.Value,
                  System.Reflection.Missing.Value,
                  System.Reflection.Missing.Value);

                myChart.ChartWizard(
                  myRangeValues,
                  Excel.XlChartType.xl3DColumn,
                  System.Reflection.Missing.Value,
                  Excel.XlRowCol.xlRows,
                  System.Reflection.Missing.Value,
                  System.Reflection.Missing.Value,
                  System.Reflection.Missing.Value,
                  "Titel",
                  "Kunden",
                  "Anzahl",
                  System.Reflection.Missing.Value);

                myChart.CopyPicture(Excel.XlPictureAppearance.xlScreen,
                 Excel.XlCopyPictureFormat.xlBitmap,
                 Excel.XlPictureAppearance.xlScreen);

                myChart.Location(Excel.XlChartLocation.xlLocationAsObject, myExcelWorkSheet.Name);

                // Excel Datei abspeichern
                // wenn die Datei vorher vorhanden ist, kommt in Excel eine Fehlermeldung.
                myExcelWorkbook.Close(true, "D:\\kunden.xlsx", System.Reflection.Missing.Value);
            }

            catch (Exception ex)
            {
                String myErrorString = ex.Message;
                System.Windows.MessageBox.Show(myErrorString);
            }
            finally
            {
                // Excel beenden
                if (myExcelApplication != null)
                {
                    myExcelApplication.Quit();
                }

            }

        }
    }
    
}
