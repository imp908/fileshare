using System;
using System.Collections.Generic;
using System.Text;

namespace tNine
{    

    class tNine
    {

    }

    public interface IToken
    {
        //token printed order times too immitate button multiple clicks
        int order { get; }

        char[] arr { get; set; }
        string Value();
        string ToString();
    }
    public interface IButton
    {
        string Name { get; set; }
    }
    public interface IKey
    {
        IButton button { get; set; }
        Dictionary<int,IToken> signatures { get; set; }
        char[] command(string str_);
    }   
    public interface IKeyPad
    {
        List<IKey> keys { get; }
    }



    public struct Token : IToken
    {
        public Token(int order_, char[] arr_) { this.order = order_; this.arr = arr_; }
        public int order { get; }
        public char[] arr { get; set; }

        public string Value() { return string.Join(string.Empty,this.arr); }
        public override string ToString() { return this.Value(); }
    }
    public class Button : IButton
    {
        public Button() { }
        public Button(string name_) { this.Name = name_; }
        public string Name { get; set; }        
    }
    public class Key : IKey
    {
        public Key(){}
        public Key(IButton button_,IToken tk_) {           
            this.signatures.TryAdd(tk_.Value().GetHashCode(), tk_);
        }
        public Key(IButton button_, List<IToken> signs_)
        {
            foreach (IToken tk_ in signs_)
            {
                try{
                    this.signatures.TryAdd(tk_.Value().GetHashCode(), tk_);
                }catch(ArgumentNullException e) { }
                catch (ArgumentException e) { }
            }
        }

        public IButton button { get; set; }
        public Dictionary<int, IToken> signatures { get; set; } = new Dictionary<int, IToken>();      

        public char[] command(IToken tk)
        {
            return command(tk.Value());
        }
        public char[] command(string str_){
            List<char[]> ret = new List<char[]>();
            char[] ch_1 = str_.ToCharArray();
            char[] ch_2 = str_.ToCharArray();
            int hs1 = ch_1.GetHashCode();
            int hs2 = ch_2.GetHashCode();

            if (signatures.ContainsKey(1))
            {
                IToken tk=signatures.GetValueOrDefault(str_.GetHashCode());
                for(int i = 0; i < tk.order; i++)
                {
                    ret.Add(tk.arr);                    
                }
                return tk.arr;
            }
           
            return null;
        }
    }
    public class KeyPad : IKeyPad
    {
        public KeyPad() { }
        public KeyPad(List<IKey> keys_) { this.keys = keys_; }

        public List<IKey> keys { get;}

        public string command(string input_,string split)
        {
            StringBuilder sb = new StringBuilder();
            string[] str = input_.Split(split);
            char[] previous = null;
            char[] current = null;

            foreach(string str_ in str)
            {
                foreach(IKey key in this.keys)
                {
                    current = key.command(str_);

                    if (previous != null) {                        
                        if (previous == current)
                        {
                            sb.Append(string.Empty);
                        }
                    }
                    else { previous = current; }

                    sb.Append(key.command(str_));
                }

                return sb.ToString();
            }
            return null;
        }

    }

}
