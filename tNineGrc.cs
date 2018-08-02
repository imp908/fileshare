using System;
using System.Collections.Generic;
using System.Text;

namespace tNine
{

    public interface ILabels<T> where T:struct
    {
        T[] arr { get; set; }
        int GetHashCode();
    }

    public interface Icount
    {
        int cnt { get; set; }
    }
    public interface IKeyItem<T> where T:struct
    {
        Dictionary<ILabels<T>,ILabels<T>> Items { get; set; }
    }

    public interface IButtonName<T> : ILabels<T> , Icount where T : struct
    {

    }




    //base class 
    //can be abstract if intense 
    public class Button<T> : ILabels<T> where T:struct 
    {
        public Button() { }
        public Button(T[] arr_)
        { this.arr = new T[arr_.Length]; arr_.CopyTo(this.arr, arr_.Length-1); }
        public T[] arr { get; set; }
        
        public override int GetHashCode()
        {
            StringBuilder sb = new StringBuilder();

            foreach(T ch_ in this.arr)
            {
                sb.Append(ch_);
            }
            
            //resource intensive but only at initial time
            //can be further replaced with plugin to startup read
            return sb.ToString().GetHashCode();
        }
    }

    public class ButtonName<T> : IButtonName<T> where T : struct
    {
        public ButtonName() { }
        public ButtonName(T[] arr_, int count_)
        { this.arr = new T[arr_.Length]; arr_.CopyTo(this.arr, arr_.Length - 1); this.cnt = count_; }

        public T[] arr { get; set; }
        public int cnt { get; set; }
    }
    public class Buttonlabel<T> : Button<T> where T: struct
    {

        public Buttonlabel() { }
        public Buttonlabel(T[] arr_) : base(arr_) { }
    }

    public class KeyItem<T> : IKeyItem<T>where T:struct
    {
        public Dictionary<ILabels<T>, ILabels<T>> Items { get; set; }
      
    }
 
    public interface IKeyItems<T> where T : struct
    {        
        List<T[]> command(List<T[]> input_);
        IKeyItems<T> command(string input_);
        string commandToString();
    }




    public class KeyItemsChar : IKeyItems<char>
    {      
        public KeyItemsChar() { }

        List<char[]> commandResult = new List<char[]>();

        public Dictionary<int, char[]> buttonLabels { get; set; } = new Dictionary<int, char[]>();
        public Dictionary<int, int> buttonCounts { get; set; } = new Dictionary<int, int>();

        public void addItem(ILabels<char> label_, IButtonName<char> button_)
        {
            int hs = label_.GetHashCode();
            try
            {
                this.buttonLabels.TryAdd(hs, button_.arr);
                this.buttonCounts.TryAdd(hs, button_.cnt);
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        public List<char[]> command (List<char[]> input_)
        {
            List<char[]> chArr = new List<char[]>();
            char[] prev=new char[0];
            char[] found;
            int? count=null;

            foreach (char[] arr_ in input_){
           
                found = this.buttonLabels.GetValueOrDefault(ArrayToHash(arr_));
                if (prev[0]==found[0])
                {
                    chArr.Add(new char[] { ' ' });
                }
                if (found != null)
                {
                    count=this.buttonCounts.GetValueOrDefault(ArrayToHash(arr_));
                    if (count != null)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            chArr.Add(found);
                        }
                        
                    }
                    prev = found;
                }
            }

            return chArr;
        }

        public IKeyItems<char> command(string input_)
        {
            this.commandResult = new List<char[]>();
            char[] prev = new char[0];
            char[] found;
            int? count = null;

            foreach (char arr_ in input_)
            {
                
                found = this.buttonLabels.GetValueOrDefault(ArrayToHash(new char[] { arr_ }));

                if (found != null)
                {

                    if (prev.Length>0)
                    {
                        if (prev[0] == found[0])
                        {
                            this.commandResult.Add(new char[] { ' ' });
                        }
                    }

                    count = this.buttonCounts.GetValueOrDefault(ArrayToHash(new char[] { arr_ }));
                    if (count != null)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            this.commandResult.Add(found);
                        }
                    }
                    prev = found;
                }
                found = null;
            }

            return this;
        }
        public string commandToString()
        {
            StringBuilder sb = new StringBuilder();
                foreach(char[] arr_ in this.commandResult)
                {
                    foreach(char ch_ in arr_)
                    {
                        sb.Append(ch_);
                    }
                }
            return sb.ToString();
        }
        public int ArrayToHash(char[] arr_)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch_ in arr_)
            {
                sb.Append(ch_);
            }
            return sb.ToString().GetHashCode();
        }
    }
   

    public static class KeyItemsFactory
    {

        public static KeyItemsChar keysChar() {
            KeyItemsChar res = new KeyItemsChar();
                res.addItem(new Buttonlabel<char>(new char[] { 'a'}), new ButtonName<char>(new char[] { '2' },1));
                res.addItem(new Buttonlabel<char>(new char[] { 'b' }), new ButtonName<char>(new char[] { '2' },2));
                res.addItem(new Buttonlabel<char>(new char[] { 'c' }), new ButtonName<char>(new char[] { '2' },3));
                res.addItem(new Buttonlabel<char>(new char[] { 'd' }), new ButtonName<char>(new char[] { '3' },1));
                res.addItem(new Buttonlabel<char>(new char[] { 'e' }), new ButtonName<char>(new char[] { '3' },2));
                res.addItem(new Buttonlabel<char>(new char[] { 'f' }), new ButtonName<char>(new char[] { '3' },3));
                res.addItem(new Buttonlabel<char>(new char[] { ' ' }), new ButtonName<char>(new char[] { '0' },1));
            return res;
        }

    }

}
