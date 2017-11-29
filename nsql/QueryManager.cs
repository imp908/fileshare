using System.Collections.Generic;
using System.Linq;
using System;

using IQueryManagers;


using OrientRealization;

namespace QueryManagers
{

    //<<<
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

    /// <summary>
    ///  Token for storing Resulted build strings (URLs, commands e.t.c).
    ///  Can be used to manually pass commands to managers.
    /// </summary>
    public class TextToken : ITypeToken
    {
        public string Text { get; set; }
    }

    /// <summary>
    /// Token factory for OrientDb command builders.
    /// </summary>
    public class TokenMiniFactory : ITokenMiniFactory
    {

        public ITypeToken NewToken()
        {
            return new TextToken();
        }
        public ITypeToken NewEmptyString()
        {
            return new TextToken() { Text = string.Empty };
        }

        public ITypeToken Dot()
        {
            return new OrientDotToken();
        }
        public ITypeToken Coma()
        {
            return new OrientCommaToken();
        }
        public ITypeToken Gap()
        {
            return new OrientGapToken();
        }

    }    

    public class CommandFactory : ICommandFactory
    {
        public ICommandBuilder CommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_)
        {
            return new CommandBuilder(tokenFactory_, formatFactory_);
        }
     
    }
    public class FormatFactory : IFormatFactory
    {
        public IFormatFromListGenerator FormatGenerator(ITokenMiniFactory tokkenFactory_)
        {
            return new FormatFromListGenerator(tokkenFactory_);
        }
    }

    ///<summary> Base class for url tokens concatenation
    ///CommandBuilder realization for Format placeholders for URL concatenation
    ///</summary>
    public class CommandBuilder : ICommandBuilder
    {

        public IFormatFromListGenerator formatGenerator { get; set; }
        public IFormatFactory _formatFactory { get; set; }

        public ITypeToken typeToken { get; set; }
        public ITypeToken Text { get; set; }
        public ITypeToken FormatPattern { get; set; }
        public List<ITypeToken> Tokens { get; set; }
       
        public CommandBuilder(ITokenMiniFactory tokenFactory_,IFormatFactory formatFactory_)
        {
            this._formatFactory = formatFactory_;
            formatGenerator = formatFactory_.FormatGenerator(tokenFactory_);
        }
        public CommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_
            ,List<ITypeToken> tokens_, ITypeToken format_)
        {
            this._formatFactory = formatFactory_;
            formatGenerator = formatFactory_.FormatGenerator(tokenFactory_);
            AddTokens(tokens_);
            AddFormat(format_);
        }
     
        //cocatenates URLbuilders Token collections from URLbuilders with format pattern
        public CommandBuilder(List<ICommandBuilder> texts_, ITypeToken FormatPattern_)
        {
            this.FormatPattern = FormatPattern_;

            TokenFormatConcatenation(texts_, FormatPattern_);

            //build new string
            SetText(this.Tokens, this.FormatPattern);            

        }
        public CommandBuilder(List<ITypeToken> tokens_, IFormatFromListGenerator formatGenerator_)
        {            
            this.formatGenerator = formatGenerator_;
            this.FormatPattern.Text +=this.formatGenerator.FormatFromListGenerate(this.Tokens).Text;
            if (this.Tokens == null)
            {
                this.Tokens = tokens_;
            }
            else { this.Tokens.AddRange(tokens_); }         
            SetText(this.Tokens, this.FormatPattern);
        }
        public CommandBuilder(List<ICommandBuilder> texts_, IFormatFromListGenerator formatGenerator_)
        {

            this.formatGenerator = formatGenerator_;
            this.FormatPattern.Text += this.formatGenerator.FormatFromListGenerate(this.Tokens).Text;
            if (this.Tokens == null)
            {
                this.Tokens = new List<ITypeToken>();
               
            }
            foreach (ICommandBuilder cb in texts_)
            {
                foreach (ITypeToken tp in cb.Tokens)
                {
                    this.Tokens.Add(tp);
                }
            }

            TokenFormatConcatenation(texts_, this.FormatPattern);

            //build new string
            SetText(this.Tokens, this.FormatPattern);

        }

        public CommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_
          , List<ICommandBuilder> command_, ITypeToken format_)
        {
            this._formatFactory = formatFactory_;
            formatGenerator = formatFactory_.FormatGenerator(tokenFactory_);
            TokenFormatConcatenation(command_, format_);       
        }

        public void BindTokens(List<ITypeToken> tokens_)
        {
            this.Tokens = new List<ITypeToken>();
            this.Tokens.AddRange(tokens_);
        }
        public void AddTokens(List<ITypeToken> tokens_)
        {
            if (this.Tokens == null){          
                this.Tokens = new List<ITypeToken>();              
            }
            this.Tokens.AddRange(tokens_);
        }
        public void BindFormat(ITypeToken formatPatern_)
        {        
            this.FormatPattern = formatPatern_;           
            this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
        }
        public void AddFormat(ITypeToken formatPatern_)
        {
            if (this.FormatPattern != null)
            {
                this.FormatPattern.Text += formatPatern_.Text;
            }
            //<<< change
            else { this.FormatPattern = formatPatern_; }
            this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
        }
        public void BindFormatGenerator(IFormatFromListGenerator formatGenerator_)
        {
            this.formatGenerator = formatGenerator_;
        }
        public void AddBuilders(List<ICommandBuilder> texts_, ITypeToken FormatPattern_)
        {            
            TokenFormatConcatenation(texts_, FormatPattern_);
        }

        public void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {
            List<string> str = new List<string>();
            this.FormatPattern = FormatPattern_;
            this.Tokens = tokens_;
            if (this.Tokens != null && this.FormatPattern != null) {
            
                foreach (ITypeToken tt in this.Tokens)
                {
                    if (tt != null) { str.Add(tt.Text); }
                    else { str.Add(null); }
                }

                try
                {
                    this.Text = new TextToken()
                    {

                        Text = string.Format(this.FormatPattern.Text, str.ToArray())

                    };
                }
                catch (Exception e) { }

            }
        }
        public string GetText()
        {
            try
            {
                Build();
            }
            catch (Exception e) { throw e; }

            return this.Text.Text;
        }
        public string Build()
        {
            if (this.Tokens == null) { throw new Exception("No tokens"); }
            CheckFormat();
            SetText(this.Tokens, this.FormatPattern);
            return GetText();
        }
        ///<summary>Defined NESTED, concatenates every command according to it format. 
        ///Results concatenated according to passed FormatPettern. 
        ///FULL concatenates all tokens from all commandBuilders according to new passed patterformat</summary>
        public string Build(List<ICommandBuilder> texts_, ITypeToken FormatPattern_)
        {

            this.FormatPattern = FormatPattern_;
            this.Tokens = new List<ITypeToken>();
                 
            List<ITypeToken> str = new List<ITypeToken>();
            foreach (ICommandBuilder tb in texts_)
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
            
            return GetText();
        }

        public enum BuildTypeFormates { FULL, NESTED }

        //recounts format string parameters from 0 for concatenated from several format strings
        public string FormatStringReArrange(string input_)
        {
            string result = string.Empty;
            List<char> input_chars = input_.ToCharArray().ToList();
            int i = 0, i2 = 0;
            List<char> chPrev = new List<char>();
            List<char> chCur = new List<char>();
            int intPrev = 0;
            int intCur = 0;
            char[] chToInsert=null;

            for (i = 0; i < input_chars.Count; i++)
            {
              
                bool multiDigit = false;
                i2 = i;

                //if digit is met
                if (char.IsDigit(input_chars[i]))
                {
                    //check length of multidigit
                    while (char.IsDigit(input_chars[i2 + 1]))
                    {
                        multiDigit = true;
                        if ((i2 + 1) < input_chars.Count)
                        {
                            i2 += 1;
                        }
                        else { break; }
                    }

                    //rem number if first encounter
                    if (chPrev.Count == 0)
                    {
                        for (int i3 = i; i3 <= i2; i3++)
                        {
                            chPrev.Add(input_chars[i3]);
                        }
                        int.TryParse(chCur.ToString(), out intCur);
                        if (intCur != 0) {
                            intCur = 0;

                            for (int i3 = i; i3 <= i2; i3++)
                            {
                                input_chars.RemoveAt(i);
                            }

                            chToInsert = intCur.ToString().ToCharArray();

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

                        }
                    }
                    else
                    {//second didgit and so on
                        for (int i3 = i; i3 <= i2; i3++)
                        {
                            chCur.Add(input_chars[i3]);
                        }

                        int.TryParse(chCur.ToString(), out intCur);
                        int.TryParse(chPrev.ToString(), out intPrev);

                        if ((intPrev+1)!= intCur)
                        {//if digits not sequenced by +1
                            intCur = intPrev + 1;
                            
                            //rewrite
                            for (int i3 = i; i3 <= i2; i3++)
                            {
                                input_chars.RemoveAt(i);
                            }
                            multiDigit = false;

                            chToInsert = intCur.ToString().ToCharArray();

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

                        }
                        //swap prev to cur
                        chPrev.Clear();
                        chPrev.AddRange(chCur);
                        chCur.Clear();
                    }

                 
                }
            }

            return result = string.Concat(input_chars);
        }

        internal void TokenFormatConcatenation(List<ICommandBuilder> texts_, ITypeToken FormatPattern_=null)
        {

            List<ITypeToken> tempTokens = new List<ITypeToken>();
            List<ITypeToken> str = new List<ITypeToken>();
            string newFromat = null;

            foreach (ICommandBuilder tb in texts_)
            {
                if (tb.FormatPattern == null)
                {
                    tb.FormatPattern = formatGenerator.FormatFromListGenerate(tb.Tokens); 
                }
                //build string
                tb.SetText(tb.Tokens, tb.FormatPattern);
                //add tokens to list
                tempTokens.AddRange(tb.Tokens);
                //concatenate formats according to new, nested format
                str.Add(tb.FormatPattern);
                               
            }

            //concatenating of all formatpatterns
            string[] arr = (from s in str select s.Text).ToArray();

            this.Tokens = tempTokens;

            if (FormatPattern_==null)
            {
                if (this.FormatPattern==null || this.FormatPattern.Text == null)
                {
                    //try to generate if no format passed
                    try
                    {
                        if (formatGenerator == null) { throw new Exception("No ForamtPattern no FormatGenerator not binded"); }
                        FormatPattern = formatGenerator.FormatFromListGenerate(this.Tokens);
                    }
                    catch (Exception e) { }
                }             
            }
            else
            {
                this.FormatPattern = FormatPattern_;
            }
            try
            {
                newFromat = FormatStringReArrange(string.Format(this.FormatPattern.Text, arr));
                this.Tokens = tempTokens;
                this.FormatPattern.Text = newFromat;
            }
            catch (Exception e) { }
            
        }
     
        internal void CheckFormat()
        {
            if (this.Tokens == null) { throw new Exception("No tokens passed"); }

            if (this.FormatPattern != null)
            {
                //rearrange
                this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
            }
            else
            {
                if (formatGenerator == null) { throw new Exception("No format pattern or generator passed"); }
                typeToken = formatGenerator.FormatFromListGenerate(this.Tokens);
            }
        }

    }

    /// <summary>
    /// Genrates sample format from collection of tokens. 
    /// If delimeter passed uses delimeter , if not uses default empty string
    /// </summary>
    public class FormatFromListGenerator : IFormatFromListGenerator
    {
        ITokenMiniFactory _factory;
        public FormatFromListGenerator(ITokenMiniFactory factory)
        {
            this._factory = factory;
        }

        public ITypeToken FormatFromListGenerate(List<ITypeToken> tokens)
        {
            string res = "{}";
            int[] cnt = new int[tokens.Count];
            for (int i = 0; i < tokens.Count(); i++)
            {
                cnt[i] = i;
            }
            string res2 = string.Join(@"} {", cnt);
            ITypeToken token = this._factory.NewToken();
            token.Text = res.Insert(1, res2);
            return token;
        }
        public ITypeToken FormatFromListGenerate(List<ITypeToken> tokens, string delimeter = null)
        {
            string res = "{}";
            string placeholder;
            if (delimeter != null)
            {
                placeholder = string.Join("}", delimeter, "{");
            }
            else { placeholder = @"} {"; }
            int[] cnt = new int[tokens.Count];
            for (int i = 0; i < tokens.Count(); i++)
            {
                cnt[i] = i;
            }
            string res2 = string.Join(placeholder, cnt);
            ITypeToken token = this._factory.NewToken();
            token.Text = res.Insert(1, res2);
            return token;
        }
        public ITypeToken FormatFromListGenerate<T>(List<T> items, string delimeter = null)
            where T : class
        {
            string res = "{}";
            string placeholder;
            if (delimeter != null)
            {
                placeholder = string.Join("}", delimeter, "{");
            }
            else { placeholder = @"} {"; }
            int[] cnt = new int[items.Count];
            for (int i = 0; i < items.Count(); i++)
            {
                cnt[i] = i;
            }
            string res2 = string.Join(placeholder, cnt);
            ITypeToken token = this._factory.NewToken();
            token.Text = res.Insert(1, res2);
            return token;
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
