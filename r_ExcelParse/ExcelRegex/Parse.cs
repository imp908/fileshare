using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excel = Microsoft.Office.Interop.Excel;

using System.IO;

using System.Runtime.InteropServices;

using System.Text.RegularExpressions;

namespace ExcelRegex
{
    public class Parse
    {
        protected static string Pattern;
        protected static int col = 1;
        protected static int colSt = 2;

        public Parse ()
        {

        }

        public Parse (string path,string pattern)
        {
            //I
            //pattern = @"^[\w]+\s{1,3}";

            //II
            //pattern = @"(\s+[A-Z,a-z]+\s?)|(\s?[A-Z,a-z]+\s+)";

            //III
            pattern = @"\s[\d]+\,([\d]+\.*)*";

            Pattern = pattern;
            
            Excel.Application apl;
            Excel.Workbook wb;
            //Excel.Sheets sh;
            Excel.Worksheet ws;

            apl = new Excel.Application();

            apl.Visible = true;
            apl.DisplayAlerts = false;

            wb = apl.Workbooks.Open(path);
            ws = (Excel.Worksheet)wb.Sheets[1];

            for(int i = 2; i < ws.UsedRange.Rows.Count;i=i+1)
            {
                string st = ws.Cells[i, colSt].Value2;
                string st2 = ws.Cells[i, colSt - 1].Value2;

                if (st != null)
                {
                    if (Pattern == null && st2 != null) { Pattern = st2; }

                    foreach (Match mt in regexp(ws.Cells[i, colSt].Value))
                    {
                        ws.Cells[i, colSt + col].Value = mt.Value;
                        col = col + 1;
                    }
                }
                //ws.Cells[cl.Row, cl.Column + 1].Value = regexp(ws.Cells[cl.Row, cl.Column].Value);
              
                col = 1;

            }

            wb.SaveAs(Path.GetDirectoryName(path) + @"\parsed" + Path.GetExtension(path));

            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(apl);

            ws = null;
            wb = null;
            apl = null;
          
        }

        private static MatchCollection regexp(string input)
        {
            Regex reg = new Regex(Pattern);
            MatchCollection mc = reg.Matches(input);
           
            return mc;
        }
    }

}
