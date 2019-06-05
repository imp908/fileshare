using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotepadSaveAs
{
   
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            NotepadSaveAsImitation np = new NotepadSaveAsImitation();
            np.SaveAs(Encoding.ASCII.GetBytes(@"Test text to save"));
        }
    }
}
