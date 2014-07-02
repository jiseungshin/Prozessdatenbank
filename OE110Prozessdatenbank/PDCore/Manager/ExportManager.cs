using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using PDCore.Processes;

namespace PDCore.Manager
{
    public static class ExportManager
    {

        private static void exportTurningMoore(List<PTurningMoore> processes, Excel.Worksheet ws)
        {
            // Überschriften eingeben
            ws.Cells[2, 1] = "Projekt";    
            ws.Cells[2, 2] = "Werkstück";  
            ws.Cells[2, 3] = "Material";   
            ws.Cells[2, 4] = "Werkzeugnummer";
            ws.Cells[2, 5] = "Radius";
            ws.Cells[2, 6] = "Spanwinkel";
            ws.Cells[2, 7] = "Drehzahl";
            ws.Cells[2, 8] = "Vorschub";  
            ws.Cells[2, 9] = "Schnittiefe";  
            ws.Cells[2, 10] = "Bemerkung";  

            // Formatieren der Überschrift
            Excel.Range myRangeHeadline;
            myRangeHeadline = ws.get_Range("A2", "J2");
            myRangeHeadline.Font.Bold = true;
            myRangeHeadline.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //myRangeHeadline.Borders.Weight = Excel.XlBorderWeight.xlThick;

            for (int i = 0; i < processes.Count;i++ )
            {
                ws.Cells[i + 3, 1] = ObjectManager.Instance.Projects.Find(item => item.ID == processes[i].ProjectID).Description;
                ws.Cells[i + 3, 2] = processes[i].Workpieces[0].Label;
                ws.Cells[i + 3, 3] = processes[i].Workpieces[0].Material.Name;
                ws.Cells[i + 3, 4] = processes[i].ToolID;
                ws.Cells[i + 3, 5] = processes[i].Radius;
                ws.Cells[i + 3, 6] = processes[i].CuttingAngle;
                ws.Cells[i + 3, 7] = processes[i].Speed;
                ws.Cells[i + 3, 8] = processes[i].Feed;
                ws.Cells[i + 3, 9] = processes[i].CuttingDepth;
                ws.Cells[i + 3, 10] = processes[i].Remark;

            }
                // Daten eingeben
                
           
            ws.Name = "Prozessdaten Drehen";
        }

        public static void foo(List<PTurningMoore> process)
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


                exportTurningMoore(process , myExcelWorkSheet);
               


                

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
