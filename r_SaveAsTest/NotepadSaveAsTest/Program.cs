using NUnit.Framework;
using ConsoleApplication1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NotepadSaveAs;

namespace ConsoleApplication1.Tests
{
    [TestFixture()]
    public class NotepadSaveAsImitationTests_integration
    {
        NotepadSaveAsImitation notepad;
        string path_correct, path_directory_nofile, path_empty, path_null;
        byte[] text_;

        [OneTimeSetUp()]
        public void Initialization()
        {
            path_correct = @"C:\TEMP\test.txt";
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
        }

        [Test()]
        public void SaveAsTestInt()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetPathFromDialogTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SaveTest_CorrectPath_SavesToDiskCheck()
        {
            notepad.SaveAs(path_correct, text_);
            if (File.Exists(path_correct)) { Assert.Pass(); }
            Assert.Fail();
        }

        [Test()]
        public void SaveTest_EmptyFile_ThrowsIOException()
        {
            try
            {
                notepad.SaveAs(path_directory_nofile, text_);
            }
            catch (IOException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test()]
        public void SaveTest_EmptyPath_ThrowsEmptyException()
        {
            try
            {
                notepad.SaveAs(path_empty, text_);
            }
            catch (EmptyFilepathException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test()]
        public void SaveTest_NullPath_ThrowsEmptyException()
        {
            try
            {
                notepad.SaveAs(path_null, text_);
            }
            catch (EmptyFilepathException e)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }


    }
}
