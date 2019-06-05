using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iExcel = Microsoft.Office.Interop.Excel;

using System.IO;

using System.Runtime.InteropServices;

using System.Text.RegularExpressions;

namespace PDF_PARSE
{
    public class Parse
    {
        protected static string Pattern;
        protected static int colAct = 2;
        protected static int colStart = 2;
        protected static int colDiv = 1;

        protected static int colGAP = 3;

        public Parse()
        {

        }

        public Parse(string path )
        {
            colAct = colStart + colGAP;

            //List<string> patternList = new List<string>();
            Dictionary<string, string> patternList = new Dictionary<string, string>();
            
            string SC = "";

            //0
            patternList.Add("Zero", @"^Service Code:((\s+[A-Z,a-z]+\s?)|(\s?[A-Z,a-z]+\s+))");

            //I
            patternList.Add( "One",@"^[\w]+\s{1,3}");

            //II
            patternList.Add( "Two",@"(\s+[A-Z,a-z]+\s?)|(\s?[A-Z,a-z]+\s+)");

            //III
            patternList.Add("Three",@"\s[\d]+(\,|\.)([\d]+\.*)*");
          

            iExcel.Application apl;
            iExcel.Workbook wb;
            //iExcel.Sheets sh;
            iExcel.Worksheet ws;

            apl = new iExcel.Application();

            apl.Visible = true;
            apl.DisplayAlerts = false;

            wb = apl.Workbooks.Open(path);
            ws = (iExcel.Worksheet)wb.Sheets[1];

            for (int i = 1; i < ws.UsedRange.Rows.Count; i = i + 1)
            {
                string st = ws.Cells[i, colStart].Value2;
                string st2 = ws.Cells[i, colStart - 1].Value2;

                if (st != null)
                {
                    MatchCollection mtc = regexp(ws.Cells[i, colStart].Value, patternList["Zero"]);

                    if (mtc.Count == 1)
                    {
                        SC = mtc[0].Value;

                        colAct = colAct + colDiv;
                    }
                }

                if (st != null)
                {

                    foreach (KeyValuePair<string, string> kp in patternList)
                    {
                        string Pattern = kp.Value;
                        string Res = "";

                        if (kp.Key != "Two")
                        {
                            foreach (Match mt in regexp(ws.Cells[i, colStart].Value, Pattern))
                            {
                                ws.Cells[i, colAct].Value = mt.Value;
                                ws.Cells[i, colStart + 1].Value = SC;
                                ws.Cells[i, colAct].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aquamarine);
                                colAct = colAct + colDiv;
                            }
                        }
                        else
                        {
                            foreach (Match mt in regexp(ws.Cells[i, colStart].Value, Pattern))
                            {
                                Res = Res + "_" + mt.Value;
                            }
                            ws.Cells[i, colAct].Value = Res;
                            ws.Cells[i, colStart + 1].Value = SC;
                            ws.Cells[i, colAct].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Aquamarine);
                            colAct = colAct + colDiv;
                        }


                    }

                }

                colAct = colStart + colGAP;
                //ws.Cells[cl.Row, cl.Column + 1].Value = regexp(ws.Cells[cl.Row, cl.Column].Value);                                            

            }

            wb.SaveAs(Path.GetDirectoryName(path) + @"\parsed" + Path.GetExtension(path));

            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(apl);

            ws = null;
            wb = null;
            apl = null;

        }

        private static MatchCollection regexp(string input,string pattern_)
        {
            Regex reg = new Regex(pattern_);
            MatchCollection mc = reg.Matches(input);

            return mc;
        }

    }

}
