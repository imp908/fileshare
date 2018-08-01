using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace tNine
{    

    class tNine
    {

    }

    public interface ILabel
    {
        //token printed order times too immitate button multiple clicks
        int order { get; }

        char[] arr { get; set; }
        char[] GetCommand();
        int GetHashCode();
    }
    public interface IButton
    {
        char[] Name { get; set; }
    }
    public interface IKey
    {
        IButton button { get; set; }
        Dictionary<int, ILabel> label { get; set; }
        ILabel GetLabel(char[] str_);
        List<char> GetCommand(char[] str_);
    }   
    public interface IKeyPad
    {
        List<IKey> keys { get; }
       
    }



    public struct Label : ILabel
    {
        public Label(int order_, char[] arr_) { this.order = order_; this.arr = arr_; }
        public int order { get; }
        public char[] arr { get; set; }      

        public char[] GetCommand() {

            char[] ch = new char[this.arr.Length * this.order];
            for (int i = 1; i <= this.order; i++)
            {
                this.arr.CopyTo(ch, this.arr.Length * (i - 1));
            }

            return ch;
        }
        public override int GetHashCode()
        {            
            //return ((IStructuralEquatable)this.arr).GetHashCode(EqualityComparer<int>.Default);
            return this.arr.GetHashCode();
        }
    }
    public class Button : IButton
    {
        public Button() { }
        public Button(string name_) { this.Name = name_.ToCharArray(); }
        public char[] Name { get; set; }        
    }
    public class Key : IKey
    {
        public Key(){}
        public Key(IButton button_,ILabel label_) {
            this.button = button_;this.label.TryAdd(label_.GetHashCode(),label_);
        }
        public Key(IButton button_, List<ILabel> label_)
        {
            this.button = button_;
            foreach (ILabel lb_ in label_)
            {
                 this.label.Add(lb_.GetHashCode(), lb_);
            }
        }

        public IButton button { get; set; }
        public Dictionary<int,ILabel> label { get; set; } = new Dictionary<int, ILabel>();
        public ILabel GetLabel(char[] str_) {

            ILabel lb = null;
            int hs = str_.GetHashCode();
            var a = this.label;

            return null;
        }
        public List<char> GetCommand(char[] str_)
        {
            ILabel lb = this.GetLabel(str_);
            List<char> arr = new List<char>();
            if (lb != null)
            {
                for(int i = 1; i <= lb.order; i++)
                {
                    arr.AddRange(this.button.Name);
                }
                return arr;
            }

            return null;
        }
    }

    public class KeyPad : IKeyPad
    {
        public KeyPad() { }
        public KeyPad(List<IKey> keys_) { this.keys = keys_; }

        public List<IKey> keys { get; set; }

        public char[] command(char input_)
        {
            char[] ip_ = new char[] { input_ };
            return ip_;
        }
        public char[] command(string input_)
        {
            List<char> arr = new List<char>();
            foreach (char ch_ in input_)
            {
                arr.AddRange(this.command(ch_));
            }
            return arr.ToArray();
        }
    }

}
