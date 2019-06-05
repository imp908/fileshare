using NUnit.Framework;
using NotepadSaveAs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moq;

namespace NotepadSaveAs.Tests
{

    public class PathText
    {
        public string path { get; set; }
        public byte[] text { get; set; }

    }
    public class DialogResultWrapper
    {
        public DialogResult dialogResult { get; set; }
    }

    [TestFixture()]
    public class NotepadSaveAsImitationTests
    {
        Moq.Mock<INotepadSaveAsImitation> notepad;
        List<PathText> FileIO;
        PathText pt;
        string pathToGetExp, pathToGetAct;
        byte[] textExpected, textActual;

        Mock<DialogResultWrapper> dialogResultMock;
        SaveFileDialog saveFileDialog;

        [OneTimeSetUp]
        public void TestSetup()
        {
            pathToGetExp = @"C:\temp\text.txt";
            textExpected = Encoding.ASCII.GetBytes(@"test text1");
            FileIO = new List<PathText>();
            FileIO.Add(new PathText() { path = pathToGetExp, text = textExpected });
            pt = new PathText();
            notepad = new Moq.Mock<INotepadSaveAsImitation>();
            dialogResultMock = new Mock<DialogResultWrapper>();
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = pathToGetExp;

            notepad.Setup(s => s.GetDialogResult())
                //.Callback<DialogResult>(c => dialogResultMock.Object.dialogResult = c)
                .Returns(dialogResultMock.Object.dialogResult = new DialogResult());
            notepad.Setup(s => s.SetPathFromDialog(It.IsAny<SaveFileDialog>()))
                .Callback<SaveFileDialog>(c => saveFileDialog = c)
                .Returns(pathToGetExp);
            notepad.Setup(s => s.CheckDialogResult(It.IsAny<DialogResult>()))
                .Returns<DialogResult>(c => dialogResultMock.Object.dialogResult == c);
            notepad.Setup(s => s.SaveToPath(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((c, z) =>
            {
                pathToGetAct = c;
                textActual = z;
            }
            );

        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test()]
        public void SaveAsTest()
        {
            notepad.Object.SaveToPath(pathToGetExp, textExpected);
            notepad.Verify(s => s.SaveToPath(pathToGetExp, textExpected), Times.Once);

            Assert.AreEqual(pathToGetExp, pathToGetAct);
            Assert.AreEqual(textExpected, textActual);
        }
        [Test()]
        public void GetDialogResultTest()
        {
            DialogResult dialogresultAct = notepad.Object.GetDialogResult();
            DialogResult dialogResultExpected = dialogResultMock.Object.dialogResult;
            Assert.AreEqual(dialogResultExpected, dialogresultAct);
        }
        [Test()]
        public void SetPathFromDialogTest()
        {
            pathToGetAct = notepad.Object.SetPathFromDialog(saveFileDialog);
            Assert.AreEqual(pathToGetExp, pathToGetAct);
        }
        [Test()]
        public void CheckDialogResultTest()
        {
            DialogResult dr = new DialogResult();

            notepad.Object.CheckDialogResult(dr);
            notepad.Verify(s => s.CheckDialogResult(dr), Times.Once());
            Assert.AreEqual(dr, dialogResultMock.Object.dialogResult);
        }

    }
}