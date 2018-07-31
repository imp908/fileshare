using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// Proof of concept for 3 first buttons and 0 space
/// button conversion is hardcoded in char arrays
/// </summary>
namespace tNine
{

    //https://code.google.com/codejam/contest/351101/dashboard#s=p2
    //T9  

    //running custom test cases
    public static class tNineCheck
    {
        public static void GO()
        {
            tNineCheck.check1();
        }
        public static void check1()
        {
            List<CaseList> cl = new List<CaseList>() {
                new CaseList(){Case="ab cff",Exp="2 220222333 333",Act=null}
                , new CaseList("hg e a","44 403302",null)
            };

            foreach (CaseList cl_ in cl)
            {
                cl_.Act = tNineChecks.GO(new KeyPadStrait(), cl_.Case);
                cl_.check();
            }
        }

    }
    //class for test  cases usage
    public class CaseList
    {
        public CaseList() { }

        public CaseList(string @case, string exp, string act)
        {
            Case = @case;
            Exp = exp;
            Act = act;
        }

        public void check()
        {
            if (this.Exp == this.Act) { this.isOK = true; } else { this.isOK = false; }
            //or 
            //this.isOK=this.Exp == this.Act ?   true :  false;
        }
        public string Case { get; set; } = string.Empty;
        public string Exp { get; set; } = string.Empty;
        public string Act { get; set; } = null;
        public bool? isOK { get; private set; } = null;
    }

    //key presser interface handler
    public static class tNineChecks
    {
        public static string GO(IKeyPresser kp_, string case_)
        {
            return kp_.print(case_);
        }
    }

    //key presser interface with base realization
    public interface IKeyPresser
    {
        string print(string input);
    }
    public class KeyPresser : IKeyPresser
    {
        public string print(string input)
        {
            return null;
        }
    }

    //straightforward "naive" approach with char arrays
    public class KeyPadStrait : IKeyPresser
    {

        public static Dictionary<char, char?[]> keyPad = new Dictionary<char, char?[]>()
        {
            {'a', new char?[]{'2'} },{'b', new char?[]{'2','2'} },{'c', new char?[]{'2','2','2'} }
            ,{'d', new char?[]{'3'} },{'e', new char?[]{'3','3'} },{'f', new char?[]{'3','3','3'} }
            ,{'g', new char?[]{'4'} },{'h', new char?[]{'4','4'} },{'i', new char?[]{'4','4','4'} }
            ,{ ' ', new char?[]{'0'}}

        };
        public static List<char> presser(char[] str_)
        {

            //"".ToCharArray().First();
            char?[] foundPrev = null;

            List<char> res = new List<char>();
            for (int i = 0; i < str_.Length; i++)
            {
                char?[] found = null;
                if (keyPad.ContainsKey(str_[i]))
                {
                    keyPad.TryGetValue(str_[i], out found);
                    if (foundPrev != null)
                    {
                        if (foundPrev[0] == found[0]) { res.Add(' '); }
                    }

                    foreach (char ch in found)
                    {
                        res.Add(ch);
                    }
                    foundPrev = found;
                }
            }
            return res;
        }

        public string print(string input_)
        {
            return string.Join(string.Empty, KeyPadStrait.presser(input_.ToCharArray()));

        }
    }

}
