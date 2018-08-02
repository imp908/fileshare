using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace tNine
{
  
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
        char[] GetCommand(char[] str_);
    }
    public interface IKeyPad
    {
        HashSet<IKey> keys { get; }

    }



    public struct Label : ILabel
    {
        public Label(int order_, char[] arr_) { this.order = order_; this.arr = arr_; }
        public int order { get; }
        public char[] arr { get; set; }

        public char[] GetCommand()
        {

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
            StringBuilder sb = new StringBuilder();
            foreach (char ch_ in this.arr)
            {
                sb.Append(ch_);
            }
            return sb.ToString().GetHashCode();
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
        public Key() { }
        public Key(IButton button_, ILabel label_)
        {
            this.button = button_; this.label.TryAdd(label_.GetHashCode(), label_);
        }
        public Key(IButton button_, List<ILabel> label_)
        {
            this.button = button_;
            labelBind(label_);
        }

        public IButton button { get; set; }
        public Dictionary<int, ILabel> label { get; set; } = new Dictionary<int, ILabel>();
        public ILabel GetLabel(char[] str_)
        {

            ILabel lb = null;
            StringBuilder sb = new StringBuilder();
            foreach (char ch_ in str_)
            {
                sb.Append(ch_);
            }
            int hs = sb.ToString().GetHashCode();
            label.TryGetValue(hs, out lb);
            if (lb != null)
            {
                return lb;
            }

            return null;
        }
        public char[] GetCommand(char[] str_)
        {
            ILabel lb = this.GetLabel(str_);
            char[] arr = new char[lb.order * this.button.Name.Count()];
            int len = this.button.Name.Count();

            if (lb != null)
            {
                for (int i = 0; i < lb.order; i++)
                {
                    this.button.Name.CopyTo(arr, i * len);
                }
                return arr;
            }

            return null;
        }
        public void labelBind(IEnumerable<ILabel> label_)
        {
            foreach (ILabel lb_ in label_)
            {
                this.label.Add(lb_.GetHashCode(), lb_);
            }
        }

    }

    public class KeyPad : IKeyPad
    {
        public KeyPad() { }
        public KeyPad(HashSet<IKey> keys_) { this.keys = keys_; }

        public HashSet<IKey> keys { get; set; }

        public char[] command(char[] input_)
        {
            IKey k = (from s in keys where s.GetLabel(input_) != null select s).FirstOrDefault();
            return k.GetCommand(input_);
        }
        public char[] command(char input_)
        {
            char[] ip_ = new char[] { input_ };
            return command(ip_);
        }

        public string command(string input_)
        {
            char[] char_ = input_.ToCharArray();
            char[] found = null;
            char[] previous = null;

            StringBuilder sb = new StringBuilder();
            foreach (char ch_ in char_)
            {
                found = command(ch_);
                if (previous != null)
                {
                    if (found[0] == previous[0])
                    {
                        sb.Append(" ");
                    }
                }
                sb.Append(found);
                previous = found;
            }
            return sb.ToString();
        }
    }


    public class KeyFactory
    {
        public Key CharKey(string btName_,List<char[]> labels_)
        {
            //future logging goes here
            if (btName_ == null) { throw new NullReferenceException(); }
            if (labels_ == null) { throw new NullReferenceException(); }

            Button bt = new Button(btName_);
            List<ILabel> lb = new List<ILabel>();
            Key key = new Key();
            for (int i=1;i<=labels_.Count();i++)
            {
                lb.Add(new Label(i, labels_[i-1]));
            }
            key.button = bt;
            key.labelBind(lb);

            return key;
        }
    }

    public class KeyPadfactory
    {
        public KeyPad KeyPadTNine(HashSet<IKey> keys_)
        {
            //future logging goes here
            if (keys_ == null) { throw new NullReferenceException(); }

            KeyPad kp = new KeyPad(keys_);
            return kp;
        }
    }

}
