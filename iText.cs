using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using iExcel = Microsoft.Office.Interop.Excel;

using System.Reflection;

using System.IO;

using System.Text.RegularExpressions;


namespace PDF_PARSE
{

    public class PARSE_PDF
    {
        
        public PARSE_PDF()
        {
            
        }

        public PARSE_PDF(string path)
        {
            string result;

            int col, row;

            iExcel.Application apl;
            iExcel.Workbook wb;
            iExcel.Worksheet ws;

            row = 2;

            apl= new iExcel.Application();
            apl.Visible = true;
            apl.DisplayAlerts = false;

            wb = (iExcel.Workbook)apl.Workbooks.Add(Missing.Value);
            ws = (iExcel.Worksheet)wb.Worksheets[1];

            ws.Activate();

            // System.IO.Path.GetDirectoryName(path)
                     

            foreach (string f in System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(path)))
            {
                result = null;                
                result = iText.ExtractTextFromPdf(f);

                var res = result.Split(new[] { '\n' });

                for (int i = 0; i < res.Length; i = i + 1)
                {
                    if (res[i] != "")
                    {
                        ws.Cells[row, 1].Value = System.IO.Path.GetFileName(f).ToString();
                        //ws.Cells[row, 2].Value = result;
                        ws.Cells[row, 2].Value = res[i];
                        row = row + 1;
                    }
                   
                }
                row = row + 1;
               
            }

            wb.SaveAs(System.IO.Path.GetDirectoryName(path) + @"\Parsed\to_parse.xlsx");
            
            wb.Close();
        }
    }

    public static class iText
    {
        public static string ExtractTextFromPdf(string path)
        {
            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();

            using (PdfReader reader = new PdfReader(path))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                    string[] theLines = thePage.Split('\n');
                    foreach (var theLine in theLines)
                    {
                        text.AppendLine(theLine);
                    }
                }

                return text.ToString();
            }
        } 
    }


    public static class TO_COLUMN_reg
    {

        public static void Parse(string path)
        {
          

            iExcel.Application apl;
            iExcel.Workbook wb;
            iExcel.Worksheet ws;
            iExcel.Sheets wss;

            int stCol = 5;

            int row =1994;
            int col = stCol;

            string inp;
            string[] ts;

            apl = new iExcel.Application();
            apl.Visible = true;
            apl.DisplayAlerts = false;

            wb = (iExcel.Workbook)apl.Workbooks.Open(path);
            wss = wb.Worksheets;

            ws = (iExcel.Worksheet)wss.get_Item(1);
          
            ws.Activate();

            char[] del = {' '};

            for (int i = 1; i < 9000; i = i+1)
            {
               //split 
               //splstr( i, ws,  del, row, col, stCol);

               reg(i, ws, del, row, col, stCol);

               row = row + 1;
               col = stCol;
            }

        }

        private static void reg(int i, iExcel.Worksheet ws, char[] del, int row, int col, int stCol)
        {
           
            string inp = ws.Cells[i, 2].Value;

            if(inp!=null)
            {
                string pattern = @"[0-9]{1,5},[0-9]{3}.[0-9]{3}";
                Regex rg = new Regex(pattern);
                MatchCollection mcl = rg.Matches(inp);
                foreach (Match mc in mcl)
                {
                    string res = mc.Value;
                }
            }          

        }

        private static string [] reg2 ()
        {
            string[] res = null;

            return res;
        }

        //DefaultSplitCharacter line
        private static void splstr(int i,iExcel.Worksheet ws, char[] del,int row,int col,int stCol)
        {
            //ws.Cells[i, 3].Value = ws.Cells[i, 2].Value;

            var val= ws.Cells[i, 2].Value;

            if (val!=null)
            {
                foreach (string st in spl((string)(ws.Cells[i, 2].Value), del))
                {
                    ws.Cells[row, col].Value = st;
                    ws.Cells[row, col].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Silver);
                    //ws.Cells[row, col].Interior.Color = 350;

                    col = col + 1;
                };
            }
            
           
        }
        private static string[] spl(string inp, char[] del)
        {           
          string[] tempval = inp.Split(del);
          return tempval;
        }

    }
}
