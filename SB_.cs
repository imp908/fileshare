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



namespace SB_
{

    public static class check {
        public static void GO() {
            //OverrideCheck.GO();
            //GenericSwapCheck.GO();
            DelCheck.GO();
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

    public static class DelCheck
    {
        public static void GO()
        {
            parent p = new parent();           

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
            return "Int ot str:" + i.ToString();
        }

    }

}
