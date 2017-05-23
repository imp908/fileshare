using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Text;

using OfficeOpenXml;
using System.Data.Entity;

using Presentation_.DAL;

namespace Presentation_.Models
{
    public class upload
    {
        public List<string> fileNames = new List<string>();
        public IEnumerable<HttpPostedFileBase> filesPosted {get;set;}
        public List<parsedValues> parsedValues = new List<DAL.parsedValues>();
        private string currentUser;
        public string folder;

        public String CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public upload()
        {
                       
        }      

        public void UploadFromFiles(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            GetPostedFiles(filesPosted_);
            FilesRead();
            DataDelete();
            DataInsert();
        }

        public void GetPostedFiles(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            if(filesPosted_ != null) {
                this.filesPosted = filesPosted_;
                GetFilenames(filesPosted_);
            }
        }
        private void GetFilenames(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            foreach(HttpPostedFileBase file in filesPosted_)
            {
                this.fileNames.Add(file.FileName);
            }
        }
        private void FilesRead()
        {
           
            string path = "";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (HttpPostedFileBase file in this.filesPosted)
            {
                FilesParse(file);
                path = Path.Combine(folder, file.FileName);
                file.SaveAs(path);
            }
        }
        private void FilesParse(HttpPostedFileBase file_)
        {
            using (ExcelPackage p = new ExcelPackage(file_.InputStream))
            {
                foreach(ExcelWorksheet ws in p.Workbook.Worksheets)
                {
                    OfficeOpenXml.ExcelAddressBase dim = ws.Dimension;
                    if(dim!=null)
                    {
                        int rowSt = dim.Start.Row;
                        int colSt = dim.Start.Column;
                        int rowFn = dim.End.Row;
                        int colFn = dim.End.Column;
                        StringBuilder sb = new StringBuilder(100, 500);
                        
                        for(int i2 = colSt; i2 <= colFn;i2++)
                        {
                            if(ws.Cells[1, i2].Value!=null && ws.Cells[1, i2].Value.ToString().Contains("merchant"))
                            {
                                for(int i =rowSt+1;i<=rowFn;i++)
                                {
                                    if(ws.Cells[i, i2].Value!=null)
                                    {
                                        if(!parsedValues.Any(s=> s.ITEM_ID== ws.Cells[i, i2].Value.ToString() && s.USER_ID == CurrentUser))
                                        {
                                            parsedValues.Add(new DAL.parsedValues { ITEM_ID = ws.Cells[i, i2].Value.ToString(), USER_ID = CurrentUser });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void DataDelete()
        {
            DWH_REPLICAEntities db = new DWH_REPLICAEntities();
            var arr = from s in db.parsedValues where s.USER_ID==currentUser select s ;
            db.parsedValues.RemoveRange(arr);
            db.SaveChanges();
        }
        private void DataInsert()
        {
            if(parsedValues.Count!=0)
            {
                foreach(parsedValues pv in parsedValues)
                {
                    DWH_REPLICAEntities db = new DWH_REPLICAEntities();
                    db.parsedValues.Add(new DAL.parsedValues { ITEM_ID = pv.ITEM_ID, USER_ID = pv.USER_ID });
                    db.SaveChanges();
                }
            }
        }
    }
}