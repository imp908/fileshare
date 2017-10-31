
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace APItesting
{

    /// <summary>
    /// Testing APIS
    /// Manages class for JSON API url,expected,ectual,ok values. Read/create + export JSON file with values.
    /// For every URL execute webrequest, reads string response, compares with Expected value, 
    /// changes statusess - OK(true/flase), Exception message - if needed. Exports result.
    /// </summary>
    public class APItester_sngltn
    {

        public string URI { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool OK { get; set; }
        public string ExceptionText { get; set; }
        public string Comment { get; set; }

        public static List<APItester_sngltn> TestCases = new List<APItester_sngltn>();

        public void OK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = true;
        }
        public void OK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = true;
        }

        public void NotOK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = false;
        }
        public void NotOK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = false;
        }

    }

    //class for collection of test cases with expected and result
    //currently unused due to refactor needed
    public class TestCase
    {
        public int CaseNumber { get; private set; }
        public string Input { get; set; }
        public string Expected { get; set; }
        public string Actual { get; private set; }
        public bool Equal { get; private set; }

        public TestCase()
        {
            this.CaseNumber += 1;
            this.Actual = string.Empty;
            this.Expected = string.Empty;
            this.Equal = false;
        }
        public void Check(string Actual_)
        {
            this.Actual = Actual_;
            if (this.Expected == this.Actual) { this.Equal = true; }
            else { this.Equal = false; }
        }
    }

}