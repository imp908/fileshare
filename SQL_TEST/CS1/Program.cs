using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace ConsoleApplication1
{

    class Program {       

        [STAThread]
        static void Main(string[] args)
        {

            byte[] toWrite = Encoding.ASCII.GetBytes(@"Test text 1");
            NotepadSaveAsImitation ns = new NotepadSaveAsImitation();
            ns.text = toWrite;
            ns.SaveAs();

        }
      
    }

    public interface INotepadSaveAsImitation
    {
        string path { get; set; }
        byte[] text { get; set; }

        void SaveAs();
        string GetPlace();
        void Save(string path_, byte[] text_);

    }
    /// <summary>
    /// Class immitates Isolated notepad SaveAa method.
    /// Assumptions are, that methods receives inputed text from outside,
    /// inputed text is in byte[] array, not string.
    /// after call, methods calls SaveFileDialog winforms method
    /// </summary>
    public class NotepadSaveAsImitation : INotepadSaveAsImitation
    {
        
        public string path { get; set; }
        public byte[] text { get; set; }
        SaveFileDialog sv = new SaveFileDialog();
        
        public void SaveAs()
        {
            sv.Filter = @"Image Files(*.BMP; *.JPG; *.GIF)|*.BMP; *.JPG; *.GIF|
Excel files (*.xlsx)|*.xlsx|All files(*.*)|*.*|Text Files (*.txt)|*.txt";
            GetPlace();
            if (!string.IsNullOrEmpty(this.path))
            {
                Save(this.path, text);
            }
        }
        public string GetPlace()
        {
            string path_ = string.Empty;

            if (sv.ShowDialog() == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(sv.FileName))
                {
                    path = sv.FileName;
                    return path_;
                }
            }

            return path_;
        }
        public void Save(string path_,byte[] text_)
        {
            if (string.IsNullOrEmpty(path_)) { throw new EmptyFilepathException(); }         

            try
            {
                File.WriteAllBytes(path_, text_);
            }
            catch(PathTooLongException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            catch (IOException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }      
    }
    
    public class EmptyFilepathException : Exception
    {
        public EmptyFilepathException():base(@"File path not provided")
        {

        }
        public EmptyFilepathException(string input_) : base(input_)
        {

        }
    }
}
