using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Threading.Tasks;

using System.Threading;
using System.Reflection;
using System.Diagnostics;

using System.IO;

using System.ServiceProcess;

using System.Security.Cryptography;

/*
using Newtonsoft.Json;

using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;
using System.Configuration.Install;

using System.Web.Http;
//install-package Microsoft.AspNet.WebApi.SelfHost
using System.Web.Http.SelfHost;

//install-package Microsoft.AspNet.Mvc 
//using System.Web.Mvc;
using System.Web.Routing;
*/

using System.ComponentModel;


using System.Net;
using System.Net.Sockets;


namespace SBcr_
{

    public static class check {
        public static void GO() {
            //OverrideCheck.GO();
            //GenericSwapCheck.GO();
            DelegateCheck.GO();
            //GenericvalueItemsCHeck.GO();
        }
    }

 
    //overriding    

    public class parent {

        public int ID { get; set; }
        public parent() { }
        public virtual string printV() { return "printed from base " + this.GetType().Name; }
        public string log() { return "logged from base " + this.GetType().Name; }
    }
    class child1 : parent {
        public child1() { }
        public override string printV() { return "printed from child1 " + this.GetType().Name; }
        public new string log() { return "logged from child1 " + this.GetType().Name; }
    }
    class child2 : parent
    {
        public child2() { }
        public new string printV() { return "printed from child2 " + this.GetType().Name; }
        public new string log() { return "logged from child2 " + this.GetType().Name; }
    }
    public static class OverrideCheck{
        public static void GO() {
            parent parent = new parent();
            parent parentAsChild1 = new child1();
            parent parentAsChild2 = new child2();
            child1 child = new child1();
            

            Console.WriteLine(parent.printV()); //base
            Console.WriteLine(parent.log()); //base
            Console.WriteLine(parentAsChild1.printV()); //child1
            Console.WriteLine(parentAsChild1.log()); //base
            Console.WriteLine(parentAsChild2.printV()); //base
            Console.WriteLine(parentAsChild2.log()); //base
            Console.WriteLine(child.printV()); //child1
            Console.WriteLine(child.log()); //child1
        }
    }





    //generic delegate swap 

    public static class SwapG
    {
        public static void Sort<T>(List<T> arr, Func<T, T, bool> cmpr) where T: parent
        {
            bool sort = true;
            while (sort)
            {
                sort = false;
                for (int i = 0; i < arr.Count()-1; i++)
                {
                    if (cmpr(arr[i], arr[i + 1]))
                    {
                        sort = true;
                        SwapG.swap<T>(arr,arr.IndexOf(arr[i]), arr.IndexOf(arr[i + 1]));
                    }
                }
            }
        }

        static void swap<T>(List<T> arr,int i1,int i2)
        {
            T item;
            item = arr[i1];
            arr[i1] = arr[i2];
            arr[i2]= item;
        }
        
        
    }
    static class Comparers
    {
        public static bool desc<T>(T itm1, T itm2) where T : parent
        {
            if (itm1.ID < itm2.ID) { return true; }
            return false;
        }
        public static bool asc<T>(T itm1, T itm2) where T : parent
        {
            if (itm1.ID > itm2.ID) { return true; }
            return false;
        }
    }
    public static class GenericSwapCheck {
        public static void GO() {

            List<parent> arr = new List<parent>(){
                new parent(){ID=0},new parent(){ID=3},new parent(){ID=5},new parent(){ID=2},new parent(){ID=1}
            };

            Console.WriteLine("before swap:");
            foreach(parent p in arr){Console.Write(p.ID);}
            Console.WriteLine();

            SwapG.Sort<parent>(arr, Comparers.desc<parent>);

            Console.WriteLine("after swap desc:");
            foreach (parent p in arr){Console.Write(p.ID);}
            Console.WriteLine();

            SwapG.Sort<parent>(arr, Comparers.asc<parent>);

            Console.WriteLine("after swap asc:");
            foreach (parent p in arr){Console.Write(p.ID);}
            Console.WriteLine();

        }
    }


   
    //delegate

    public delegate string Del1(int i);

    public class DelegateInvokation
    {
        public static void GO()
        {
            //named method instance
            Del1 d11 = print;
            Console.WriteLine(d11.Invoke(2));
            Console.WriteLine(d11(3));

            //anonimous method instance
            Del1 d12 = delegate (int i) { return "Anonimous to str: " + i.ToString(); };
            Console.WriteLine(d12.Invoke(4));
            Console.WriteLine(d12(5));

            //lambda instance
            Del1 d13 = s => "Lambd to str:" + s.ToString();
            Console.WriteLine(d13.Invoke(6));
            Console.WriteLine(d13(7));

        }

        static string print(int i)
        {
            return "Int of str:" + i.ToString();
        }


    }


    public class DelegateReceiver
    {
        Del1 delHandler;
        
        public void RegisterDel(Del1 del_)
        {
            this.delHandler += del_;
        }
        public void UnregisterDel(Del1 del_)
        {
            this.delHandler -= del_;
        }

        public void Fire(int i)
        {
            this.delHandler.Invoke(i);
        }
    }


   
    public class DelegateEmitter {
        public void GO()
        {

            DelegateReceiver du = new DelegateReceiver();      
            du.RegisterDel(print2);
            du.RegisterDel(print3);

            du.Fire(2);
            du.UnregisterDel(print2);
            du.Fire(3);
        }
        string print2(int i)
        {
            string ret= "Print2 of str:" + i.ToString();
            Console.WriteLine(ret);
            return ret;
        }
        string print3(int i)
        {
            string ret = "Print3 of str:" + i.ToString();
            Console.WriteLine(ret);
            return ret;
        }
    }

    public static class DelegateCheck
    {
        public static void GO()
        {
            DelegateInvokation.GO();

            DelegateEmitter dr = new DelegateEmitter();
            dr.GO();
        }
      
    }

   



    //generic value classes

    public interface IvalueNode<T>
    {
        T item { get; set; }
    }
    public interface IvalueArray<T>
    {
        IvalueNode<T>[] arr { get; set; }
    }
    

    public class ValueNode<T> : IvalueNode<T>
    {
        public T item { get; set; }
        public ValueNode<T> Init(T val_) { this.item = val_; return this; }
    }
    public class ValueArray<T> : IvalueArray<T>
    {
        public IvalueNode<T>[] arr { get; set; }
        public ValueArray<T> Init(int i) {this.arr = new IvalueNode<T>[i]; return this; }
    }

    public class NodeFactory
    {
        public static IvalueNode<T> CreateNode<T>(T val_)
        {
            return new ValueNode<T>().Init(val_);
        }

        public static ValueArray<T> CreateArray<T>(int len_)
        {     
            return new ValueArray<T>().Init(len_);
        }
    }

    public class GenericvalueItemsCHeck
    {
        public static void GO(){

            IvalueNode<int> n01 = NodeFactory.CreateNode<int>(0);
            IvalueNode<int> n02 = NodeFactory.CreateNode<int>(1);
            IvalueNode<int> n03 = NodeFactory.CreateNode<int>(2);

            IvalueNode<char> c01 = NodeFactory.CreateNode<char>('a');
            IvalueNode<char> c02 = NodeFactory.CreateNode<char>('b');
            IvalueNode<char> c03 = NodeFactory.CreateNode<char>('c');

            IvalueArray<int> v0 = NodeFactory.CreateArray<int>(3);
            IvalueArray<char> v1 = NodeFactory.CreateArray<char>(3);

            v0.arr[0] = n01;
            v0.arr[1] = n02;
            v0.arr[2] = n03;

            v1.arr[0] = c01;
            v1.arr[1] = c02;
            v1.arr[2] = c03;

            int res = 0;
            foreach(var i in v0.arr)
            {
                res += i.item;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var i in v1.arr)
            {
                sb.Append(i.item);
            }

            bool b0=v0.arr[1].Equals(n02);
            bool b1=v1.arr[0].Equals(c01);

            bool b3 = res == 3;
            bool b4 = sb.ToString() == "abc";

        }

       
    }



    //hash from different collections compare check

    public static class HashCodeCheck{
		public static void GO(){			

			//all return different hashes 

			char[] ch1 = new char[] { 'a' };
			char[] ch2 = new char[] { 'a' };
			string s1 = string.Join("", ch1);
			string s2 = string.Join("", ch2);
			int h1 = ch1.GetHashCode();
			int h2 = ch2.GetHashCode();
			int h21 = s1.GetHashCode();
			int h22 = s2.GetHashCode();

			byte[] bt1 = Encoding.UTF8.GetBytes(ch1);
			byte[] bt2 = Encoding.UTF8.GetBytes(ch2);
			int h31 = bt1.GetHashCode();
			int h32 = bt2.GetHashCode();

			using (MD5 m = MD5.Create())
			{
				byte[] h41 = m.ComputeHash(bt1);
				byte[] h42 = m.ComputeHash(bt2);
			}

		}
	}



}
