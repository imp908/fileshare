using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace ConsoleApplication1
{

    class Program { 

        [STAThread]
        static void Main(string[] args)
        {

            SaveFileDialog sv = new SaveFileDialog();
            if (sv.ShowDialog() == DialogResult.OK)
            {
                                
                File.WriteAllText(sv.FileName, @"aaa");              
                
            }
            
        }
        
    }

    

}
