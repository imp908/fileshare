# SaveAsTest
Test project with Notepad like SaveAs method tests
///Class NotepadSaveAsImitation 
main class that immitates method, that calls winforms FileDialog.
Main method gets text as byte array to be saved, and fires FileSaveDialog, after user input, checks it
and if OK, tries to write to selected path, if possible.
///NotepadSaveAsImitationTests unit tests for INotepadSaveAsImitation interface uing moq objects.
///NotepadSaveAsImitationTests_integration integration tests for NotepadSaveAsImitation class.
Method SaveAs is fired from Main method of Programm file.
