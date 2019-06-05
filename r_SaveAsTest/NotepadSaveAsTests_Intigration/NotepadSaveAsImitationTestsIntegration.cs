using NUnit.Framework;
using NotepadSaveAs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ConsoleApplication1.Tests
{
    [TestFixture()]
    public class NotepadSaveAsImitationTests_integration
    {
        NotepadSaveAsImitation notepad;
        string path_correct, path_directory_nofile, path_empty, path_null, path_SaveAs;
        byte[] text_;
   

       [OneTimeSetUp()]
        public void Initialization()
        {
            path_correct = @"C:\TEMP\test.txt";
            path_SaveAs = @"C:\TEMP\testSavedAs.txt";
            path_directory_nofile = @"C:\TEMP\";
            path_empty = string.Empty;
            path_null = null;
            text_ = Encoding.ASCII.GetBytes(@"C:\TEMP\");
            notepad = new NotepadSaveAsImitation();                 

            //remove test file from disk before test
            if (File.Exists(path_correct))
            {
                File.Delete(path_correct);
            }
        }

        [TearDown]
        public void TearDown()
        {
            //remove test file from disk after test
            if (File.Exists(path_correct))
            {
                File.Delete(path_correct);
            }
            if (File.Exists(path_SaveAs))
            {
                File.Delete(path_SaveAs);
            }
            
        }       

        [Test()]      
        public void CheckDialogResultTest_FlaseForCancel()
        {
           Assert.IsFalse( notepad.CheckDialogResult(DialogResult.Cancel));
        }
        [Test()]
        public void GetPathFromDialogTest_TrueForOk()
        {
            Assert.IsTrue(notepad.CheckDialogResult(DialogResult.OK));
        }
        [Test()]
        public void SaveToPathTest_CorrectPath_SavesToDiskCheck()
        {
            notepad.SaveToPath(path_correct, text_);
            if (File.Exists(path_correct)) { Assert.Pass(); }
            Assert.Fail();
        }

        [Test()]
        public void SaveToPathTest_EmptyFile_ThrowsIOException()
        {
            try
            {
                notepad.SaveToPath(path_directory_nofile, text_);
            }
            catch (IOException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test()]
        public void SaveToPathTest_EmptyPath_ThrowsEmptyException()
        {
            try
            {
                notepad.SaveToPath(path_empty, text_);
            }
            catch (EmptyFilepathException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test()]
        public void SaveToPathTest_NullPath_ThrowsEmptyException()
        {
            try
            {
                notepad.SaveToPath(path_null, text_);
            }
            catch (EmptyFilepathException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }      
       
    }
}
