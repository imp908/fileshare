using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Intranet.Models.Logger
{
    public class FileLogger: ILogger
    {
     //   private StreamWriter file;
        public FileLogger()
        {
        //    file = new StreamWriter();
        }
        public void LogInformation(string infoString)
        {
            using (StreamWriter file = File.AppendText("E:\\EWSTest\\Log.txt"))
            {
                file.WriteLine(infoString);
            }

        }

        public void LogError(string errorString)
        {

        }

    }
}