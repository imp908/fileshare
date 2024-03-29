﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Reflection;
using System.Diagnostics;

using System.IO;

using Newtonsoft.Json;

using System.ServiceProcess;

using System.ServiceModel;
using System.ServiceModel.Activities;
using System.ServiceModel.Description;

using System.ComponentModel;
using System.Configuration.Install;

using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

using System.Web.Http;
//install-package Microsoft.AspNet.WebApi.SelfHost
using System.Web.Http.SelfHost;

//install-package Microsoft.AspNet.Mvc 
//using System.Web.Mvc;
using System.Web.Routing;

//custom linq
using System.Linq.Expressions;

namespace SBsh_
{

    #region TipsAndTricks
    public static class TnT
    {
        public static void Check()
        {
            //equality();
            StructsCompare();
        }

        public static void equality()
        {
            string a = null;
            //->throws System.NullReferenceException
            a.Equals(null);
        }

        public struct Str1
        {
            public string a;
            public int b;
        }
        public struct Str2
        {
            public string a;
            public int b;
        }
        public static void StructsCompare()
        {
            Str1 str1 = new Str1() { a = "Str1", b = 5 };
            Str1 str2 = str1;
            //true
            bool equalsResult = str1.Equals(str2);
            //false
            bool referenceResult = Object.ReferenceEquals(str1, str2);

        }

    }

    #endregion

    #region EqualityCheck
    public interface Ieq1 { int ID { get; set; } }

    public class Eq1: Ieq1
    {
        public Eq1() { ID = 0;name = "Eq1 nm1"; }

        public int ID { get; set; }
        public string name{ get; set; }

        public virtual string print() { return "printing from base"; }
        public string printhide() { return "hided in base"; }
    }
    public class Eq2
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public class Eq3 : Eq1
    {
        public Eq3() { ID = 0; name = "Eq3 nm1"; }

        public override string print() { return "printing from child"; }
    }
    public class Eq4 : Eq1
    {
        public Eq4() { ID = 0; name = "Eq4 nm1"; }

        public new string print() { return "return from child for real"; }
        public new string printhide() { return "hided in child"; }
    }
    public static class EqualityCheck{
        public static void GO()
        {
            Eq1 eq00 = new Eq1();
            Eq2 eq01 = new Eq2();
            Eq3 eq3 = new Eq3();
            Eq1 eq31 = new Eq3();

            Type t0 = eq00.GetType();
            Type t1 = typeof(Eq1);
            bool b00=t0.Equals(t1);

            bool b0 = eq3 is Eq1;
            bool b1 = eq3 is Eq3;
            bool b2 = eq31 is Eq1;
            bool b3 = eq31 is Eq3;

            Eq1 eq312=eq3 as Eq1;
            Type t2 = eq312.GetType();
            bool b4 = eq00.Equals(eq01);
            bool b5 = eq3.Equals(eq31);
            bool b6 = eq3.Equals(eq312);

            string str0 = eq3.print();
            string str1 = eq312.print();

            Eq1 eq41 = new Eq4();
            Eq1 eq412 = new Eq4() as Eq1;
            string str2 = eq41.print();
            string str3 = eq412.print();
            bool b7 = eq3.Equals(eq312);

            string str4=eq41.printhide();
            string str5=eq412.printhide();

            bool bI0 = eq00 is Ieq1;
            bool bI1=eq00.GetType().GetInterface(typeof(Ieq1).Name)!=null;
        }
    }
    #endregion 

    //Language
    #region Overriding
    //sample override
    public static class OverridingCheck
    {
        public static void Check()
        {
            //BaseCheck()
            CarCheck();
        }

        internal static void BaseCheck()
        {
            BaseClass bc = new BaseClass();
            DereivedClass dc = new DereivedClass();
            BaseClass bcdc = new DereivedClass();

            bc.Method1();
            bc.Method2();
            dc.Method1();
            dc.Method2();
            bcdc.Method1();
            bcdc.Method2();
        }

        internal static void CarCheck()
        {
            Car car = new Car();
            Car convcar = new ConvCar();
            Car extcar = new ExtCar();

            car.Info();
            convcar.Info();
            extcar.Info();

            ConvCar vc = new ConvCar();
            ExtCar ec = new ExtCar();
            vc.Info();
            ec.Info();
        }
    }

    public class BaseClass
    {
        public virtual void Method1()
        {
            System.Diagnostics.Trace.WriteLine(@"Base Method 1");
        }
        public virtual void Method2()
        {
            System.Diagnostics.Trace.WriteLine(@"Base Method 2");
        }
    }
    public class DereivedClass : BaseClass
    {
        public override void Method1()
        {
            System.Diagnostics.Trace.WriteLine(@"Dereived Method 1");
        }
        public new void Method2()
        {
            System.Diagnostics.Trace.WriteLine(@"Dereived Method 2");
        }
    }


    public class Car
    {
        public void Info()
        {
            System.Diagnostics.Trace.WriteLine(@"Car info:");
            Details();
        }
        public virtual void Details()
        {
            System.Diagnostics.Trace.WriteLine(@"Standart car:");
        }
    }
    public class ConvCar : Car
    {
        public new void Details()
        {
            System.Diagnostics.Trace.WriteLine(@"Conv car:");
        }
    }
    public class ExtCar : Car
    {
        public override void Details()
        {
            System.Diagnostics.Trace.WriteLine(@"Ext car:");
        }
    }
    #endregion

    #region genericMethod

    /// <summary>
    /// Generic methods with generic delegate for universal list sorting
    /// receives array and boolean method to compare item1 to item2
    /// </summary>
    static class SB
    {
        public static void Sort<T>(List<T> array, Func<T, T, bool> compare)
        {
            bool swap = true;
            T item;

            while (swap)
            {
                swap = false;
                for (int i = 0; i < array.Count() - 1; i++)
                {
                    if (compare(array[i], array[i + 1]))
                    {
                        item = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = item;
                        swap = true;
                    }
                }
            }
        }
        static void Swap<T>(ref T c1, ref T c2)
        {
            T item;
            item = c2;
            c2 = c1;
            c1 = item;
        }
    }

    /// <summary>
    /// class for sorting example with init and print methods
    /// </summary>
    public class Item
    {
        public List<Item> ListOfItems { get; private set; }

        internal string Field1 { get; set; }
        internal int Field2 { get; set; }
        public void ItemsInit()
        {
            ListOfItems = new List<Item>() {
                new Item { Name = "A", ID = 4},   new Item { Name = "B", ID = 2 },   new Item { Name = "C", ID =3 },   new Item { Name = "D", ID = 1}
            };
        }
        public string Name { get; set; }
        public int ID { get; set; }

        public bool itemsCompare(Item c1, Item c2)
        {
            bool result = false;
            if (c1.ID > c2.ID)
            {
                result = true;
            }
            return result;
        }

        public void itemsPrint()
        {
            foreach (Item item_ in this.ListOfItems)
            {
                System.Diagnostics.Debug.WriteLine("Item ID:{0} , item Name:{1}", item_.ID, item_.Name);
            }
        }
    }

    #endregion

    #region events
    //event class,emmiter,listener and test classes
    public class CarEventHandler : EventArgs
    {
        internal string CarName { get; set; }
        public CarEventHandler(string carname_)
        {
            this.CarName = carname_;
        }
    }

    public class EventPublisher
    {

        public event EventHandler<CarEventHandler> carevent;

        public void CarAdd(string name_)
        {
            Emmit(new CarEventHandler(name_));
        }

        public void Emmit(CarEventHandler e)
        {
            EventHandler<CarEventHandler> handler = carevent;

            if (handler != null)
            {
                e.CarName += " evented";
                System.Diagnostics.Debug.WriteLine("Event emmited");
                handler(this, e);
            }
        }
    }

    public class EventSubscriber
    {
        public string name { get; set; }
        public EventSubscriber(string name_)
        {
            this.name = name_;
        }
        public void subscribe(EventPublisher ep)
        {
            ep.carevent += EventReceiver;
        }

        public void EventReceiver(object sender, CarEventHandler e)
        {
            System.Diagnostics.Debug.WriteLine("Event received");
            System.Diagnostics.Debug.WriteLine("Event received with {0} , from {1}", e.CarName, this.name);
        }
    }

    public static class EventsCheck
    {
        public static void checkCarEvent()
        {
            EventPublisher ep = new EventPublisher();
            EventSubscriber es = new EventSubscriber("subscriber 1");
            EventSubscriber es2 = new EventSubscriber("subscriber 2");
            es.subscribe(ep);
            es2.subscribe(ep);
            ep.CarAdd("name_1");

        }
    }

    public static class EventtToDelegate
    {
        public static event GetString event1;
        public static void EventFromDelFire()
        {
            event1 += StringOutput;
            event1(@"aaa");
        }
        public static void StringOutput(string str_)
        {
            Console.WriteLine(str_);
        }
    }

    #endregion

    #region Interfaces
    //Explicit Initialization
    interface IPrintInt
    {
        void Print(int i);
    }
    interface IPrintDouble
    {
        void Print(Double i);
    }

    public class PrintIntDouble : IPrintInt, IPrintDouble
    {
        void IPrintInt.Print(int i) { Console.WriteLine(i); }
        void IPrintDouble.Print(double i) { Console.WriteLine(i); }
    }
    static class ExplicitInterfacesCheck
    {
        public static void Check()
        {
            IPrintDouble pri = (IPrintDouble)new PrintIntDouble();
            IPrintInt prd = (IPrintInt)new PrintIntDouble();

            pri.Print(0.321);
            prd.Print(123);
        }
    }

    //Indexer
    interface IIndexer_
    {
        int this[int index] { get; set; }
    }
    class Indexer_ : IIndexer_
    {
        int[] arr = new int[100];

        public int this[int index]
        {
            get { return arr[index]; }
            set { this.arr[index] = value; }
        }
    }
    public static class IndexerInterfaceCheck
    {
        public static void Check()
        {
            Indexer_ ind = new Indexer_();
            ind[0] = 1;
            Console.WriteLine(ind[0]);
        }
    }
    #endregion

    #region delegates

    /// <summary>
    /// delegates invokation check   
    /// 
    /// </summary>

    public delegate void GetString(string input);

    public static class DelegateCheck
    {

        static Helper helper = new Helper();

        public delegate void del1();
        public delegate void del11();
        public delegate string del2();

        public delegate string del4(string input);

        public delegate void OutputDel(string message);
        public delegate void ValueCheck(int i);
        public delegate bool ValueCheckAnon(int i);

        internal static del1 del = delMeth1;
        internal static del11 del11_ = delMeth1_;
        internal static del2 del2_ = delMeth2;
        internal static GetString del3_ = delMeth3;

        internal static OutputDel outputDel_;

        public static void Check()
        {
            delegatesInvokation();
            delegatesAction();
            delegateArray();
            OutputDelCheck();
        }

        static void delMeth1()
        {
            System.Diagnostics.Debug.WriteLine("Del 1 fired");
        }
        static void delMeth1_()
        {
            System.Diagnostics.Debug.WriteLine("Del 1_ fired");
        }
        static string delMeth2()
        {
            return " del2 output;";
        }
        static void delMeth3(string input_)
        {
            helper.cout("Del3 inputed " + input_);
        }
        static string delMeth4(string input_)
        {
            return " del3 output for inputed " + input_;
        }


        static void ConsoleOutput(string message_)
        {
            Console.WriteLine(message_);
            System.Diagnostics.Trace.WriteLine(@"Console output:" + message_);
        }
        static void Traceoutput(string message_)
        {
            System.Diagnostics.Trace.WriteLine(@"Trace output:" + message_);
        }
        public static void OutputDelAssign(OutputDel del)
        {
            outputDel_ += del;
        }
        public static void OutputDelInvoke(ValueCheck del_)
        {
            Random rnd = new Random();
            int a = rnd.Next(0, 10);
            int b = a - 5;
            del_.Invoke(b);
        }
        public static void OutputDelAnon(ValueCheckAnon del)
        {
            Random rnd = new Random();
            int a = rnd.Next(0, 10);
            int b = a - 5;
            if (del(b))
            {
                outputDel_.Invoke(b.ToString() + " condition is true");
            }
            else
            {
                outputDel_.Invoke(b.ToString() + " condition is false");
            }
        }
        public static void OutputDelCheck()
        {
            OutputDelAssign(ConsoleOutput);
            OutputDelAnon(s => s < 5);
            OutputDelAnon(s => s % 2 == 0);

            OutputDelInvoke(CompareToFive);
            OutputDelAssign(Traceoutput);
            OutputDelInvoke(EvennesCheck);

        }


        internal static void CompareToFive(int b)
        {
            if (b < 5)
            {
                outputDel_.Invoke(@"value " + b.ToString() + " less than 5");
            }
            else
            {
                outputDel_.Invoke(@"value " + b.ToString() + " more than 5");
            }
        }
        internal static void EvennesCheck(int b)
        {
            if (b % 2 == 0)
            {
                outputDel_.Invoke(@"value " + b.ToString() + " % 2 = 0");
            }
            else
            {
                outputDel_.Invoke(@"value " + b.ToString() + " % 2 = " + (b % 2).ToString(@"0.00"));
            }
        }

        //simple invokation
        public static void delegatesInvokation()
        {
            helper.cout("Delegates invoked");
            del.Invoke();

            helper.cout(del2_());
            helper.cout(del2_.Invoke());

            del3_.Invoke("del 3 input");
        }
        //invokation with action
        public static void delegatesAction()
        {
            helper.cout("Delegate actions invoked");
            Action act = delMeth1;
            act += delMeth1_;
            act.Invoke();
        }
        //invokation from array
        public static void delegateArray()
        {
            helper.cout("Delegates array");
            del1[] delarr = { delMeth1, delMeth1_ };
            foreach (del1 del_ in delarr)
            {
                del_.Invoke();
            }
        }


    }


    public static class DelegatesCheck
    {
        public static void Check()
        {
            Delegates _delegates = new Delegates();
            _delegates.DelegatesBind();
            _delegates.DelegatesInvoke();
        }
    }
    //new structural delegates
    public class Delegates
    {
		 //delegate type declaration
        delegate void GetStringDel(string i);
        //variables of delegate type declaratoin
        GetStringDel PrintString;
        GetStringDel PrintString2;
        GetStringDel PrintString3;
        Random rnd = new Random();
        GetStringDel[] arr;
        public Delegates()
        {

        }

        public void DelegatesBind()
        {
            //delegate initializations
            //with method name		  
            PrintString = PrintStringA;

            //#2.0 anonimous init
            PrintString2 = delegate (string i) { System.Diagnostics.Trace.WriteLine(@"Anonimous init for: " + i); };

            //#3.0 lambda
            PrintString3 = (x) => { System.Diagnostics.Trace.WriteLine(@"Lambda init for: " + x); };

            //change method order at runtime from Rand values
            arr = new GetStringDel[rnd.Next(1, 10)];
            for (int i = 0; i < arr.Count(); i++)
            {
                int val = rnd.Next(1, 1000);
                if (val % 2 == 0)
                {
                    arr[i] = PrintString;
                }
                if (val % 3 == 0)
                {
                    arr[i] = PrintString2;
                }
                if (val % 5 == 0)
                {
                    arr[i] = PrintString3;
                }
                if (arr[i] == null)
                {
                    arr[i] = PrintString3;
                }
            }
        }
        public void DelegatesInvoke()
        {
            PrintString3.Invoke(@"Invoke");

            for (int i = 0; i < arr.Count(); i++)
            {
                arr[i].Invoke(" invoked from Arr in position " + i);
            }
        }
        public void PrintStringA(string i)
        {
            System.Diagnostics.Trace.WriteLine(" Method init for  : " + @" " + i);
        }

    }
    #endregion

    #region Event
    public class CreateEvent : EventArgs
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Emitter
    {
        int ID { get; set; }
        string Name { get; set; }

        public event EventHandler<CreateEvent> _handler;
        public Emitter()
        {

        }
        public void Add(int id_, string name_)
        {
            CreateEvent event_ = new CreateEvent();
            this.ID = id_;
            this.Name = name_;
            event_.ID = id_;
            event_.Name = Name;
            OnCreateEvent(event_);
        }

        protected virtual void OnCreateEvent(CreateEvent e)
        {
            EventHandler<CreateEvent> handler_ = _handler;
            if (handler_ != null)
            {
                handler_(this, e);
            }
        }

    }
    public class Receiver
    {
        public void ReceiveEvent(object sender, CreateEvent e)
        {
            Console.WriteLine(@"Event raised for sender: " + sender + @"; e: " + e.ID + @" " + e.Name);
        }
    }
    public static class EventsCheck_2
    {
        public static void Check()
        {
            Receiver rc = new Receiver();
            Emitter em = new Emitter();
            em._handler += rc.ReceiveEvent;
            em.Add(1, @"Added");
        }
    }

    #endregion

    #region bubblesort
    public static class BubbleSort
    {
        public static void BubblesortCheck()
        {
            List<Item> itemList = new List<Item>()
            {
                new Item {Field1="D",Field2= 5 }
                ,new Item {Field1="B",Field2= 2 }
                ,new Item {Field1="C",Field2= 3 }
                ,new Item {Field1="A",Field2= 7 }
            };

            cmpDel<Item> compareDelegate = Compare<Item>;

            //BubbleSort.Sort<Item_>(itemList, BubbleSort.Compare, 0 );
            BubbleSort.Sort<Item>(itemList, compareDelegate);
        }
        public static void Sort<T>(List<T> arr, Func<T, T, bool> compare_, int i_) where T : Item
        {
            bool swapped = false;
            do
            {
                swapped = false;
                for (int i = 0; i < arr.Count() - 1; i++)
                {

                    if (compare_(arr[i], arr[i + 1]))
                    {
                        Swap<T>(arr, i);
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        public static void Sort<T>(List<T> arr, cmpDel<T> compare) where T : Item
        {
            bool swapped = false;
            do
            {
                swapped = false;
                for (int i = 0; i < arr.Count() - 1; i++)
                {

                    if (compare(arr[i], arr[i + 1]))
                    {
                        Swap<T>(arr, i);
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        public static void Swap<T>(List<T> arr, int num) where T : Item
        {
            T item;
            item = arr[num];
            arr[num] = arr[num + 1];
            arr[num + 1] = item;
        }
        public static bool Compare<T>(T item1, T item2) where T : Item
        {
            bool result_ = false;
            if (item1.Field2 > item2.Field2)
            {
                result_ = true;
            }
            return result_;
        }


        public delegate bool cmpDel<T>(T item1, T item2);



    }

    #endregion

    #region LinkedList
    public class Document
    {
        internal string Title { get; set; }
        string Content { get; set; }
        internal byte Priority { get; set; }

        public Document(string title, string content, byte priority)
        {
            this.Title = title;
            this.Content = content;
            this.Priority = priority;
        }

    }
    public class PriorityDocumentManager
    {
        internal readonly LinkedList<Document> documentList;
        internal readonly List<LinkedListNode<Document>> nodeList;

        public PriorityDocumentManager()
        {
            documentList = new LinkedList<Document>();

            nodeList = new List<LinkedListNode<Document>>(10);

            for (int i = 0; i < 10; i++)
            {
                nodeList.Add(new LinkedListNode<Document>(null));
            }
        }

        public void AddDocument(Document doc)
        {
            if (doc == null) throw new ArgumentNullException("doc");

            AddDocumentToPriorityNode(doc, doc.Priority);
        }

        public void AddDocumentToPriorityNode(Document document, int priority)
        {

            if (priority > 9 || priority < 0)
            {
                throw new ArgumentException("Priority no in 1-9 range");
            }

            if (nodeList[priority].Value == null)
            {
                --priority;
                if (priority >= 0)
                {
                    AddDocumentToPriorityNode(document, priority);
                }
                else
                {
                    documentList.AddLast(document);
                    nodeList[document.Priority] = documentList.Last;
                }
                return;
            }
            else
            {
                LinkedListNode<Document> prioNode = nodeList[priority];
                if (priority == document.Priority)
                {
                    documentList.AddAfter(prioNode, document);
                    nodeList[document.Priority] = prioNode.Next;
                }
                else
                {
                    LinkedListNode<Document> firstPrioNode = prioNode;
                    while (firstPrioNode.Previous != null &&
                        firstPrioNode.Previous.Value.Priority == prioNode.Value.Priority)
                    {
                        firstPrioNode = prioNode.Previous;
                        prioNode = firstPrioNode;
                    }

                    documentList.AddBefore(firstPrioNode, document);

                    nodeList[document.Priority] = firstPrioNode.Previous;
                }

            }
        }

        public void DisplayAllNodes()
        {
            foreach (Document doc in documentList)
            {
                System.Diagnostics.Debug.WriteLine("Document: {0}, with priority: {1}", doc.Title, doc.Priority);
            }
        }

        public static void NodesInit()
        {
            var pdm = new PriorityDocumentManager();

            pdm.AddDocument(new Document("One", "sample", 9));
            pdm.AddDocument(new Document("Two", "sample", 7));
            pdm.AddDocument(new Document("Three", "sample", 9));
            pdm.AddDocument(new Document("Four", "sample", 7));

            pdm.DisplayAllNodes();
        }
    }
    public class LinkedListSwitch
    {
        public LinkedListSwitch()
        {
            string[] words = { "A", "B", "C", "D", "E" };
            LinkedList<string> sentence = new LinkedList<string>(words);

            display(sentence);
            LinkedListNode<string> node = sentence.Find("C");

            sentence.AddAfter(node, "C2");
            sentence.AddBefore(node, "C0");

            node = sentence.Find("C");
            sentence.Remove(node);
            sentence.AddFirst(node);

            node = sentence.Find("C0");
            LinkedListNode<string> nodep = node.Previous;
            LinkedListNode<string> noden = node.Next;

            sentence.Remove(node.Previous);
            sentence.Remove(node.Next);
            sentence.Remove(node);

            sentence.AddFirst(node);
            sentence.AddFirst(nodep);
            sentence.AddFirst(noden);

            display(sentence);

        }

        public void display(LinkedList<string> ls)
        {
            foreach (string st in ls)
            {
                System.Diagnostics.Debug.WriteLine(st);
            }
            System.Diagnostics.Debug.WriteLine("-----");
        }
        public void displayNode(LinkedListNode<string> node_)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(node_.Value);
                System.Diagnostics.Debug.WriteLine(node_.Previous.Value);
                System.Diagnostics.Debug.WriteLine(node_.Next.Value);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.Write(e.Message);
            }
        }
    }

    public static class LinkedLists
    {
        public static void Check()
        {
            PriorityDocumentManager.NodesInit();

            LinkedListSwitch ls = new LinkedListSwitch();
        }
    }
    #endregion

    #region LINQ
    public class Racer
    {
        public string Name { get; set; }
        public string Sername { get; set; }
        public string Car { get; set; }
        public int Year { get; set; }
    }
    public class Cup
    {
        public string Competition { get; set; }
        public int Year { get; set; }
        public int Position { get; set; }
        public string RacerName { get; set; }
    }
    public class PetOwners
    {
        public string OwnerName { get; set; }
        public int OwnerInt { get; set; }
        public List<string> Pets { get; set; }
    }
    public class LINQ_play
    {
        public static List<Racer> racers = new List<Racer>();
        public static List<Cup> cups = new List<Cup>();

        public LINQ_play()
        {

            racers.Add(new Racer { Name = @"Racer1", Sername = @"sername1", Year = 1990, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer2", Sername = @"sername2", Year = 1991, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer3", Sername = @"sername3", Year = 1990, Car = @"car2" });
            racers.Add(new Racer { Name = @"Racer4", Sername = @"sername4", Year = 1990, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer5", Sername = @"sername5", Year = 1990, Car = @"car2" });
            racers.Add(new Racer { Name = @"Racer6", Sername = @"sername6", Year = 1990, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer7", Sername = @"sername7", Year = 1991, Car = @"car1" });
            racers.Add(new Racer { Name = @"Racer8", Sername = @"sername8", Year = 1991, Car = @"car3" });
            racers.Add(new Racer { Name = @"Racer9", Sername = @"sername9", Year = 1991, Car = @"car1" });

            cups.Add(new Cup { Competition = @"Cup1", Year = 1990, Position = 1, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup1", Year = 1990, Position = 2, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 1, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 2, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup2", Year = 1991, Position = 3, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 1, RacerName = @"Racer4" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 2, RacerName = @"Racer5" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 3, RacerName = @"Racer3" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 4, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup3", Year = 1992, Position = 5, RacerName = @"Racer7" });
            cups.Add(new Cup { Competition = @"Cup4", Year = 1989, Position = 1, RacerName = @"Racer1" });
            cups.Add(new Cup { Competition = @"Cup5", Year = 1988, Position = 1, RacerName = @"Racer2" });
            cups.Add(new Cup { Competition = @"Cup6", Year = 1985, Position = 1, RacerName = @"Racer10" });
            cups.Add(new Cup { Competition = @"Cup7", Year = 1991, Position = 1, RacerName = @"Racer11" });


            var a = (from s in cups select s.RacerName).Intersect(from r in racers select r.Name);
            var b = (from r in racers select r.Name).Except(from s in cups select s.RacerName);
            var c = (from s in cups select new { s.RacerName, s.Position }).Zip(from r in racers select new { r.Car }, (z, x) => (z.RacerName + x.Car));

            List<PetOwners> petowners = new List<PetOwners>() {
                new PetOwners {OwnerName=@"Owner1",OwnerInt=0,Pets= new List<string>() { @"Pet1",@"Pet2"} },
                new PetOwners {OwnerName=@"Owner2",OwnerInt=1,Pets= new List<string>() { @"Pet3",@"Pet4", @"Pet5" } },
                new PetOwners {OwnerName=@"Owner3",OwnerInt=2,Pets= new List<string>() { @"Pet6"} },
                new PetOwners {OwnerName=@"Owner4",OwnerInt=3,Pets= new List<string>() { @"Pet7",@"Pet8", @"Pet9", @"Pet10" } }
            };

            var d =
                from t in petowners
                from t2 in t.Pets
                select t2;

            var e = petowners.SelectMany(s => s.Pets);

            //group by several columns
            var f =
                from t in cups
                group t by new { t.Competition, t.Year } into t2
                orderby t2.Key.Year, t2.Key.Competition descending
                select new { Comp = t2.Key.Competition, Racer = t2.Key.Year, Count = t2.Count() };

            var g =
                from t in cups
                group t by t.Competition into t2
                select new
                {
                    Cup = t2.Key,
                    Count = t2.Key.Count(),
                    Racer = from s in t2 select s.RacerName
                };

            //join from different sources
            var h = from s in racers select s;
            var i = from s in cups select s;
            var j =
                (from t in h
                 join t2 in i on new { Name = t.Name } equals new { Name = t2.RacerName }
                 select new
                 {
                     t.Name,
                     t.Car,
                     t2.Competition,
                     t2.Position
                 }).Skip(2 * 2).Take(2);
            ;

            //???
            var k =
                from t in h
                join t2 in i on t.Name equals t2.RacerName into t3
                from t2 in t3.DefaultIfEmpty()
                select new
                {
                    t2.Competition,
                    t2.Position,
                    Name = t == null ? @"" : t.Name,
                    Car = t == null ? @"" : t.Car
                };

            var l =
                from t in racers
                let cnt = t.Name.Count()
                orderby cnt descending, t.Car
                select new
                {
                    Name = t.Car,
                    Count = cnt
                };

            var m =
                from t in racers
                group t by t.Car into r
                select new
                {
                    Car = r.Key,
                    Racers = (from t2 in r select t2.Name).Count()
                };


            Func<string, IEnumerable<Cup>> CarByCup =
                cup => from s in cups where s.Competition == cup select s;

            var n = from s in CarByCup("Cup2").Intersect(CarByCup("Cup3")) select s;
        }

        public void write(List<Racer> racers_)
        {
            System.Diagnostics.Debug.WriteLine(@"\\\");

            StringBuilder sb = new StringBuilder();
            foreach (Racer rc_ in racers_)
            {
                sb.AppendLine(rc_.Name + @";" + rc_.Sername + @";" + rc_.Year + @";" + rc_.Car);
                System.Diagnostics.Debug.Write(sb);
                sb.Clear();
            }

            System.Diagnostics.Debug.WriteLine(@"///");
        }
    }
    #endregion

    #region Async
    public static class DelegateAsyncCheck
    {
        private delegate int AsyncDel(int i);
		private delegate string d(int i);
        internal static void delAsyncCheck_0()
        {
            AsyncDel asyncDel_ = new AsyncDel(DelMethod);

            asyncDel_.BeginInvoke(1,
                cb =>
                {
                    System.Diagnostics.Trace.WriteLine(@"Begin invoked");
                    asyncDel_.EndInvoke(cb);
                }
            , null);
        }
        internal static void delAsyncCheck_1()
        {
            AsyncDel del = new AsyncDel(DelMethod);
            System.Diagnostics.Trace.WriteLine(@"Programm begin invoke");

            //            IAsyncResult res = del.BeginInvoke(1, new AsyncCallback(AsyncResult), @"");
            IAsyncResult res = del.BeginInvoke(1, null, null);

            System.Diagnostics.Trace.WriteLine(@"Programm field");
            System.Diagnostics.Trace.WriteLine(@"Programm end invoke");
            int a = del.EndInvoke(res);
        }
        private static int DelMethod(int i)
        {
            System.Diagnostics.Trace.WriteLine(@"Del method invoked start");
            Thread.Sleep(3000);
            System.Diagnostics.Trace.WriteLine(@"Del method invoked end");
            return i + 1;
        }

        private static void AsyncResult(IAsyncResult res)
        {
            System.Diagnostics.Trace.WriteLine(@"Async result");
        }
    }

    public static class AsyncFoundation
    {

        #region one
        public static void Check_0()
        {
            TraceLog.WriteLn(TraceLog.GetFunctionName());
            CallNotAsync();
            CallStrAsync();
            CallAsync();
        }
        public static void Check_1()
        {
            TraceLog.WriteLn(@"Check start 1");
            CallSequence();
            TraceLog.WriteLn(@"Check finish 1");
            TraceLog.WriteLn(@"Check start 2");
            CallSequence2();
            TraceLog.WriteLn(@"Check finish 2");
            TraceLog.WriteLn(@"Check start 3");
            Task t = CallSequence3();
            TraceLog.WriteLn(@"Check finish 3");
            t.Wait();
        }
        //manage async 0 in sequence4
        public static void Check_4()
        {
            TraceLog.WriteLn(@"Check start");
            Task t = CallSequence4();
            TraceLog.WriteLn(@"Check finish");
            t.Wait();
            TraceLog.WriteLn(@"Wait finish");
        }
        public static void Check_3()
        {
            TraceLog.WriteLn(@"Check started");
            CallAsync();
            TraceLog.WriteLn(@"Check finished");

            TraceLog.WriteLn(@"Check started 2");
            Task t = MethodAsync();
            TraceLog.WriteLn(@"Check wait");
            t.Wait();
            TraceLog.WriteLn(@"Check finished 2");
        }


        private static void CallNotAsync()
        {
            MethodStr(TraceLog.GetFunctionName());
        }
        private static async void CallStrAsync()
        {
            var a = await MethodStrAsync(@"1");
            TraceLog.WriteLn(@"Break line betweenn 1 and 2 code");
            Task<string> str = MethodStrAsync(@"2");
            TraceLog.WriteLn(@"Break line betweenn 2 and 3 code");
            //add await            
            await str.ContinueWith(t =>
            {
                TraceLog.WriteLn(t.Result + @"3");
            }
            );
            TraceLog.WriteLn(@"Break line betweenn 3 and 4 code");

            await Task.Run(() =>
            {
                str.ContinueWith(t =>
                {
                    TraceLog.WriteLn(t.Result + @"4");
                }
                );
            });
        }
        private static async void CallAsync()
        {
            TraceLog.WriteLn(@"async started");
            await MethodAsync();
            TraceLog.WriteLn(@"async finished");
        }


        private static async void CallSequence()
        {
            TraceLog.WriteLn(@"Call start 1");
            string a = await PrintAsync(@"Async 1 run");
            TraceLog.WriteLn(@"Call finish 1");
        }
        private static async void CallSequence2()
        {
            TraceLog.WriteLn(@"Call start 2");
            string a = await PrintAsync(@"Async 2 run");
            TraceLog.WriteLn(@"Call finish 2");
        }
        private static async Task CallSequence3()
        {
            TraceLog.WriteLn(@"Call start 3");
            string a = await PrintAsync(@"Async 3 run");
            TraceLog.WriteLn(@"Call finish 3");
            string b = await PrintAsync(@"Async 3.2 run");
            TraceLog.WriteLn(@"Call finish 3.2");
        }
        //manage async 0 with await
        private static async Task CallSequence4()
        {
            Task t0 = PrintAsync(@"Async 0");


            TraceLog.WriteLn(@"Call start 1");
            string b = await PrintAsync(@"Async 1");
            TraceLog.WriteLn(@"Call finish 1");
            TraceLog.WriteLn(@"Call start 2");
            b = PrintAsync(@"Async 2").GetAwaiter().GetResult();
            TraceLog.WriteLn(@"Call finish 2");
            TraceLog.WriteLn(@"Call start 3");
            b = await Task.Run(() => {
                return PrintAsync(@"Async 3");
            });
            TraceLog.WriteLn(@"Call finish 3");

            t0.Wait();
        }
        private static async Task WaitAll()
        {
            Task<string> t1 = PrintAsync(@"1");
            Task<string> t2 = PrintAsync(@"2");
            Task<string> t3 = PrintAsync(@"3");

            await Task.WhenAll(new[] { t1, t2, t3 });
            TraceLog.WriteLn(@"All");
        }
        private static async Task AllStart()
        {
            Task<string> t1 = WaitSec(2000);
            Task<string> t2 = WaitSec(4000);
            Task<string> t3 = WaitSec(8000);

            await Task.Run(() => { t1.Start(); });

            t2.Start();
            t3.Start();


            //await Task.WhenAll(new[] { t1, t2, t3 });
            TraceLog.WriteLn(@"All" + t1.Result);
        }
        private static async Task StartMethodParallel()
        {
            Parallel.ForEach<string>(new List<string> {
                @"A",@"B",@"C"
            }, WaitSecStr
            );

            Parallel.For(0, 10, WaitSec_);
            await Task.Run(() =>
            {
                Parallel.Invoke(
() => { WaitSec_(1000); }
, () => { WaitSec_(3000); }
, () => { WaitSec_(5000); }
);
            });
        }


        private static string MethodStr(string item_)
        {
            string result_ = item_;
            TraceLog.WriteLn(TraceLog.GetFunctionName() + item_ + @"; Thread name: " + Thread.CurrentThread.Name);
            return result_;
        }
        private static Task<string> MethodStrAsync(string item_)
        {
            string result_ = TraceLog.GetFunctionName();
            return Task<string>.Run(() => {
                TraceLog.WriteLn(result_ + item_ + @"; Thread name: " + Thread.CurrentThread.Name);
                return result_;
            });
        }
        private static Task MethodAsync()
        {
            TraceLog.WriteLn(@"task started");
            Thread.Sleep(3000);
            return Task.Run(() => {
                TraceLog.WriteLn(TraceLog.GetFunctionName() + @"; Thread name: " + Thread.CurrentThread.Name);
            });
        }

        private static Task<string> PrintAsync(string str_)
        {
            string result_ = str_;
            Thread.Sleep(2000);
            return Task.Run(() => {
                TraceLog.WriteLn(str_);
                return result_;
            });
        }
        private static Task<string> WaitSec(int item_ = 500)
        {
            Thread.Sleep(item_);
            return Task.Run(() => { TraceLog.WriteLn(item_.ToString()); return item_.ToString(); });
        }
        private static void WaitSec_(int item_ = 1000)
        {
            Thread.Sleep(item_);
            TraceLog.WriteLn(item_.ToString());
        }
        private static void WaitSecStr(string str_)
        {
            Thread.Sleep(1000);
            TraceLog.WriteLn(str_.ToString());
        }

        #endregion

        #region two

        public static void Check()
        {
            TraceLog.WriteLn(@"Check started");
            MethodAsync_2();
            TraceLog.WriteLn(@"Check continues 1");
            MethodAsync_string();
            TraceLog.WriteLn(@"Check continues 2");
            MethdoAsync_sync();
            TraceLog.WriteLn(@"Check finished");
        }

        //async await methods for Task,Task<string> ,async with Task.Run(()=>{})
        private static async void MethodAsync_2()
        {
            TraceLog.WriteLn(@"method async start");
            await MethodTask();
            TraceLog.WriteLn(@"method async finish");
        }
        private static async void MethodAsync_string()
        {
            TraceLog.WriteLn(@"method async_String start");
            string result_ = await MethodTask_string();
            TraceLog.WriteLn(@"method async_String finish");
        }
        private static async void MethdoAsync_sync()
        {
            TraceLog.WriteLn(@"method sync start");
            Task t = Task.Run(() => {
                MethodSync();
            });
            t.Wait();
            TraceLog.WriteLn(@"method sync finished");

        }


        //tasked methods Task,Task<string> and sync string
        private static Task MethodTask()
        {
            TraceLog.WriteLn(@"task start");
            Thread.Sleep(2000);
            TraceLog.WriteLn(@"task finish");
            return Task.Run(() => {

            });
        }
        private static Task<string> MethodTask_string()
        {
            string result_ = @"";
            TraceLog.WriteLn(@"task string start");
            Thread.Sleep(2000);
            TraceLog.WriteLn(@"task string finish");
            return Task.Run(() => {
                return result_;
            });
        }
        private static string MethodSync()
        {
            string result_ = @"";
            TraceLog.WriteLn(@"Task sync start");
            Thread.Sleep(2000);
            TraceLog.WriteLn(@"Task sync finish");
            return result_;
        }

        #endregion

        public class AsyncDelegates
        {

            public delegate void dAsyncPrint();

            List<int> intList = new List<int>();
            List<string> strList = new List<string>();
            Helper helper = new Helper();
            int capacity;

            public AsyncDelegates(int capacity_)
            {
                capacity = capacity_;
                ListsInit();
            }

            protected void ListIntPrintFor()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--1");
                TraceLog.WriteLn(@"Started ListIntPrintFor in thread : " + Thread.CurrentThread.ManagedThreadId);

                for (int i = 0; i <= capacity; i++)
                {
                    System.Diagnostics.Trace.Write(intList[i] + @" ");
                }
                System.Diagnostics.Trace.WriteLine(@"--END--1");
            }
            protected void ListStrPrintFor()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--2");
                TraceLog.WriteLn(@"Started ListStrPrintFor in thread : " + Thread.CurrentThread.ManagedThreadId);

                for (int i = 0; i <= capacity; i++)
                {
                    System.Diagnostics.Trace.Write(strList[i] + @" ");
                }
                System.Diagnostics.Trace.WriteLine(@"--END--2");
            }
            protected void ListIntPrintWith()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--1");
                TraceLog.WriteLn(@"Started ListIntPrintWith in thread : " + Thread.CurrentThread.ManagedThreadId);

                helper.PrintList<int>(intList);
                System.Diagnostics.Trace.WriteLine(@"--END--1");
            }
            protected void ListStrPrintWith()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--2");
                TraceLog.WriteLn(@"Started ListStrPrintWith in thread : " + Thread.CurrentThread.ManagedThreadId);

                helper.PrintList<string>(strList);
                System.Diagnostics.Trace.WriteLine(@"--END--2");
            }

            protected void ListsInit()
            {
                for (int i = 0; i <= capacity; i++)
                {
                    intList.Add(i);
                    strList.Add(intList[i].GetHashCode().ToString());
                }
            }

            public void PrintForCall()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--0");
                dAsyncPrint dPrint = new dAsyncPrint(ListIntPrintFor);

                IAsyncResult iasrt = dPrint.BeginInvoke(null, dPrint);
                TraceLog.WriteLn(@"Other stuff at thread : " + Thread.CurrentThread.ManagedThreadId);
                ListStrPrintFor();
                dPrint.EndInvoke(iasrt);
                System.Diagnostics.Trace.WriteLine(@"--END--0");

            }
            public void PrintWithCall()
            {
                System.Diagnostics.Trace.WriteLine(@"--ST--0");
                dAsyncPrint dp = new dAsyncPrint(ListIntPrintWith);

                IAsyncResult iasrt = dp.BeginInvoke(null, dp);

                while (!iasrt.IsCompleted)
                {
                    TraceLog.WriteLn(@"State :" + iasrt.AsyncState + " " + Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(100);
                }
                ListStrPrintWith();
                dp.EndInvoke(iasrt);
                System.Diagnostics.Trace.WriteLine(@"--END--0");
            }

            public void Check()
            {
                AsyncDelegates ad = new AsyncDelegates(20);
                ad.PrintForCall();
                System.Diagnostics.Trace.WriteLine(@"-----------");
                ad.PrintWithCall();
            }
        }

    }
	
	public class AsyncCheck
    {
        string ams = @"Async method start";
        string amf = @"Async method final";

        string acs = @"Async call start";
        string acf = @"Async call final";

        string sms = @"sync method start";
        string smf = @"sync method final";

        void SyncMethod()
        {
            TraceLog.Log(sms);
            TraceLog.WriteLn(sms);

            TraceLog.Log(smf);
            TraceLog.WriteLn(smf);
        }
        async Task async_()
        {
            TraceLog.Log(ams);
            TraceLog.WriteLn(ams);
            await Task.Run(()=> Thread.Sleep(1000));
            TraceLog.Log(amf);
            TraceLog.WriteLn(amf);
        }

        public async void AsyncCall()
        {
            TraceLog.Log(acs);
            TraceLog.WriteLn(acs);
            await async_();
            SyncMethod();
            TraceLog.Log(acf);
            TraceLog.WriteLn(acf);        
        
        }

        public void TaskWaitLambda()
        {
            Task t = Task.Run(() => {
                // Just loop.
                long ctr = 0;
                for (ctr = 0; ctr <= 100000000; ctr++)
                { }
                TraceLog.Log(string.Format("Finished {0} loop iterations",ctr));
                TraceLog.WriteLn(string.Format("Finished {0} loop iterations", ctr));
            });
            t.Wait();
        }
    }
    #endregion

    #region Threads

    /// <summary>
    /// class for passing parameters, of list ammount to thread method
    /// </summary>
    public class PrintParams
    {
        public int cpacity { get; set; }
    }

    public class ThreadCheck
    {
        //instance of list generator\printer
        Helper helper = new Helper();

        //object to lock threads
        private object threadLockToken = new object();

        int lkCnt = 0;

        //Methods to fire 
        //sample thread check list print
        public void ThreadSt()
        {
            Thread thr = new Thread( new ThreadStart(helper.PrintListInt));
            thr.Start();
        }
        //Host method for printing methods
        public void ThreadMultipleStart(int ammount)
        {
            Thread[] threads = new Thread[ammount];            

          
            for (int i = 0; i < ammount; i++)
            {
                threads[i] = new Thread(new ThreadStart(ThreadSleepPrintMonitor));
            }


            for (int i = 0; i < ammount; i++)
            {
                threads[i].Start();
            }
            
        }
        //Host method multiple threads with parameter
        public void ThreadMultipleStartParam(int ammount)
        {
            Thread[] threads = new Thread[ammount];
            PrintParams pp = new PrintParams() { cpacity = 10 };            

            for (int i = 0; i < ammount; i++)
            {
                threads[i] = new Thread(PrintSleepParam);
            }


            for (int i = 0; i < ammount; i++)
            {
                threads[i].Start(pp);
            }

        }       

        public void ThreadInterlock()
        {
            int nums = 5;
            int trhs = 4;

            helper.intListSampleInit(nums);
            Thread[] threads = new Thread[trhs];

            for(int i =0;i< trhs; i++)
            {
                threads[i] = new Thread(ThreadSleepPrintInterlock);
            }

            for (int i = 0; i < trhs; i++)
            {
                threads[i].Start();
                System.Diagnostics.Trace.WriteLine(@"Loop value -> " + lkCnt);
            }
           
        }


        //host methods to pass params or instantiate delegates
        private void PrintParam(object print_)
        {
            PrintParams pp = null;

            if(print_ is PrintParams)
            {
                pp = (PrintParams)print_;
                helper.intListSampleInit(pp.cpacity);
                helper.PrintListInt(pp.cpacity);
            }
        }
        public void ThreadStParam(int capacity_)
        {
            helper.PrintListInt(capacity_);
            helper.intListSampleInit(capacity_);
            PrintParams pp = new PrintParams() { cpacity = 10 };
            Thread thr = new Thread(PrintParam);
            thr.Name = "Thread_0";
            thr.Start(pp);
        }
        //target method for parametrized Thread with parameter class
        public void PrintSleepParam(object capacity_)
        {
            PrintParams pp = null;
            if(capacity_ is PrintParams)
            {
                pp = (PrintParams)capacity_;
            }
            ThreadSleepPrintParam(pp);
        }

        //Methods to Lock
        //satart printing 
        public void ThreadSleepPrintLock()
        {
            helper.intListSampleInit(10);
            //bad style
            lock (this)
            {
                for (int i = 0; i < helper.GetIntList().Count(); i++)
                {
                    int sleep_ = helper.r.Next(10) * 100;
                    Thread.Sleep(sleep_);
                    System.Diagnostics.Trace.WriteLine(
                        "Thread :" + Thread.CurrentThread.ManagedThreadId
                        + @" slept :" + sleep_
                        + " Value -> " + helper.GetIntList()[i] + ";"
                        );
                }
            }
        }
        //satart printing with params
        public void ThreadSleepPrintParam(PrintParams capacity_)
        {
            helper.intListSampleInit(capacity_.cpacity);
            //bad style locking
            //lock (this)

            lock (threadLockToken)
            {
                for (int i = 0; i < capacity_.cpacity; i++)
                {
                    int sleep_ = helper.r.Next(10) * 100;
                    Thread.Sleep(sleep_);
                    System.Diagnostics.Trace.WriteLine(
                        "Thread :" + Thread.CurrentThread.ManagedThreadId
                        + @" slept :" + sleep_
                        + " Value -> " + helper.GetIntList()[i] + ";"
                        );
                }
            }
        }
        //lock with monitor syntax explicitly
        public void ThreadSleepPrintMonitor()
        {
            helper.intListSampleInit(10);
            //good style
            Monitor.Enter(threadLockToken);
            try{                
                for (int i = 0; i < helper.GetIntList().Count(); i++)
                {
                    int sleep_ = helper.r.Next(10) * 100;
                    Thread.Sleep(sleep_);
                    System.Diagnostics.Trace.WriteLine(
                        "Thread monitor:" + Thread.CurrentThread.ManagedThreadId
                        + @" slept :" + sleep_
                        + " Value -> " + helper.GetIntList()[i] + ";"
                        );
                }
            
            }catch(Exception e)
            {
System.Diagnostics.Trace.WriteLine(e.Message);
            }
            finally
            {
                Monitor.Exit(threadLockToken);
            }
        }
        public void ThreadSleepPrintInterlock()
        {
            
            for (int i = 0; i < helper.GetIntList().Count(); i++)
            {
                Interlocked.Add(ref lkCnt, 1);
                //lkCnt += 1;
                System.Diagnostics.Trace.WriteLine(@"Locked value -> " + lkCnt);
            }
        }



    }
    
    #endregion

    #region Parallel
    public static class ParallelTasks
    {
        static System.Diagnostics.Stopwatch watch = new Stopwatch();
        //wait for parallel task
        public static void Check_0()
        {
            Task task = new Task(PT);
            Task task2 = new Task(PT2);
            TraceLog.WriteLn(@"Check start");
            task.Start();
            task2.Start();
            task.Wait();
            TraceLog.WriteLn(@"Check finish");
        }
        public static void Check()
        {
            //Task[] tasks = new Task[4] { new Task(PT), new Task(PT2), new Task(PT2) , new Task(PT) };
            Task[] tasks = new Task[3] { new Task(PT3), new Task(PT3), new Task(PT3) };
            //Task[] tasks = new Task[3] { new Task(PT4), new Task(PT5), new Task(PT6) };
            watch.Start();
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Start();
            }

            Task.WaitAll(tasks);
            watch.Stop();
            TraceLog.WriteLn("Whole elapsed: " + watch.Elapsed);
        }


        private static void PT()
        {

            TraceLog.WriteLn(@"PT1 start: " + watch.Elapsed);
            Thread.Sleep(2500);
            TraceLog.WriteLn(@"Excecuting PT1; " + watch.Elapsed);
            TraceLog.WriteLn(@"PT1 finish; " + watch.Elapsed);
        }
        private static void PT2()
        {
            TraceLog.WriteLn(@"PT2 start; " + watch.Elapsed);
            Thread.Sleep(5000);
            TraceLog.WriteLn(@"Excecuting PT2; " + watch.Elapsed);
            TraceLog.WriteLn(@"PT2 finish; " + watch.Elapsed);
        }
        private static void PT3()
        {
            TraceLog.ConsumeCPUcount(6000, 10000);
        }


        private static void PT4()
        {
            Thread.Sleep(2000);
        }
        private static void PT5()
        {
            Thread.Sleep(4000);
        }
        private static void PT6()
        {
            Thread.Sleep(8000);
        }
    }

    public class ParallelFor
    {
        List<int> list = new List<int>();
        private int capacity = 0;

        public ParallelFor(int capacity_)
        {
            capacity = capacity_;
            ListInit();
        }            
        public void GO()
        {
            //ParallelForeachPrint();            
            ParallelInvoke();

            ForeachPrint();
        }

        private void ListInit()
        {
            for (int i = 0; i <= capacity; i++)
            {
                list.Add(i);
            }
        }
        private void ParallelForeachPrint()
        {
            Parallel.ForEach(list, (item) => {                
                System.Diagnostics.Trace.Write(item + ",");
            });
        }
        private void ParallelInvoke()
        {
            Parallel.Invoke(
                () => {
                    ForeachPrint();
                }
                );
        }
        private void ForeachPrint()
        {
           foreach(int item in list)
            {
                System.Diagnostics.Trace.Write(item + ",");
            };
        }

    }

    #endregion

    #region Reflection
    public static class Reflections_
    {
        public static void Check()
        {
            InstantiateAssemblyType();
        }
        public static void InstantiateAssemblyType()
        {
            Assembly asm = Assembly.GetCallingAssembly();
            var types_ = asm.GetTypes();
            var types_filtered = (from s in types_ where s.IsAbstract == false && s.IsSealed == false && s.Name == @"EMPLOYEES" select s).ToList();

            foreach (var b in types_filtered)
            {
                //defining non static type
                if (b.IsClass && b.IsAbstract == false && b.IsSealed == false)
                {
                    var c = Activator.CreateInstance(b);
                }
            }
        }
    }


    //loop throught list of class class properties
    //read property custom attribute
    //sample parameter class
    public class Parameters
    {
        public Parameters()
        {

        }

        [New(AttrType = true)]
        public string STR0 { get; set; }
        public string STR1 { get; set; }
        public string STR2 { get; set; }

        public void Check()
        {
            Parameters parameter = new Parameters();
            List<Parameters> parametersL = new List<Parameters> {
                new Parameters { STR0="A",STR1="B",STR2="C"},
                new Parameters{ STR0 = "A1", STR1 = "B2", STR2 = "C2" }
            };



            Type type = typeof(Parameters);
            PropertyInfo[] b = type.GetProperties();

            foreach (PropertyInfo c in b)
            {
                IEnumerable<Attribute> pa = c.GetCustomAttributes();
                var ca = c.GetCustomAttributes(typeof(NewAttribute), false).FirstOrDefault();

                foreach (Attribute v in pa)
                {
                    NewAttribute na = v as NewAttribute;
                    bool val = (bool)na.AttrType;
                }

                System.Diagnostics.Trace.WriteLine(c.Name);
                foreach (Parameters parameter_ in parametersL)
                {
                    var d = parameter_.GetType().GetProperty(c.Name).GetValue(parameter_);
                }
            }
        }
    }
    //custom attribute class
    public class NewAttribute : Attribute
    {
        public bool AttrType { get; set; }
    }
    #endregion

    #region IoC

    //Factory Method
    public interface IStringGiver
    {
        string Name { get; }
    }

    public class NameHaver : IStringGiver
    {
        public string Name { get; set; }
        public NameHaver(string Name_)
        {
            Name = Name_;
        }
    }
    public class NameContainer
    {
        public IStringGiver IntContVar;

        public NameContainer()
        {
            InterfaceInit();
        }
        protected virtual void InterfaceInit()
        {
            IntContVar = IinterfaceForIocCreate();
        }
        protected virtual IStringGiver IinterfaceForIocCreate()
        {
            string Name_ = @"1";
            IStringGiver _result = new NameHaver(Name_);
            return _result;
        }
    }
    public class SernameContainer : NameContainer
    {
        new public IStringGiver IntContVar;

        public SernameContainer()
        {
            InterfaceInit();
        }
        protected override void InterfaceInit()
        {
            this.IntContVar = IinterfaceForIocCreate();
        }
        protected override IStringGiver IinterfaceForIocCreate()
        {
            string Name_ = @"2";
            IStringGiver _result = new NameHaver(Name_);
            return _result;
        }
    }

    public class DIcheck
    {
        public DIcheck()
        {
            NameContainer nc = new NameContainer();
            SernameContainer snc = new SernameContainer();
            Console.WriteLine(@"Name Container value :" + nc.IntContVar.Name
            + @"; SernameContainer value = :" + snc.IntContVar.Name);
            System.Diagnostics.Trace.WriteLine(@"Name Container value :" + nc.IntContVar.Name
            + @"; SernameContainer value = :" + snc.IntContVar.Name);
        }
    }
    public static class IoC_check
    {
        public static void Check()
        {
            DIcheck di = new DIcheck();
        }
    }

    #endregion

    #region WCF

    public class WCF_
    {

        public WCF_()
        {
            Check();
        }

        public void Check()
        {
            try
            {
                //StoreLog.InterfaceBind(new LogParams());
                //StoreLog.Log(@"TestService started");
                TestService.Run(new TestService());
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        public void WCFcheck()
        {
            //using (ServiceHost serviceHost = new ServiceHost();           
            WorkflowService service = new WorkflowService();
            Uri address = new Uri(@"http://localhost:8080/Sample");
            ServiceHost host = new ServiceHost(typeof(WCFTarget), address);
            //WorkflowServiceHost host = new WorkflowServiceHost(service, address);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();

            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);
            host.AddServiceEndpoint(typeof(IWCFService), new WSHttpBinding(), "");

            try
            {
                host.Open();
                System.Diagnostics.Trace.WriteLine(address);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
            finally
            {
                host.Close();
            }
        }
    }
    [ServiceContract]
    public interface IWCFService
    {
        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        string GetInfo(int i);
    }
    public class WCFTarget : IWCFService
    {
        public string GetInfo(int input_)
        {
            string result_ = null;
            result_ = @"Value recieved " + input_ + @" ;";
            return result_;
        }
    }

    #endregion

	 #region SelfHost

    [RoutePrefix(@"api/test")]
    public class TestController : ApiController
    {
        [Route("all")]
        public string GetAll()
        {
            return " All ";
        }
        [Route("call")]
        public string GetCall()
        {
            return " Call ";
        }
        
        public string PostAll(string input)
        {
            return "All posted " + input;
        }
    }   

        public class SelfHost
    {
        HttpSelfHostConfiguration config;
        
        public SelfHost()
        {
            config
            = new HttpSelfHostConfiguration(@"http://localhost:8080");

            config.Routes.MapHttpRoute("Call"
                , "api/{controller}/{action}/{input}"
                , new { input = RouteParameter.Optional });
            config.MapHttpAttributeRoutes();
            
        }
        public  void GO()
        {
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                System.Diagnostics.Trace.WriteLine("Press Enter to exit");
                Console.ReadLine();
            }
        }
    }
    #endregion
	
    #region WindowsService

    [RunInstaller(true)]
    public class TestServiceInstaller : Installer
    {
        private ServiceProcessInstaller
            processInstaller;
        private ServiceInstaller
           serviceInstaller;

        public TestServiceInstaller()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "TestService";
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
    public class TestService : ServiceBase
    {
        public TestService()
        {
            StoreLog.InterfaceBind(new LogParams());
            StoreLog.Log(@"TestService initialized");
            this.ServiceName = "TestService";
            this.CanStop = true;
            this.CanPauseAndContinue = false;
            this.AutoLog = true;
        }
        protected override void OnStart(string[] args)
        {
            StoreLog.Log(@"TestService started");
            MessageBox.Show("TestServiceStarted st", "TestServiceStarted",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information,
            MessageBoxDefaultButton.Button1,
            MessageBoxOptions.ServiceNotification);
        }
        protected override void OnStop()
        {
            StoreLog.Log(@"TestService stopped");
            base.OnStop();
        }
    }

    #endregion
	
    #region Sockets

    public class SocketListener
    {
        public void Listen()
        {

            Socket listener = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.IPv6Any, 2112));
            listener.Listen(10);                      
            
            while(true)
            {
                Console.WriteLine(@"Waiting...");
                Socket socket = listener.Accept();
                string receive = string.Empty;
                string reply = string.Empty;

                while(true)
                {
                    try
                    {
                        byte[] bytes = new byte[1024];
                        int numBytes = socket.Receive(bytes);
                        
                        Console.WriteLine(@"Receiving");
                        receive += Encoding.ASCII.GetString(bytes, 0, numBytes);                       
                        
                    }catch(Exception e)
                    {
                        TraceLog.Log(e.Message);
                    }

                    if (receive.IndexOf("[FINAL]") > -1)
                    {
                        break;
                    }
                   
                   
                }

                reply = @"Received: " + receive;
                Console.WriteLine(@"Reply: " + reply);
                byte[] response = Encoding.BigEndianUnicode.GetBytes(reply);
                try
                {
                    socket.Send(response);
                }
                catch (Exception e)
                {
                    TraceLog.Log(e.Message);
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
            listener.Close();
        }
        
    }

    public class SocketClient
    {
        public void Send()
        {
            byte[] receivedBytes = new byte[1024];
            IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAdress = ipHost.AddressList[0];




            while (true)
            {
                IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, 2112);

                Socket sender = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(ipEndpoint);
                    Console.WriteLine("Connected");
                }
                catch (Exception e)
                {
                    TraceLog.Log(e.Message);
                }

                string sendMessage = Console.ReadLine();
                byte[] sendBytes = Encoding.ASCII.GetBytes(sendMessage + @"[FINAL]");

                try
                {
                    sender.Send(sendBytes);
                    int receivedNum = sender.Receive(receivedBytes);
                }
                catch (Exception e)
                {
                    TraceLog.Log(e.Message);
                }

                if (sendMessage.IndexOf("[FINAL]") > -1)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }

        }

    }

    #endregion

    //.NET assemblies
    #region Libraries
    public static class DriveRead
    {
        private static DriveInfo[] drives_;
        private static DirectoryInfo[] directories_;
        private static FileInfo[] fileInfo_;

        public static void Disk()
        {
            BindDrives();
            ReadDrives(drives_);
            ReadDirectories(directories_);
        }

        private static void BindDrives()
        {
            drives_ = DriveInfo.GetDrives();
        }
        private static void ReadDrives(DriveInfo[] drives_)
        {
            foreach (DriveInfo di in drives_)
            {
                DirectoryInfo rootDir_ = di.RootDirectory;
                directories_ = rootDir_.GetDirectories();
            }
        }
        private static void ReadDirectories(DirectoryInfo[] dirs_)
        {
            foreach (DirectoryInfo dirInf_ in dirs_)
            {
                if (dirInf_.GetDirectories().Count() != 0)
                {
                    ReadDirectories(dirInf_.GetDirectories());
                }
                else
                {
                    fileInfo_ = dirInf_.GetFiles();
                }
            }
        }
        private static void ReadFiles(FileInfo[] files_)
        {
            foreach (FileInfo fi_ in files_)
            {

            }
        }
    }

    #endregion

    #region CutomLinq    
				 
    public static class LinqToContextCheck
    {
      static TestContext ts = new TestContext();
      public static void GO()
      {
        ts.ExpressionBuild();

        string st0=ts.TraverseExpression<TestEntity>(s=>s.tp.isTrue==true);

        string st4=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.tp.isTrue==false);
        string st1=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.Id>=1);      

        string st2=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.name=="test name");
        string st3=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.intrinsicIsTrue==true); 
           
      }
    }
    //check Linq to context
    public class TestContext
    {
      ParameterExpression leftParamExpr;
      Type leftType_=null;
      string memberName;

      ConstantExpression constExpr;    

      public TestContext(){
      
      }
      public string VisitLeftRightFromExpressionTypes<T>(Expression<Func<T,bool>> expr_)
        where T:TestEntity{
        var b = expr_;
        var c = b.Body;
        var gtp = c.GetType();
        var nt = c.NodeType;
        var pm = expr_.Parameters;
        var nm = expr_.Name;
        var tp = expr_.Type;
        var typeName = typeof(T);

        //straight convertsion
        string straight=expr_.ToString();     
        BinaryExpression binaryE=(BinaryExpression)expr_.Body;

        //conversion from nested class
        string straightNested=binaryE.ToString();

        if(binaryE.Left!=null){VisitConditional(binaryE.Left);}
        if(binaryE.Right!=null){VisitConditional(binaryE.Right);}

        Expression leftParameter=Expression.Parameter(leftType_,leftType_.Name);         
        Type tp0=leftParameter.GetType();     
        Expression leftExpr=Expression.Property(leftParameter,leftParamExpr.Name);
        Type tp1=leftExpr.GetType();
        ExpressionType nodeType=c.NodeType;
        Expression rightParameter=Expression.Constant(constExpr.Value,constExpr.Type);
        Type tp2=rightParameter.GetType();
        Expression e0=Expression.Assign(leftExpr,rightParameter);
        Type tp3=e0.GetType();

        string ets=e0.ToString();
      
        string lb=this.VisitBinary((MemberExpression)binaryE.Left,"oper",binaryE);
        string lb2=this.VisitBinary(binaryE,"converted",leftExpr,nodeType,rightParameter);

        //variable not invoked
        //var a = Expression.Lambda(e0).Compile().DynamicInvoke();

        return ets;
      }   
      public Expression VisitConditional(Expression expr){
        Type type_=expr.GetType().BaseType;
        if(type_==typeof(MemberExpression)){
          MemberExpression memberExpr=(MemberExpression)expr;
          leftType_=memberExpr.Expression.Type;
          /*
          MemberExpression mn=(MemberExpression)memberExpr.Expression;
          memberName=mn.Member.Name;
          */
          leftParamExpr=Expression.Parameter(memberExpr.Type,memberExpr.Member.Name);
        }
        if(type_==typeof(ConstantExpression)||type_==typeof(Expression)){
          constExpr=(ConstantExpression)expr;
          Expression rightExpr=Expression.Constant(constExpr.Value,constExpr.Type);
        }
        return expr;
      }

      public string VisitBinary(MemberExpression binary,string @operator,BinaryExpression expression) =>
        $"{@operator}({binary},{expression})";
      public string VisitBinary(BinaryExpression expression,string op,Expression left,ExpressionType nt,Expression right) =>
        $"{expression}({op}),{left}|{nt}|{right}";
    
      public string TraverseExpression<T>(Expression<Func<T,bool>> expr_){
        //traversing expression
        Pv pv=new Pv();
        string traversed=pv.VisitBody(expr_);
        return traversed;
      }

      //build in function compile
      internal static void BuiltInCompile()
      {
        Expression<Func<double, double, double, double, double, double>> infix =
            (a, b, c, d, e) => a + b - c * d / 2 + e * 3;
        Func<double, double, double, double, double, double> function = infix.Compile();
        double result = function(1, 2, 3, 4, 5); // 12
      }
      public void ExpressionBuild(){
        double a = 2;
        double b = 3;
        BinaryExpression be = Expression.Power(Expression.Constant(2D), Expression.Constant(3D));
        Expression<Func<double>> fd = Expression.Lambda<Func<double>>(be);
        Func<double> ce = fd.Compile();
        double res = ce();
      }

    }

    public class TestEntity
    {
      public string name {get;set;}
      static int id {get;set;}=0;
      public ToggleProp tp {get;set;}
      public int Id{
        get{
          if(id==0){id += 1;}
          return id; 
        }
        set{ id=value;}
      }
      public bool intrinsicIsTrue {get;set;}
    }
    public class ToggleProp
    {
      public bool isTrue {get;set;}
    }

    //https://weblogs.asp.net/dixin/functional-csharp-function-as-data-and-expression-tree
    internal abstract class Ba<TResult>{

      internal virtual TResult VisitBody(LambdaExpression expression) => this.VisitNode(expression.Body, expression);

      protected TResult VisitNode(Expression node, LambdaExpression expression){

        switch (node.NodeType){

          case ExpressionType.Equal:
            return this.VisitEqual((BinaryExpression)node, expression);

          case ExpressionType.GreaterThanOrEqual:
            return this.VisitGreaterorEqual((BinaryExpression)node, expression);

          case ExpressionType.MemberAccess:
            return this.VisitMemberAccess((MemberExpression)node, expression);

          case ExpressionType.Constant:
            return this.VisitConstatnt((ConstantExpression)node, expression);

          default:
            throw new ArgumentOutOfRangeException(nameof(node));
        }

      }

      protected abstract TResult VisitEqual(BinaryExpression equal, LambdaExpression expression);
      protected abstract TResult VisitGreaterorEqual(BinaryExpression equal, LambdaExpression expression);
      protected abstract TResult VisitMemberAccess(MemberExpression equal, LambdaExpression expression);
      protected abstract TResult VisitConstatnt(ConstantExpression equal, LambdaExpression expression);
    }
    internal class Pv : Ba<string>{
	
      protected override string VisitEqual
        (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Equal", expression);

      protected override string VisitGreaterorEqual
        (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Greater", expression);

      protected override string VisitMemberAccess
        (MemberExpression add, LambdaExpression expression) => this.VisitBinary(add, "Member", expression);

      protected override string VisitConstatnt
        (ConstantExpression add, LambdaExpression expression) => this.VisitBinary(add, "Constant", expression);

      private string VisitBinary( // Recursion: operator(left, right)
        BinaryExpression binary, string @operator,LambdaExpression expression) =>
        $"{@operator}({this.VisitNode(binary.Left, expression)},{this.VisitNode(binary.Right, expression)})";

      private string VisitBinary( // Recursion: operator(left, right)
        MemberExpression binary,string @operator,LambdaExpression expression) =>
        $"{binary}";

      private string VisitBinary( // Recursion: operator(left, right)
        ConstantExpression binary,string @operator,LambdaExpression expression) =>
        $"{binary}";
    }

    #endregion

    //start
    #region Check
    public static class Check
    {
        public static void GO()
        {
            tNineCheck.GO();
            EqualityCheck.GO();
        }
    }


    public static class SquareKATA
    {
        public static void GO(string input_)
        {
            
        }
    }
      

    #endregion

    /// <summary>
    /// Old polinom parsing
    /// </summary>
    #region PoliParse

    public class PoliParseCheck
    {
        SignDictionary SignDictionary = new SignDictionary();
        SignDictionary dict_ = new SignDictionary();
        string input_ = @"";

        public void Check()
        {
            input_ = @"b*2.15+a^2*b^2+c(a+b)";

            input_ = @".5*b+12.15+c*0.6";
            var a = DigitParseTest();

        }

        public string DigitParseTest()
        {
            string _result = @"";
            StringBuilder sb = new StringBuilder();
            char[] ch = new char[0];

            for (int i = 0; i < input_.Length; i++)
            {
                if (EncoutedType(input_[i]) == typeof(Number))
                {
                    ch = ParseDigit(i);
                    i += ch.Count();
                    sb.AppendLine();
                    foreach (char dct_ in ch)
                    {
                        sb.Append(dct_);
                    }
                }
            }

            _result = sb.ToString();
            return _result;
        }

        //from position, if digit, parses value (int,double)
        public char[] ParseDigit(int start_)
        {
            char[] _result = new char[0];

            if (start_ <= input_.Length)
            {
                if (EncoutedType(input_[start_]) == typeof(Number))
                {
                    int curpos = input_.Length;
                    Token token_ = null;
                    bool rightGap = false;

                    for (int i = start_; i < input_.Length; i++)
                    {
                        if (rightGap) { break; }

                        char ch_ = input_[i];

                        //digit with dots meet + parse + add

                        token_ = new DigitInt();

                        for (int i2 = i; i2 < input_.Length; i2++)
                        {
                            ch_ = input_[i2];
                            if (EncoutedType(input_[i2]) == typeof(Number))
                            {
                                if (i2 == input_.Length - 1)
                                {
                                    curpos = input_.Length;
                                }
                                else { curpos = i2; }
                                if ((SignDictionary.GetToken(ch_) is DOT))
                                {
                                    token_ = new DigitDouble();
                                }
                            }
                            else
                            {
                                break;
                            }

                        }
                        rightGap = true;
                        _result = new char[curpos + 1 - i];
                        i += 1;
                        for (int i3 = 0; i3 < curpos + 1 - i; i3++)
                        {
                            _result[i3] = input_[i - 1 + i3];
                        }

                        i = curpos + 1;

                    }

                }
            }

            return _result;

        }

        //gets type of char
        public Type EncoutedType(char ch_)
        {
            Type _result = null;

            if (SignDictionary.GetToken(ch_) != null)
            {
                if (dict_.GetToken(ch_) is DOT) { _result = typeof(Number); }
                else { _result = SignDictionary.GetToken(ch_).GetType(); }
            }
            else
            {
                if (Char.IsLetter(ch_)) { _result = typeof(Var); }
                if (Char.IsDigit(ch_)) { _result = typeof(Number); }

            }
            return _result;
        }
    }

    public interface Token
    {
        int ID { get; set; }
    }
    public interface TokenChar : Token
    {
        char Value { get; set; }
    }
    public interface TokenNumber : Token
    {

    }
    public interface TokenInt : TokenNumber
    {
        int Value { get; set; }
    }
    public interface TokenDouble : TokenNumber
    {
        double Value { get; set; }
    }
    public interface Sign : Token
    {
        char Value { get; set; }
    }
    public interface IOperation : Token
    {
        char Value { get; set; }
        void BindValue(char chr_);
    }
    public interface IElement
    {
        int ID { get; set; }
        Token Token { get; set; }
    }

    public class OPERATION : IOperation
    {
        public int ID { get; set; }
        public char Value { get; set; }
        public void BindValue(char ch_)
        {
            Value = ch_;
        }
    }
    public class SUMM : OPERATION
    {

    }
    public class MULT : OPERATION
    {

    }
    public class SUBS : OPERATION
    {

    }
    public class DIVI : OPERATION
    {

    }
    public class EXP : OPERATION
    {

    }
    public class EQU : Sign
    {
        public int ID { get; set; }
        public char Value { get; set; }
    }
    public class Number : TokenNumber
    {
        public int ID { get; set; }
    }
    public class DigitInt : Number
    {

    }
    public class DigitDouble : Number
    {
    }
    public class DOT : TokenNumber
    {
        public int ID { get; set; }
        public char Value { get; set; }
    }
    public class Var : TokenChar
    {
        public int ID { get; set; }
        public char Value { get; set; }
    }
    public class LEFTBR : Sign
    {
        public int ID { get; set; }
        public char Value { get; set; }
    }
    public class RIGHTBR : Sign
    {
        public int ID { get; set; }
        public char Value { get; set; }
    }

    public class Element : IElement
    {
        public int ID { get; set; }
        public Token Token { get; set; }
    }
    public class Tree
    {
        public static int NodeId { get; set; }

        public Tree LeftNode { get; set; }
        public Tree RightNode { get; set; }

        public IOperation TreeVertex { get; set; }
        public List<Token> Body { get; set; }

        public bool simplified { get; set; }

    }

    public class SignDictionary
    {

        public Dictionary<char[], Token> Dictionary_;
        public SignDictionary()
        {


            Dictionary_ = new Dictionary<char[], Token>();

            DictionaryAdd(new char[1] { '.' }, new DOT());

            DictionaryAdd(new char[1] { '=' }, new EQU());

            DictionaryAdd(new char[1] { '+' }, new SUMM());
            DictionaryAdd(new char[1] { '*' }, new MULT());
            DictionaryAdd(new char[1] { '^' }, new EXP());

            DictionaryAdd(new char[1] { '(' }, new LEFTBR());
            DictionaryAdd(new char[1] { ')' }, new RIGHTBR());

            foreach (KeyValuePair<char[], Token> kvp in Dictionary_)
            {
                char[] ch = new char[1] { '.' };
                var c = kvp.Key.SequenceEqual(ch);
            }

        }
        public void DictionaryAdd(char ch_, Token value_)
        {
            char[] _ch = new char[0];
            _ch[0] = ch_;
            if (!this.Dictionary_.Where(s => s.Key == _ch).Any())
            {
                this.Dictionary_.Add(_ch, value_);
            }
        }
        public void DictionaryAdd(char[] ch_, Token value_)
        {
            if (!this.Dictionary_.Where(s => s.Key == ch_).Any())
            {
                Dictionary_.Add(ch_, value_);
            }
        }
        public Token GetToken(char[] ch_)
        {
            Token _result = null;

            if (Dictionary_.Where(s => s.Key.SequenceEqual(ch_)).Any())
            {
                _result = Dictionary_.Where(s => s.Key.SequenceEqual(ch_)).FirstOrDefault().Value;
            }

            return _result;
        }
        public Token GetToken(char ch_)
        {
            Token _result = null;
            char[] _ch = new char[1] { ch_ };

            _result = GetToken(_ch);

            return _result;
        }

    }

    public delegate Tree OperationDel(Token tl, Token rt, SUMM op);

    public class TokenSum
    {

        TokenInt _result;
        TokenDouble _double;
        TokenChar _char;

        public void TokenIntBind(TokenInt token_)
        {
            _result = token_;
        }
        public void TokenBindDouble(TokenDouble token_)
        {
            _double = token_;
        }
        public void TokenBindChar(TokenChar token_)
        {
            _char = token_;
        }

        public TokenInt TokenSumInt(TokenInt tl, TokenInt tr, SUMM op)
        {
            _result.Value = tl.Value + tr.Value;

            return _result;
        }
        public TokenDouble TokenSumDouble(TokenDouble tl, TokenDouble tr, SUMM op)
        {
            _double.Value = tl.Value + tr.Value;
            return _double;
        }

    }
    public class TreeSum
    {
        Tree _result;

        public void bindTree(Tree tree_)
        {
            _result = tree_;
        }
        public Tree TreeSumming(Tree t1, Tree t2, IOperation o)
        {
            if (t1.simplified)
            {
                if (t2.simplified)
                {
                    if (o is SUMM)
                    {

                    }
                }
            }

            return _result;
        }
    }

    #endregion

    /// <summary>
    /// Polinom parsing project
    /// </summary>
    #region PoliParseType

    public static class PoliParse
    {
        public static void Check()
        {

            Container container_ = new Container();

            //container_.items.Where(s=>s.)

            try
            {

                StringBuilder sb = new StringBuilder();
                List<ItemsStatus> ds = new List<ItemsStatus>();

                container_.ParsedInit();
                container_.StringToItemParse(@"123");
                container_.ExpressionsParse();
                int _actual = (from s in container_.parsed select s).Count();

                ds.Add(new ItemsStatus() { Equation = @"1", Items = 1 });
                ds.Add(new ItemsStatus() { Equation = @"1+1=2", Items = 1 });

                foreach (ItemsStatus ds_ in ds)
                {
                    container_.ParsedInit();
                    container_.StringToItemParse(ds_.Equation);
                    container_.ExpressionsParse();
                    try
                    {
                        ds_.Compare(container_._priority.Count());
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.WriteLine(e.Message);
                    }
                }

                var res = (from s in ds where s.Status == false select s).Any();

            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

        }
    }

    /// <summary>
    /// Interfaces for syntactic level primitives
    /// IToken - char container, IItem - for digits, several chars, IExp - expression level from single token for variables and to several 
    /// </summary>
    public interface IToken_
    {
        char value_ { get; set; }
    }
    public interface IItem_
    {
        IToken_[] tokens_ { get; set; }
        bool CheckValue(char[] arr_);
        bool CheckValue(char arr_);
    }
    public interface IExp_
    {
        IItem_[] body { get; set; }
    }

    /// <summary>
    /// Interfaces for operations, equation and brakets signs
    /// need to be initialized with IItem of IToken chars collection
    /// </summary>
    public interface IOpeartion_ : IItem_
    {

    }
    public interface ISign_ : IOpeartion_
    {

    }

    /// <summary>
    /// Interfaces for operands - digits and virables
    /// Vriables is letter, single char only -->>!! change detection for words
    /// </summary>
    public interface IOperand_ : IItem_
    {

    }
    public interface IDigit_ : IOperand_
    {

    }
    public interface IVariable_ : IOperand_
    {

    }

    /// <summary>
    /// Interface for collection of left and right parts of Expressions with operations and priorities for operations
    /// </summary>
    public interface IPriority
    {
        int ID { get; set; }
        IExp_ leftItem { get; set; }
        IExp_ rightItem { get; set; }
        IOperation operation { get; set; }

        IExp_ result { get; set; }
        int resultID { get; set; }

        int priority { get; set; }
    }

    /// <summary>
    /// Custom errors
    /// </summary>
    public class DoubleDotsException : System.Exception
    {
        public DoubleDotsException() : base()
        {

        }

        public DoubleDotsException(string input_) : base(input_)
        {

        }

    }
    public class NotRecognizedTokenException : System.Exception
    {
        public NotRecognizedTokenException() : base()
        {

        }

        public NotRecognizedTokenException(string input_) : base(input_)
        {

        }

    }
    public class DoubleDigitsException : System.Exception
    {
        public DoubleDigitsException() : base()
        {

        }

        public DoubleDigitsException(string input_) : base(input_)
        {

        }

    }

    /// <summary>
    /// Classes implementing syntactic interfaces
    /// </summary>
    public class Token_ : IToken_
    {
        public char value_ { get; set; }
    }
    public class Item_ : IItem_
    {
        public Item_()
        {

        }
        public Item_(char[] char_)
        {
            this.tokens_ = new Token_[char_.Count()];
            for (int i = 0; i < char_.Count(); i++)
            {
                this.tokens_[i] = new Token_ { value_ = char_[i] };
            }

        }
        public Item_(char char_)
        {
            this.tokens_ = new Token_[1];
            this.tokens_[0] = new Token_ { value_ = char_ };
        }
        public IToken_[] tokens_ { get; set; }
        public bool CheckValue(char[] arr_)
        {
            bool _result = false;
            if (tokens_ != null)
            {
                if (tokens_.Count() == arr_.Count())
                {
                    _result = true;
                    for (int i = 0; i < tokens_.Count(); i++)
                    {
                        if (tokens_[i].value_ != arr_[i])
                        {
                            _result = false;
                        }
                    }
                }
            }
            return _result;
        }
        public bool CheckValue(char ch_)
        {
            char[] arr = new char[1] { ch_ };
            return CheckValue(arr);
        }
    }
    public class Exp_ : IExp_
    {
        public IItem_[] body { get; set; }
    }

    /// <summary>
    /// Classes implementing interfaces for operations and signs
    /// </summary>
    public class Operation_ : Item_, IOpeartion_
    {
        public Operation_()
        {

        }
        public Operation_(char[] char_) : base(char_)
        {

        }
        public Operation_(char char_) : base(char_)
        {

        }
    }
    public class Sign_ : Item_, ISign_
    {

    }

    /// <summary>
    /// Classes implementing variable and digit interfaces
    /// </summary>
    public class Variable_ : Item_, IVariable_
    {
        public Variable_()
        {

        }
        public Variable_(char[] char_) : base(char_)
        {

        }
        public Variable_(char char_) : base(char_)
        {

        }
    }
    public class Digit_ : Item_, IDigit_
    {
        public Digit_()
        {

        }
        public Digit_(char[] char_) : base(char_)
        {

        }
        public Digit_(char char_) : base(char_)
        {

        }
    }

    /// <summary>
    /// Priority class
    /// </summary>
    public class Priority : IPriority
    {
        public int ID { get; set; }
        public IExp_ leftItem { get; set; }
        public IExp_ rightItem { get; set; }
        public IOperation operation { get; set; }

        public IExp_ result { get; set; }
        public int resultID { get; set; }

        public int priority { get; set; }
    }

    /// <summary>
    /// Class responsible for:
    /// initializing operators primitives -->>!! separate
    /// parsing equatation using Token and interfaces 
    /// priority logic
    /// </summary>
    public class Container
    {

        /// <summary>
        /// 
        /// Main operation logic
        /// 
        /// (a+b+c)(a+c+e) -> foreach item in multiple division united sum with every item from another
        /// 
        /// Priority list:
        /// 0 a b + 0  0
        /// 1 0 c + 1  1
        /// 2 1 4 + 2  4
        /// 3 a c + 3  2
        /// 4 3 e + 4  3        
        /// 
        /// Item operations:
        /// ch +(-) ch(dg) -> 
        /// if(ch!=ch ) { concat }
        /// if(ch==ch) { ch * 2 } increase multiple +1
        /// ch * ch -> 
        /// if(ch==ch){ ch ^ 2 } increase exp +1
        /// if(ch!=ch){ concat }
        /// 
        /// ch / ch ->
        /// if(ch == ch){ 1 }
        /// if(ch!=ch){ concat }
        /// 
        /// Expression operations:
        /// 
        /// dg + -> add to PL
        /// dg * wait for next
        /// dg * ch -> item with mult
        /// x(dg *) ch -> item with mult * ch
        /// dg * ch ^ ch -> item with mult and exp
        /// ch * ch -> char mult char
        /// ch ^ ch -> char exp char
        /// nx^y*zx^w -> ch*ch^ch*ch*ch^ch -> Expr * Expr
        /// if (ch == ch){ mult + mult; exp + exp }
        /// if (ch != ch){ mult + mult; } -> (nz)*x^y*m^w
        /// a*b*c*a -> a^2 * b *c -> ch1 * ch2 * ch3 *ch1 -> ch1 mult+1
        /// (ch *) -> exp
        /// (dg * ch ^ dg) -> item
        /// 
        /// ==============================
        /// 
        /// ch* .. *ch n -> ^ exp +1 n
        /// ch+ .. +ch n -> * mult +1 n
        /// expr
        /// item1* item2*item2 - > item1* item2^2
        /// (2ab^2+3e)*eb^3
        /// (3a+2b+4c*3d*6e^2)+(4a-1b+3c*2d^3*5e)
        ///
        /// exp1{
        ///a 2 1
        ///b 1 2
        ///*
        ///}
        ///
        /// exp2 {
        ///e 3 1 
        ///}
        ///
        ///Repeat for every not simplified operation until operation = is simplified left right:
        ///First - > gather expressions by* and / operators containing of
        ///items with multiples with(* digits) and expression with(^)
        ///Second -> simplify expressions
        ///Third -> table of priorities for expressions
        /// 
        /// </summary>

        public List<IPriority> _priority = new List<IPriority>();
        public List<IItem_> operations = new List<IItem_>();
        public List<IItem_> items = new List<IItem_>();

        public List<ParsedItems> parsed;
        public List<IExp_> expressions;

        IItem_ SUMMATION;
        IItem_ SUBSTRACTION;
        IItem_ MULTIPLICATION;
        IItem_ EXPONENTIATION;

        IItem_ DIGIT;
        IItem_ VARIABLE;

        IItem_ EQUATION;

        public Container()
        {
            OperationsInit();
            ItemInit();
            SignsInit();
        }

        protected void OperationsInit()
        {
            SUMMATION = new Operation_() { tokens_ = new Token_[] { new Token_() { value_ = '+' } } };
            SUBSTRACTION = new Operation_() { tokens_ = new Token_[] { new Token_() { value_ = '-' } } };
            MULTIPLICATION = new Operation_() { tokens_ = new Token_[] { new Token_() { value_ = '*' } } };
            EXPONENTIATION = new Operation_() { tokens_ = new Token_[] { new Token_() { value_ = '^' } } };

            operations.Add(SUMMATION);
            operations.Add(SUBSTRACTION);
            operations.Add(MULTIPLICATION);
            operations.Add(EXPONENTIATION);
        }
        protected void ItemInit()
        {
            DIGIT = new Digit_();
            VARIABLE = new Variable_();

            items.Add(DIGIT);
            items.Add(VARIABLE);
        }
        protected void SignsInit()
        {
            EQUATION = new Sign_() { tokens_ = new Token_[] { new Token_() { value_ = '=' } } };
            items.Add(EQUATION);
        }
        public void ParsedInit()
        {
            this.parsed = new List<ParsedItems>();
        }

        public void StringToItemParse(string input_)
        {
            IItem_ actReadItem = null;

            for (int i = 0; i < input_.Length; i++)
            {
                char ch_ = input_[i];

                //operation met
                if ((from s in operations where s.CheckValue(ch_) == true select s).Any())
                {
                    actReadItem = (from s in operations where s.CheckValue(ch_) == true select s).FirstOrDefault();
                }
                if ((from s in items where s.CheckValue(ch_) == true select s).Any())
                {
                    //signs met
                    actReadItem = (from s in items where s.CheckValue(ch_) == true select s).FirstOrDefault();
                }
                //variable met
                if (Char.IsLetter(ch_))
                {
                    actReadItem = new Variable_(ch_);
                }
                //double digit met
                if (Char.IsDigit(ch_) || ch_ == '.')
                {
                    List<char> item_read = new List<char>();

                    //digit met
                    int i2 = i;
                    while (i2 < input_.Length && (Char.IsDigit(input_[i2]) || input_[i2] == '.'))
                    {
                        if (input_[i2] == '.' && input_[i2 + 1] == '.') { throw new DoubleDotsException(); }
                        ch_ = input_[i2];

                        item_read.Add(ch_);
                        i2 += 1;
                    }
                    i = i2 - 1;
                    actReadItem = new Variable_(item_read.ToArray());
                    item_read.Clear();
                }


                if (actReadItem == null)
                {
                    throw new NotRecognizedTokenException();
                }

                parsed.Add(new ParsedItems(actReadItem, actReadItem.GetType()));

            }

        }

        public void ExpressionsParse()
        {

            IItem_ prevReadItem = null;
            IItem_ actReadItem = null;
            IItem_[] arr;

            foreach (ParsedItems pi_ in parsed)
            {

                IItem_ item_ = pi_.Item;
                actReadItem = item_;

                //decide Item Expression
                if (prevReadItem == null)
                {
                    //first encounter or after bracket
                    arr = new IItem_[1] { actReadItem };
                    _priority.Add(new Priority() { leftItem = new Exp_() { body = arr } });
                }
                else
                {
                    //previous digit
                    if (prevReadItem.GetType() is IDigit_)
                    {
                        //actual digit
                        if (actReadItem.GetType() is IDigit_)
                        {
                            throw new DoubleDigitsException();
                        }
                        //multiplication 
                        if (actReadItem.GetType() is IVariable_)
                        {

                        }
                        if (actReadItem.GetType() is IOpeartion_)
                        {

                        }
                    }
                    //previous variable
                    if (prevReadItem.GetType() is IVariable_)
                    {

                    }
                }

                prevReadItem = actReadItem;
            }

        }

    }

    public class ParsedItems
    {
        public ParsedItems(IItem_ item_, Type type_)
        {
            this.Item = item_;
            this.Type = type_;
        }
        public IItem_ Item { get; set; }
        public Type Type { get; set; }
    }

    //Classes for test 
    public class DotsStatus
    {
        public string Equation { get; set; }
        public bool Status { get; set; }
    }
    public class ItemsStatus : DotsStatus
    {
        public int Items { get; set; }
        public void Compare(int a_)
        {
            if (a_ == Items)
            {
                Status = true;
            }
            else
            {
                Status = false;
            }
        }

    }

    #endregion

    /// <summary>
    /// Files to txt parse
    /// </summary>
    #region StreamReadWrite
    /// <summary>
    /// CopyPast class
    /// Parses class to txt with JSON
    /// Reads result JSON file to class and creates files from content
    /// Detects minimal single directory and recreates folder structure in new folder
    /// </summary>
    public class StreamCheck
    {
        public void Check()
        {
            ReadFilesCheck();
            ReadCheck();
        }

        public void ReadFilesCheck()
        {
            StreamIO sio3 = new StreamIO();
            string pathResult_ = @"C:\FILES\SHARE\debug\moove\new\result.txt";
            string pathFolders_ = @"C:\FILES\SHARE\debug\moove\1";

            sio3.pathResult_ = pathResult_;
            sio3.pathFolders_ = pathFolders_;

            sio3.DeleteFileChecked(sio3.pathResult_);
            sio3.PathsToLIstAddRecursive(sio3.pathFolders_, 0);

            foreach (string str_ in sio3.fileList)
            {
                PathDict pd = new PathDict();
                sio3.DictionaryInit(pd);
                sio3.FileToDcitionary(str_);
                sio3.DictionaryListAdd();
            }

            //File.WriteAllText(sio3.pathResult_, JsonConvert.SerializeObject(sio3.pathDictList, Formatting.Indented));
            sio3.AddBytesToFile(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sio3.pathDictList, Formatting.Indented)), sio3.pathResult_);

        }
        public void ReadCheck()
        {
            StreamIO sio3 = new StreamIO();
            string pathResult_ = @"C:\FILES\SHARE\debug\moove\new\result.txt";
            //string pathResult_ = @"C:\111\moove\result.txt";

            sio3.pathResult_ = pathResult_;

            List<PathDict> pd = JsonConvert.DeserializeObject<List<PathDict>>(File.ReadAllText(sio3.pathResult_));

            foreach (IPathDictSaearch pd_ in pd)
            {
                sio3.pathDictList.Add(pd_);
            }

            sio3.PathSimplify(sio3.PathRemoveFileName(sio3.pathResult_));

            foreach (IPathDictSaearch pd_ in sio3.pathDictList)
            {
                sio3.CreateFoldersExplicitly(sio3.PathRemoveFileName(pd_.Path));
                if (!pd_.Path.Contains(@"Thumbs"))
                {
                    File.WriteAllText(pd_.Path, Encoding.UTF8.GetString(Encoding.UTF8.GetBytes((pd_.Text))));
                }
            }

        }
    }

    public class StreamIO
    {
        internal IPathDictSaearch pathDict;
        public List<IPathDictSaearch> pathDictList = new List<IPathDictSaearch>();
        internal List<string> fileList = new List<string>();

        public string pathResult_ = null;
        public string pathFolders_ = null;

        public string path = @"";

        internal void DictionaryInit(IPathDictSaearch dict_)
        {
            this.pathDict = dict_;
        }
        internal void DictionaryListAdd()
        {
            this.pathDictList.Add(pathDict);
        }
        private void DictionaryVal(string path_ = null, string text_ = null)
        {
            if (path_ != null)
            {
                pathDict.Path = path_;
            }
            if (text_ != null)
            {
                pathDict.Text = text_;
            }
        }

        internal void DeleteFileChecked(string input_)
        {
            if (File.Exists(input_))
            {
                File.Delete(input_);
            }
        }
        internal byte[] FileToBytesChecked(string input_)
        {
            if (input_ == null)
            {
                throw new NullReferenceException();
            }
            if (input_.Length == 0)
            {
                throw new InvalidOperationException(@"Empty file");
            }
            if (!File.Exists(input_))
            {
                throw new FileNotFoundException();
            }

            //FileInfo fi = new FileInfo(input_);
            //BinaryReader br = new BinaryReader(fi.OpenRead(),Encoding.UTF8);
            //byte[] _result = new byte[br.BaseStream.Length];
            //br.Read(_result, 0, _result.Length);

            FileStream fs = File.OpenRead(input_);
            byte[] _result = new byte[fs.Length];
            fs.Read(_result, 0, _result.Length);

            return _result;
        }
        internal void AddBytesToFile(byte[] input_, string output_)
        {
            if (!File.Exists(output_))
            {
                using (FileStream fs = new FileStream(output_, FileMode.Create))    //Path.GetExtension(input_)))
                {

                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(input_, 0, input_.Length);
                }
            }
            else
            {
                using (FileStream fs = new FileStream(output_, FileMode.Append))    //Path.GetExtension(input_)))
                {
                    fs.Seek(0, SeekOrigin.End);
                    fs.Write(input_, 0, input_.Length);
                }
            }

        }

        internal void PathSimplify(string NewPath_)
        {
            if (pathDictList.Count != 0)
            {

                while (pathDictList.Where(s => !s.Searched).Any())
                {
                    string oldMInimal = null;

                    if (pathDictList.Count != 1)
                    {
                        for (int i = 0; i < pathDictList.Count; i++)
                        {
                            for (int i2 = i; i2 < pathDictList.Count; i2++)
                            {
                                if (i2 != i && !pathDictList[i2].Searched)
                                {
                                    string newMinimal = CompareMinimal(PathRemoveFileName(pathDictList[i].Path), PathRemoveFileName(pathDictList[i2].Path));
                                    if (oldMInimal == null || oldMInimal.Length > newMinimal.Length)
                                    {
                                        oldMInimal = newMinimal;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        oldMInimal = PathRemoveFileName(pathDictList[0].Path);
                    }

                    if (oldMInimal != null)
                    {
                        PathReplace(oldMInimal, NewPath_);
                        FilePathDictionaryPrint();
                    }
                }

            }

            //search shortest path

            //remember
            //remember lowest level
            //mark as searched

            //remove path from all others

            //repeat for all
        }
        private string CompareMinimal(string a_, string b_)
        {
            string _minimalString = null;

            string[] arr_a = a_.Split('\\');
            string[] arr_b = b_.Split('\\');

            if (arr_a.Length < arr_b.Length)
            {
                for (int i = 0; i < arr_a.Length; i++)
                {
                    if (arr_b[i] != arr_a[i]) { break; }

                    if (_minimalString == null)
                    {
                        _minimalString += arr_a[i];
                    }
                    else
                    {
                        _minimalString += @"\" + arr_a[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < arr_a.Length; i++)
                {
                    if (arr_b[i] != arr_a[i]) { break; }

                    if (_minimalString == null)
                    {
                        _minimalString += arr_b[i];
                    }
                    else
                    {
                        _minimalString += @"\" + arr_b[i];
                    }
                }
            }

            return _minimalString;
        }
        internal string PathRemoveFileName(string input_)
        {
            string _result = null;
            if (Path.GetExtension(input_) != "")
            {
                _result = Path.GetDirectoryName(input_);
            }
            else
            {
                _result = input_;
            }
            return _result;
        }
        private void PathReplace(string old_, string new_)
        {
            foreach (IPathDictSaearch pd in pathDictList)
            {
                if (pd.Path.Contains(old_))
                {
                    pd.Path = pd.Path.Replace(old_, new_);
                    pd.Searched = true;
                }
            }
        }

        internal void CreateFoldersExplicitly(string _input)
        {
            string[] arr = _input.Split('\\');
            StringBuilder sb = new StringBuilder();

            foreach (string str in arr)
            {
                if (sb.Length == 0)
                {
                    sb.Append(str);
                }
                else
                {
                    sb.Append(@"\" + str);
                }

                if (!Directory.Exists(sb.ToString()))
                {
                    try
                    {
                        Directory.CreateDirectory(sb.ToString());
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Trace.WriteLine(e.Message);
                    }
                }
            }
        }

        internal void PathsToLIstAddRecursive(string input_, int level_)
        {
            level_ += 1;

            string[] directories = Directory.GetDirectories(input_);
            string[] files = Directory.GetFiles(input_);
            if (files.Length != 0)
            {
                foreach (string str_ in files)
                {
                    fileList.Add(str_);
                }
            }

            if (directories.Length != 0)
            {
                foreach (string str_ in directories)
                {
                    PathsToLIstAddRecursive(str_, level_);
                }
            }

            level_ -= 1;
        }

        private void FilePathDictionaryPrint()
        {
            System.Diagnostics.Trace.WriteLine(@"---------------------------");
            foreach (IPathDictSaearch pd in pathDictList)
            {
                System.Diagnostics.Trace.WriteLine(pd.Searched + @" " + pd.Path);
            }
        }

        internal void FileToDcitionary(string str)
        {
            pathDict.Path = str;
            pathDict.Text = Encoding.UTF8.GetString(FileToBytesChecked(str));
        }
    }

    public class PathDict : IPathDict, IPathDictSaearch
    {
        public void Init(string path_ = null, string text_ = null)
        {
            if (path_ != null)
            {
                this.Path = path_;
            }
            if (text_ != null)
            {
                this.Text = text_;
            }
        }
        public string Path { get; set; }
        public string Text { get; set; }
        public bool Searched { get; set; }
    }
    public interface IPathDict
    {
        void Init(string path_ = null, string text_ = null);
        string Path { get; set; }
        string Text { get; set; }
    }
    public interface IPathDictSaearch : IPathDict
    {
        bool Searched { get; set; }
    }
    #endregion

    /// <summary>
    /// Reading parameters passed to application
    /// </summary>
    #region ConsoleParameters

    public static class ConsoleParametersCheck
    {
        public static void Check(string[] args)
        {
            ConsoleParameters.Check(args);
        }
    }

    public static class ConsoleParameters
    {
        public static List<ParametersForConsole> parameters_;

        static ConsoleParameters()
        {
            Initialize();
        }

        public static void Check(string[] args)
        {
            foreach (string str_ in args)
            {
                Console.WriteLine(str_);
            }
            Console.ReadLine();
        }
        public static void Initialize()
        {
            parameters_ = new List<ParametersForConsole>();
            parameters_.Add(new ParametersForConsole()
            {
                ID = 0,
                ParameterName = @"-m",
                Description = @"months"
            });
        }
        public static void ExportJson()
        {

        }
        public static void ImportJson()
        {

        }
        private static string[] StringToArgs(string input_)
        {
            string[] args = null;
            args = input_.Replace(@"""", @"").Split(' ');
            return args;
        }
    }

    public class ParametersForConsole
    {
        public int ID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Description { get; set; }
    }

    #endregion

    /// <summary>
    /// >>||| REWRITE
    /// Gets files from folder parses them with Encoding to txt file with pathes and Encoding name.
    /// </summary>s
    #region StreamsTesting

    public delegate string encodeStringDel(byte[] arr);
    public delegate byte[] encodeArrDel(string input_);

    public delegate string encodeStringCodePageDel(byte[] arr, int codepage_);
    public delegate byte[] encodeArrCodePageDel(string input_, int codepage_);

    public static class StreamTesting
    {
        static string FilePathIn { get; set; }
        static string DirectoryIn { get; set; }
        static string FilePathOut { get; set; }
        static string FileNameOut { get; set; }
        static string DirectoryOut { get; set; }
        static FileStream fsIn { get; set; }
        static FileStream fsOut { get; set; }
        static byte[] arr { get; set; }
        static int length { get; set; }
        static int pos { get; set; }

        static StreamReader sr;
        static int cnt;
        static int b = 0;
        static string a = null;

        static encodeStringDel encodingStr { get; set; }
        static encodeArrDel encodingArr { get; set; }

        static encodeStringCodePageDel encodingStrCP { get; set; }
        static encodeArrCodePageDel encodingArrCP { get; set; }

        static Encoding encoding { get; set; }
        static int codepage { get; set; }

        static Encoding localEncoding_ = null;

        static List<PathText> pathTextList = new List<PathText>();
        public static void Check()
        {

            StartDefault();

            //SerializeOne();
            //DeserializeOne();

        }

        public static void SerializeOne()
        {
            string FileToParse = @"C:\FILES\SHARE\debug\moove\1\send.7z";
            string FileToExportBytesStr = @"C:\FILES\SHARE\debug\moove\new\send_bytes_str.txt";
            //string FileToExportBytes = @"C:\FILES\SHARE\debug\moove\new\send_bytes.txt";
            string FileToExport = @"C:\FILES\SHARE\debug\moove\new\out.txt";
            byte[] arr;
            string name;
            string res_;
            StreamReader sr_;
            StringBuilder sb;

            using (FileStream fs_ = new FileStream(FileToParse, FileMode.Open))
            {
                sr_ = new StreamReader(fs_, true);
                name = sr_.CurrentEncoding.BodyName;
                arr = new byte[fs_.Length];
                fs_.Read(arr, 0, arr.Length);
            }

            sb = new StringBuilder();

            foreach (byte bt_ in arr)
            {
                sb.Append(bt_);
                sb.Append(@",");
            }
            res_ = sb.ToString();

            File.WriteAllText(FileToExportBytesStr, res_);

            using (FileStream fsWrite_ = new FileStream(FileToExport, FileMode.OpenOrCreate))
            {
                fsWrite_.Write(arr, 0, arr.Length);
            }
        }
        public static void DeserializeOne()
        {
            string FileToParse = @"C:\FILES\SHARE\debug\moove\new\out.txt";

            string FileToExport = @"C:\FILES\SHARE\debug\moove\new\out_de.txt";
            byte[] arr;

            FileStream fs = new FileStream(FileToParse, FileMode.Open);
            arr = new byte[fs.Length];
            fs.Read(arr, 0, arr.Length);

            using (FileStream fsWrite_ = new FileStream(FileToExport, FileMode.OpenOrCreate))
            {
                fsWrite_.Write(arr, 0, arr.Length);
            }

        }

        public static void StartDefault()
        {
            StringEncoder();
            InitFromCode();
            SetDefaultEncoding();
            Serialize_();
            Deserialize_();
        }
        public static void InitFromCode()
        {
            DirectoryIn = @"C:\FILES\SHARE\debug\moove\1";
            DirectoryOut = @"C:\FILES\SHARE\debug\moove\new";
            FileNameOut = @"out.txt";
            FilePathOut = Path.Combine(DirectoryOut, FileNameOut);
        }
        public static void StreamWrite()
        {
            arr = new byte[length];
            fsIn.Read(arr, 0, length);
            ByteArrayAsIs(arr, FilePathOut + @"_bytesASIS.txt");
            fsOut.Write(arr, 0, length);
            BytesExport();
        }
        public static void BytesExport()
        {
            File.WriteAllBytes(DirectoryOut + @"\_bytes.txt", arr);
        }
        public static void ArrWrite(byte[] arr_)
        {
            fsOut.Write(arr_, 0, arr_.Length);
        }
        public static void WriteNewLine()
        {
            fsOut.WriteByte(13);
        }
        public static void Write_(byte[] arr_)
        {
            ArrWrite(arr);
            WriteNewLine();
        }

        public static void ByteArrayAsIs(byte[] arr_, string path_)
        {
            StringBuilder sb = new StringBuilder();

            foreach (byte bt_ in arr_)
            {
                sb.Append(bt_);
                sb.Append(@",");
            }
            string res_ = sb.ToString();

            File.WriteAllText(path_, res_);
        }

        public static void Serialize_()
        {
            fsOut = new FileStream(FilePathOut, FileMode.OpenOrCreate);
            foreach (string filename in Directory.GetFiles(DirectoryIn))
            {
                if (!filename.Contains(@"Thumbs"))
                {

                    FilePathIn = filename;
                    fsIn = new FileStream(FilePathIn, FileMode.Open);

                    //new filename in output directory
                    FileNameOut = Path.GetFileNameWithoutExtension(FilePathIn) + @"_copy" + Path.GetExtension(FilePathIn);
                    length = filename.Length;


                    //write length of next array and array
                    arr = encodingArr(length.ToString());
                    Write_(arr);

                    arr = encodingArr(filename);
                    Write_(arr);

                    //set default string encoding UTF-8
                    StringEncoder();

                    //write encoding length and name                
                    arr = encodingArr(EncodingGetFS(fsIn).CodePage.ToString().Length.ToString());
                    Write_(arr);

                    arr = encodingArr(EncodingGetFS(fsIn).CodePage.ToString());
                    Write_(arr);

                    //write length of array and array
                    length = (int)fsIn.Length;
                    arr = encodingArr(length.ToString());
                    Write_(arr);

                    //get encoding
                    EncodingGet(fsIn);
                    //change encoding for file text
                    codepage = encoding.CodePage;

                    //write file text
                    StreamWrite();
                    WriteNewLine();

                    fsIn.Close();
                    fsIn.Dispose();
                }
            }
            fsOut.Close();
            fsOut.Dispose();
        }
        public static void Deserialize_()
        {
            PathTextFill();
            PathtextRead();
        }

        public static void PathTextFill()
        {
            fsIn = new FileStream(FilePathOut, FileMode.Open);
            sr = new StreamReader(fsIn);
            int encode = 0;
            while (fsIn.Position + 1 < fsIn.Length)
            {
                string path = ParsePathText();
                Int32.TryParse(ParsePathText(), out encode);
                localEncoding_ = Encoding.UTF8; //Encoding.GetEncoding(encode);
                string text = ParsePathText();
                if (path != "" && encode != 0 && text != "")
                {
                    pathTextList.Add(new PathText(Path.Combine(DirectoryOut, Path.GetFileName(path)), arr, codepage));
                }
            }
        }
        public static void PathtextRead()
        {
            foreach (PathText pt_ in pathTextList)
            {
                if (true == true)
                {
                    using (FileStream fss = new FileStream(pt_.path, FileMode.OpenOrCreate))
                    {
                        try
                        {
                            fss.Write(pt_.text, 0, pt_.text.Length);
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Trace.WriteLine(e.Message);
                        }
                        fss.Close();
                    }
                }
            }
        }

        public static string ParsePathText()
        {
            string _Newpath = null;
            cnt = 0;
            while (fsIn.ReadByte() != 13)
            {
                if (fsIn.Position >= fsIn.Length) break;
                cnt += 1;
            }
            fsIn.Position -= cnt + 1;
            arr = new byte[cnt];
            fsIn.Read(arr, 0, cnt);
            a = encodingStrCP(arr, codepage);
            Int32.TryParse(a, out b);
            arr = new byte[b];
            fsIn.Position += 1;
            fsIn.Read(arr, 0, b);
            if (localEncoding_ != null)
            {
                _Newpath = encodingStrCP(arr, localEncoding_.CodePage);
                localEncoding_ = null;
            }
            else
            {
                _Newpath = encodingStrCP(arr, codepage);
            }
            fsIn.Position += 1;

            return _Newpath;
        }

        public static void EncodingGet(FileStream fs_)
        {
            StreamReader sr = new StreamReader(fs_, true);
            encoding = sr.CurrentEncoding;
            if (Path.GetExtension(fs_.Name) == ".7z")
            {
                encoding = Encoding.BigEndianUnicode;
            }
            codepage = encoding.CodePage;
        }
        public static Encoding EncodingGetFS(FileStream fs_)
        {
            StreamReader sr = new StreamReader(fs_, true);
            Encoding result_ = sr.CurrentEncoding;
            if (Path.GetExtension(fs_.Name) == ".")
            {
                result_ = Encoding.BigEndianUnicode;
            }
            return result_;
        }

        public static void StringEncoder()
        {
            BEenc();
            CPenc();
        }
        public static void SetDefaultEncoding()
        {
            encoding = Encoding.UTF8;
        }

        static void UTFenc()
        {
            encodingStr = EncodeStringUTF8;
            encodingArr = EncodeArrUTF8;
        }
        static void BEenc()
        {
            encodingStr = EncodeStringBE;
            encodingArr = EncodeArrBE;
        }
        static void CPenc()
        {
            encodingStrCP = EncodeStringCodepage;
            encodingArrCP = EncodeArrCodepage;
        }

        public static string EncodeStringUTF8(byte[] arr)
        {
            return Encoding.UTF8.GetString(arr);
        }
        public static string EncodeStringBE(byte[] arr)
        {
            return Encoding.ASCII.GetString(arr);
        }

        public static byte[] EncodeArrUTF8(string input_)
        {
            return Encoding.UTF8.GetBytes(input_);
        }
        public static byte[] EncodeArrBE(string input_)
        {
            return Encoding.ASCII.GetBytes(input_);
        }

        public static string EncodeStringCodepage(byte[] arr, int codepage_)
        {
            return Encoding.GetEncoding(codepage_).GetString(arr);
        }
        public static byte[] EncodeArrCodepage(string input_, int codepage_)
        {
            return Encoding.GetEncoding(codepage_).GetBytes(input_);
        }

    }

    public class PathText
    {
        public string path { get; set; }
        public byte[] text { get; set; }
        public int? codepage { get; set; }

        public PathText(string path_, byte[] text_, int? codepage_ = null)
        {
            this.path = path_;
            this.text = text_;
            this.codepage = codepage_;
        }
    }

    #endregion

    #region KATAs
    public static class ReqwindKATA
    {

      

        public static string GO(string input_)
        {

            List<char> arr = new List<char>();
            Stack<char> st = new Stack<char>();
            Stack<char> st2 = new Stack<char>();

            for (int i =0;i<input_.ToArray().Length;i++)
            {
                char ch = input_.ToArray()[i];

                if (ch != ' ')
                {
                    st.Push(ch);
                }
                else
                {
                    if (st.Count >= 5)
                    {
                        while (st.Count > 0)
                        {
                            arr.Add(st.Pop());
                        }
                       
                    }
                    else
                    {
                        while (st.Count > 0)
                        {
                            st2.Push(st.Pop());
                        }
                        while (st2.Count > 0)
                        {
                            arr.Add(st2.Pop());
                        }
                        
                    }
                    arr.Add(' ');
                }
            }

            if (st.Count >= 5)
            {
                while (st.Count > 0)
                {
                    arr.Add(st.Pop());
                }
                
            }
            else
            {
                while (st.Count > 0)
                {
                    st2.Push(st.Pop());
                }
                while (st2.Count > 0)
                {
                    arr.Add(st2.Pop());
                }
               
            }

            return string.Join(null, arr);
        }
    }

    public static class FindKata
    {
        public static char GO(char[] input)
        {
            byte[] arr = Encoding.ASCII.GetBytes(input);
            char result = ' ';
            for (int i =0; i< arr.Length-1; i++)
            {
                if(arr[i]+1<arr[i+1])
                {
                    result = (char)(arr[i]+1);
                    break;
                }
            }

            return result;
        }

    }

    public static class DivideKATA {
        public static int[] Divisors(int n)
        {
            if (n < 2) { return null; }
                    

            List<int> divisors = new List<int>();

            for (int i = 2; i < n; i++)
            {
                if (n % i == 0) { divisors.Add(i); }
            }
            if (divisors.Count == 0)
            {
                return null;
            }
            else
            {
                return divisors.ToArray();
            }
           
        }
    }
    
    public static class FormatRearrange
    {
        public static void GO()
        {
            StringsCheck();
        }

        static void StringsCheck()
        {          

            string input = "{0}{1} {2}";        
            string r1 = Rearrange(input);
        }

        static string Rearrange(string input_)
        {
            string result = input_;
            char[] chr = input_.ToCharArray();
            int lng = chr.Length;
            char[] prevDigit=null;
            char[] currDigit = null;

            for (int i =0;i<lng;i++)
            {

                int i2 = i;

                if (char.IsDigit(chr[i2]))
                {     

                    if ( i2+1 < lng)
                    {
                        while(char.IsDigit(chr[i2+1]))
                        {
                            i2++;
                        }
                      
                    }
               

                    if (prevDigit == null)
                    {
                        prevDigit = ChArrFill(i, i2, chr);                  
                    }
                    else
                    {
                        currDigit = ChArrFill(i, i2, chr);

                        if (!check(currDigit, prevDigit))
                        {
                            currDigit = intRecount(currDigit, prevDigit);

                            char[] chrN = new char[chr.Length + currDigit.Length - prevDigit.Length];

                            for (int i4 = 0; i4 < i; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }
                            for (int i4 = i; i4 < i2; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }
                            for (int i4 = i2; i4 <= lng; i4++)
                            {
                                chrN[i4] = chr[i4];
                            }

                            result = charArrToInteger(chrN).ToString();
                        }
                        else {
                            prevDigit = intToCharArr(charArrToInteger(currDigit));                        
                        }

                    }

                }

            }

            return result;
        }

        static int charArrToInteger(char[] arr_)
        {
            int res = 0;
            int i = 1;            
            for(int i2 = arr_.Length-1;i2>=0;i2--)
            {               
                res += (int)(char.GetNumericValue(arr_[i2]) * i);
                i *= 10;
            }
            return res;
        }
        static char[] intToCharArr(int i_)
        {
            return i_.ToString().ToCharArray();   
        }
        static char[] intRecount(char[] currDig_,char[] prevDigit_)
        {
            if(charArrToInteger(currDig_) == charArrToInteger(prevDigit_)+1)
            {
                return currDig_;
            }
            else
            {
                return intToCharArr(charArrToInteger(prevDigit_) + 1);
            }
        }
        static bool check(char[] currDig_, char[] prevDigit_)
        {
            if (charArrToInteger(currDig_) == charArrToInteger(prevDigit_) + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static char[] ChArrFill(int i_,int i2_, char[] chFrom_)
        {
            char[] chTo_ = new char[(i2_ - i_)+1];

            for (int i3_ = 0; i3_ <= (i2_ - i_); i3_++)
            {
                chTo_[i3_] = chFrom_[i_ + i3_];
            }
            return chTo_;
        }

    }


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
                cl_.Act=tNineChecks.GO(new KeyPadStrait(), cl_.Case);
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
        public static string GO(IKeyPresser kp_,string case_)
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
            for(int i = 0; i < str_.Count(); i++)
            {
                char?[] found = null;
                if(keyPad.ContainsKey(str_[i]))
                {
                    keyPad.TryGetValue(str_[i], out found);
                    if (foundPrev != null)
                    {
                        if(foundPrev[0] == found[0]){ res.Add(' '); }
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
    

    #endregion

}

namespace SBsh_
{
    public static class TraceLog
    {
        internal static Random rnd = new Random();
        internal static long LoadCnt = 0;
        static string path;
        static StreamWriter f;
		
		
        static TraceLog()
        {
            TraceLog.path = AppDomain.CurrentDomain.BaseDirectory;          
        }
        public static void Log(string input_)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            StreamWriter f = new StreamWriter(path + @"\log.txt", true);
            f.Write(DateTime.Now + " : ");
            f.Write(input_);
            f.Write(" ;");
            f.Write(Environment.NewLine);
            f.Flush();
            f.Close();
        }
        public static void WriteLn(string str_ = @"NO_STRING")
        {
            System.Diagnostics.Trace.WriteLine(str_);
        }
        public static string GetFunctionName()
        {
            string className = @"NO_CLASSNAME";
            string methodName = @"NO_METHODNAME";

            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            className = st.GetType().Name;
            methodName = sf.GetMethod().Name;

            string result_ = String.Format(@"Class: {0}, Method: {1} ;", className, methodName);
            return result_;
        }
        public static void ConsumeCPUsleep(int percentage, int mSeconds, int iterations)
        {
            int cnt = 0;
            if (percentage < 0 || percentage > mSeconds)
            {
                throw new ArgumentException("percentage");
            }
            Stopwatch watch = new Stopwatch();
            Console.WriteLine(@"started");
            watch.Start();
            while (cnt < iterations)
            {
                cnt += 1;
                // Make the loop go on for "percentage" milliseconds then sleep the 
                // remaining percentage milliseconds. So 40% utilization means work 40ms and sleep 60ms
                if (watch.ElapsedMilliseconds > percentage)
                {
                    WriteLn(cnt.ToString());
                    Console.WriteLine(cnt);
                    Thread.Sleep(mSeconds - percentage);
                    watch.Reset();
                    watch.Start();
                }
            }
            Console.ReadKey();
        }
        public static void ConsumeCPUcount(long x, long y, bool print = false)
        {
            int[,] arr = new int[x, y];
            long result_ = 0;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (long i = 0; i < x; i++)
            {
                for (long i2 = 0; i2 < y; i2++)
                {
                    LoadCnt += 1;
                    arr[i, i2] = rnd.Next();
                    result_ += arr[i, i2];
                    if (print)
                    {
                        TraceLog.WriteLn(LoadCnt + " - " + arr[i, i2].ToString());
                    }
                }
            }
            watch.Stop();
            TraceLog.WriteLn("Elapsed :" + watch.Elapsed + " for " + x + " " + y + " With result:" + result_);
        }
    }

    public interface IlogParams
    {
        DateTime LogTime { get; set; }
        string Messsage { get; set; }
    }
    public class LogParams : IlogParams
    {
        public static int ID { get; set; }
        public DateTime LogTime { get; set; }
        public string Messsage { get; set; }
    }
    public static class StoreLog
    {
        internal static string LogFileName = @"Log.txt";
        internal static string LogFilePath = null;
        internal static IlogParams logParams = null;

        static StoreLog()
        {
            LogFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LogFileName);
            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath);
            }

        }
        public static void InterfaceBind(IlogParams input_)
        {
            logParams = input_;
        }
        public static void Log(string message_)
        {
            if (logParams != null)
            {
                logParams.LogTime = DateTime.Now;
                logParams.Messsage = message_;
                File.AppendAllText(LogFilePath, logParams.LogTime.ToString() + @" " + logParams.Messsage + Environment.NewLine);
            }
        }
    }

    public class Helper
    {
        public Random r = new Random();

        private List<int> intList = null;
        public Helper()
        {
            intListSampleInit(101);
        }

        public  void cout(string input_)
        {
            System.Diagnostics.Trace.WriteLine(input_);
        }
        public  void PrintList<T>(List<T> list_)
        {
            foreach(var item in list_)
            {
                System.Diagnostics.Trace.Write(item);
            }
        }

        public List<int> GetIntList()
        {
            if(this.intList == null)
            {
                intListSampleInit(100);
            }
            return this.intList;
        }
        public void PrintListInt()
        {
            System.Diagnostics.Trace.WriteLine(null);
            System.Diagnostics.Trace.Write(":>>");
            foreach (var a in intList.Select(s => s))
            {
                System.Diagnostics.Trace.Write(a + ",");
            }
            System.Diagnostics.Trace.Write(";");
            System.Diagnostics.Trace.WriteLine(null);
        }
        public void PrintListInt(int capacity_)
        {           
            PrintListInt();
        }


        public void intListSampleInit(int capacity)
        {
            this.intList = new List<int>();
            for (int i=0;i< capacity; i++)
            {
                this.intList.Add(i);
            }
        }        
        public void initListRandInt(int cpacity)
        {
            intList = new List<int>();
            for(int i = 0;i<= cpacity; i++)
            {
                intList.Add(r.Next());
            }
        }

    }


}