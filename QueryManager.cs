using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IQueryManagers;

namespace QueryManagers
{

    public class TextFormatGenerate : ITypeToken
    {
        public string Text { get; set; } = string.Empty;

        public TextFormatGenerate(List<ITypeToken> tokens_)
        {
            string result = string.Empty;

            for (int i = 0; i < tokens_.Count(); i++)
            {
                result += "{" + i + "}";
                if (i != tokens_.Count() - 1)
                {
                    result += @" ";
                }


            }
            Text = result;
        }
    }

    //Tokens for storing Resulted build strings (URLs, commands e.t.c)
    public class TextToken : ITypeToken
    {
        public string Text { get; set; }
    }

    // Base class for url tokens concatenation
    //TextBuilder realization for Format placeholders for URL concatenation
    public class TextBuilder : ITextBuilder
    {

        public ITypeToken Text { get; set; }
        public ITypeToken FormatPattern { get; set; }
        public List<ITypeToken> Tokens { get; set; }
        
        public TextBuilder()
        {

        }
        //concatenates Tokens from colection with format pattern
        public TextBuilder(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {        
            this.FormatPattern = FormatPattern_;
            this.Tokens = tokens_;
            SetText(this.Tokens, this.FormatPattern);
        }
        //cocatenates URLbuilders Token collections from URLbuilders with format pattern
        public TextBuilder(List<ITextBuilder> texts_, ITypeToken FormatPattern_, BuildTypeFormates type_)
        {
      
            this.FormatPattern = FormatPattern_;
            this.Tokens = new List<ITypeToken>();

            if (type_ == BuildTypeFormates.FULL)
            {
                this.FormatPattern = FormatPattern_;
                this.Tokens = texts_.SelectMany(s => s.Tokens).ToList();
                SetText(this.Tokens, this.FormatPattern);
            }
            if (type_ == BuildTypeFormates.NESTED)
            {
                List<ITypeToken> str = new List<ITypeToken>();
                foreach (ITextBuilder tb in texts_)
                {
                    //build string
                    tb.SetText(tb.Tokens, tb.FormatPattern);
                    //add tokens to list
                    this.Tokens.AddRange(tb.Tokens);
                    //concatenate formats according to new, nested format
                    str.Add(tb.FormatPattern);
                }

                string[] arr = (from s in str select s.Text).ToArray();

                //add format concatenation 
                //concatenate collection of formats according to format
                this.FormatPattern.Text = string.Format(this.FormatPattern.Text, arr);
                //recount foramt variables from 0 to max
                this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
                //build new string
                SetText(this.Tokens, this.FormatPattern);
            }
        }
        public void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {
            List<string> str = new List<string>();
            this.FormatPattern = FormatPattern_;
            this.Tokens = tokens_;
            foreach (ITypeToken tt in this.Tokens)
            {
                if (tt != null) { str.Add(tt.Text); }
                else { str.Add(null); }
            }            
            this.Text = new TextToken() { Text = string.Format(this.FormatPattern.Text, str.ToArray()) };
        }
        public string GetText()
        {
            return this.Text.Text;
        }
        public string Build(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {
            SetText(tokens_, FormatPattern_);
            return GetText();
        }
        public string Build(List<ITextBuilder> texts_, ITypeToken FormatPattern_, BuildTypeFormates type_)
        {

            this.FormatPattern = FormatPattern_;
            this.Tokens = new List<ITypeToken>();

            if (type_ == BuildTypeFormates.FULL)
            {
                this.FormatPattern = FormatPattern_;
                this.Tokens = texts_.SelectMany(s => s.Tokens).ToList();
                SetText(this.Tokens, this.FormatPattern);
            }
            if (type_ == BuildTypeFormates.NESTED)
            {
                List<ITypeToken> str = new List<ITypeToken>();
                foreach (ITextBuilder tb in texts_)
                {
                    //build string
                    tb.SetText(tb.Tokens, tb.FormatPattern);
                    //add tokens to list
                    this.Tokens.AddRange(tb.Tokens);
                    //concatenate formats according to new, nested format
                    str.Add(tb.FormatPattern);
                }

                string[] arr = (from s in str select s.Text).ToArray();

                //add format concatenation 
                //concatenate collection of formats according to format
                this.FormatPattern.Text = string.Format(this.FormatPattern.Text, arr);
                //recount foramt variables from 0 to max
                this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
                //build new string
                SetText(this.Tokens, this.FormatPattern);
            }

            return GetText();
        }

        public enum BuildTypeFormates { FULL, NESTED }

        //recounts format string parameters from 0 for concatenated from several format strings
        string FormatStringReArrange(string input_)
        {
            string result = string.Empty;
            List<char> input_chars = input_.ToCharArray().ToList();
            int i = 0, i2 = 0, ctr = 0;
            for (i = 0; i < input_chars.Count; i++)
            {
                i2 = i;
                if (char.IsDigit(input_chars[i]))
                {
                    while (char.IsDigit(input_chars[i2 + 1]))
                    {
                        i2 += 1;
                    }
                    for (int i3 = i; i3 <= i2; i3++)
                    {
                        input_chars.RemoveAt(i3);
                    }

                    char[] chToInsert = ctr.ToString().ToCharArray();

                    if (chToInsert.Count() > 1)
                    {

                        for (int i4 = 0; i4 < chToInsert.Count(); i4++)
                        {
                            input_chars.Insert(i, chToInsert[i4]);
                            i += 1;
                        }
                        i -= 1;
                    }
                    else
                    {
                        input_chars.Insert(i, chToInsert[0]);
                    }
                    ctr += 1;
                }
            }

            return result = string.Concat(input_chars);
        }

    }


    /// <summary>
    /// Http manager Tokens
    /// </summary>

    public class GET : ITypeToken
    {
        public string Text { get; set; } = "GET";
    }
    public class PUT : ITypeToken
    {
        public string Text { get; set; } = "PUT";
    }
    public class POST : ITypeToken
    {
        public string Text { get; set; } = "POST";
    }
    public class PATCH : ITypeToken
    {
        public string Text { get; set; } = "PATCH";
    }
    public class DELETE : ITypeToken
    {
        public string Text { get; set; } = "DELETE";
    }

}
