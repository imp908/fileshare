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
            if(ns.CheckDialogResult(ns.GetDialogResult()))
            {
                ns.SetPathFromDialog(ns.saveFileDialog);
                ns.SaveAs(ns.path, toWrite);
            }
                      
        }
    
    }

    public interface INotepadSaveAsImitation
    {
        string path { get; set; }
        System.Windows.Forms.DialogResult GetDialogResult();
        string SetPathFromDialog(SaveFileDialog fileDialog_);
        bool CheckDialogResult(DialogResult dr_);       
        void SaveAs(string path_, byte[] text_);

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

        public SaveFileDialog saveFileDialog;

        System.Windows.Forms.DialogResult dialogResult;

        public NotepadSaveAsImitation()
        {
            saveFileDialog = new SaveFileDialog();
            dialogResult = new DialogResult();
            saveFileDialog.Filter = @"Image Files(*.BMP; *.JPG; *.GIF)|*.BMP; *.JPG; *.GIF|
Excel files (*.xlsx)|*.xlsx|All files(*.*)|*.*|Text Files (*.txt)|*.txt";
           
        }

        public System.Windows.Forms.DialogResult GetDialogResult()
        {
            this.dialogResult = saveFileDialog.ShowDialog();
            return this.dialogResult;
        }
        public string SetPathFromDialog(SaveFileDialog fileDialog_)
        {
            if(string.IsNullOrEmpty(fileDialog_.FileName))
            {
                throw new EmptyFilepathException();
            }
            this.path = fileDialog_.FileName;
            return fileDialog_.FileName;
        }
        public bool CheckDialogResult(DialogResult dr_)
        {
            if(dr_ == DialogResult.OK)
            {
                return true;
            }
            return false;
        }      
        public void SaveAs(string path_,byte[] text_)
        {
            if (string.IsNullOrEmpty(path_)) { throw new EmptyFilepathException(); }         
 
            try
            {
                File.WriteAllBytes(path_, text_);
            }
            catch(PathTooLongException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                throw e;
            }
            catch (IOException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                throw e;
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
