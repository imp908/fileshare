using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Reflection;
using System.Diagnostics;

using System.IO;

using Newtonsoft.Json;

namespace SB_
{

    public static class DW
    {
        public static void cout(string str_)
        {
            System.Diagnostics.Debug.WriteLine(str_);
        }
    }


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
            DW.cout("Del3 inputed " + input_);
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
            DW.cout("Delegates invoked");
            del.Invoke();

            DW.cout(del2_());
            DW.cout(del2_.Invoke());

            del3_.Invoke("del 3 input");
        }
        //invokation with action
        public static void delegatesAction()
        {
            DW.cout("Delegate actions invoked");
            Action act = delMeth1;
            act += delMeth1_;
            act.Invoke();
        }
        //invokation from array
        public static void delegateArray()
        {
            DW.cout("Delegates array");
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
        delegate void GetStringDel(string i);
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

            for(int i =0;i<input_.Length; i++)
            {
                if(EncoutedType(input_[i]) == typeof(Number))
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
        public char[] ParseDigit(int start_ )
        {
            char[] _result = new char[0];
            
            if (start_ <= input_.Length  )
            {
                if (EncoutedType(input_[start_]) == typeof(Number) )
                {
                    int curpos = input_.Length;
                    Token token_ = null;                  
                    bool rightGap = false;

                    for (int i = start_;i < input_.Length;i++)
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
                        _result = new char[curpos+1 - i];
                        i += 1;
                        for (int i3 = 0; i3 < curpos+1 - i; i3++)
                        {
                            _result[i3] = input_[i-1+i3];
                        }
                            
                        i = curpos+1;
                        
                    }
                                        
                }
            }
            
            return _result;

        }

        //gets type of char
        public Type EncoutedType(char ch_)
        {
            Type _result = null;

            if (SignDictionary.GetToken(ch_) != null) {
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

        public Tree LeftNode { get ; set; }
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

            foreach(KeyValuePair<char[], Token> kvp in Dictionary_)
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
            if(!this.Dictionary_.Where(s=>s.Key==ch_).Any())
            {
                Dictionary_.Add(ch_, value_);
            }
        }
        public Token GetToken(char[] ch_)
        {
            Token _result = null;

            if(Dictionary_.Where(s => s.Key.SequenceEqual(ch_)).Any())
            {
                _result = Dictionary_.Where(s => s.Key.SequenceEqual(ch_)).FirstOrDefault().Value;
            }

            return _result;
        }
        public Token GetToken(char ch_)
        {
            Token _result = null;
            char[] _ch = new char[1] { ch_ } ;
           
            _result = GetToken(_ch);
                    
            return _result;
        }

    }
  
    public delegate Tree OperationDel (Token tl, Token rt, SUMM op);
    
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
        public TokenDouble TokenSumDouble(TokenDouble tl,TokenDouble tr, SUMM op)
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
        public Tree TreeSumming(Tree t1,Tree t2, IOperation o)
        {
            if(t1.simplified)
            {
                if(t2.simplified)
                {
                    if(o is SUMM)
                    {

                    }
                }
            }

            return _result;
        }
    }
    
    #endregion
  
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
                container_.StringToItemParse(@"0.123*345-1=3*b");
                int _actual = (from s in container_.parsed select s).Count();

                ds.Add(new ItemsStatus() { Equation = @"1+1", Items = 3 });
                ds.Add(new ItemsStatus() { Equation = @"1+1=2", Items = 5 });

                foreach (ItemsStatus ds_ in ds)
                {
                    container_.ParsedInit();
                    container_.StringToItemParse(ds_.Equation);
                    try
                    {
                        ds_.Compare(container_.parsed.Count());
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
        /// </summary>

        public List<IPriority> _priority = new List<IPriority>();
        public List<IItem_> operations = new List<IItem_>();
        public List<IItem_> items = new List<IItem_>();

        public List<IItem_> parsed;

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
             
                parsed.Add(actReadItem);
               
            }

        }

        public void ItemsParse()
        {
        
            IItem_ prevReadItem = null;
            IItem_ actReadItem = null;

            foreach (IItem_ item_ in parsed)
            {
                actReadItem = item_;

                //decide Item Expression
                if (prevReadItem == null)
                {
                    //first encounter or after bracket
                    prevReadItem = actReadItem;
                }
                else
                {
                    //previous digit
                    if (actReadItem.GetType() is IDigit_)
                    {

                    }
                    //previous variable
                    if (actReadItem.GetType() is IVariable_)
                    {

                    }
                }
              
            }

            IItem_[] arr = new IItem_[1] { actReadItem };
            _priority.Add(new Priority() { leftItem = new Exp_() { body = arr } });
        }

        public void ParsedInit()
        {
            this.parsed = new List<IItem_>();
        }
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
            //ReadFilesCheck();
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
            //string pathResult_ = @"C:\FILES\SHARE\debug\moove\new\result.txt";
            string pathResult_ = @"C:\111\moove\result.txt";

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
                File.WriteAllText(pd_.Path, Encoding.UTF8.GetString(Encoding.UTF8.GetBytes((pd_.Text))));
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

    #region StreamsTesting

    public static class StreamTesting
    {
        static string FilePathIn { get; set; }
        static string DirectoryIn { get; set; }
        static string FilePathOut { get; set; }
        static string FileNameOut { get; set; }
        static string DirectoryOut { get; set; }
        static FileStream fs { get; set; }
        static FileStream fsOut { get; set; }
        static byte[] arr { get; set; }
        static int length { get; set; }
        static int pos { get; set; }

        static StreamReader sr;
        static int cnt;
        static int b = 0;
        static string a = null;
        static string Newpath = null;

        static List<PathText> pathTextList = new List<PathText>();
        public static void Check()
        {
            DirectoryIn = @"C:\111\moove\";
            DirectoryOut = @"C:\111\moove\output";
            FilePathIn = @"C:\111\moove\result.txt";
            FileNameOut = @"out.txt";
            FilePathOut = Path.Combine(DirectoryOut, FileNameOut);
            Serialize_();
            Deserialize_();
        }
        public static void StreamWrite()
        {
            arr = new byte[length];
            fs.Read(arr, 0, length);           
            fsOut.Write(arr, 0, length);            

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


        public static void Serialize_()
        {            
            fsOut = new FileStream(FilePathOut, FileMode.OpenOrCreate);
            foreach (string filename in Directory.GetFiles(DirectoryIn))
            {
                FilePathIn = filename;
                fs = new FileStream(FilePathIn, FileMode.Open);                
                FileNameOut = Path.GetFileNameWithoutExtension(FilePathIn) + @"_copy" + Path.GetExtension(FilePathIn);
                length = filename.Length;
                
                arr = Encoding.UTF8.GetBytes(length.ToString());
                Write_(arr);

                arr = Encoding.UTF8.GetBytes(filename);
                Write_(arr);

                length = (int)fs.Length;
                arr = Encoding.UTF8.GetBytes(length.ToString());
                Write_(arr);

                StreamWrite();
                WriteNewLine();

                fs.Close();               
            }
            fsOut.Close();
        }
        public static void Deserialize_()
        {
            PathTextFill();
            PathtextRead();

        }

        public static void PathTextFill()
        {
            fs = new FileStream(FilePathOut, FileMode.Open);
            sr = new StreamReader(fs);

            while (fs.Position + 1 < fs.Length)
            {
                string path = GetString();
                string text = GetString();
                pathTextList.Add(new PathText(Path.Combine(DirectoryOut ,Path.GetFileName(path)) , Encoding.UTF8.GetBytes(text)));
            }
        }
        public static void PathtextRead()
        {
            
            foreach(PathText pt_ in pathTextList)
            {
                FileStream fss = new FileStream(pt_.path, FileMode.OpenOrCreate);             
                fss.Write(pt_.text, 0, pt_.text.Length);
                fss.Close();
            }
        }
        public static string GetString()
        {
            string _Newpath = null;
            cnt = 0;
            while (fs.ReadByte() != 13)
            {
                if (fs.Position >= fs.Length) break;
                cnt += 1;
            }
            fs.Position -= cnt + 1;
            arr = new byte[cnt];
            fs.Read(arr, 0, cnt);
            a = Encoding.UTF8.GetString(arr);
            Int32.TryParse(a, out b);
            arr = new byte[b];
            fs.Position += 1;
            fs.Read(arr, 0, b);
            _Newpath = Encoding.UTF8.GetString(arr);
            fs.Position += 1;

            return _Newpath;
        }
    }

    public class PathText
    {
        public string path { get; set; }
        public byte[] text { get; set; }
        public PathText(string path_, byte[] text_)
        {
            this.path = path_;
            this.text = text_;
        }
    }
    #endregion

}

namespace SB_
{
    public static class TraceLog
    {
        internal static Random rnd = new Random();
        internal static long LoadCnt=0;
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
            while (cnt<iterations)
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
        public static void ConsumeCPUcount(long x,long y,bool print = false)
        {
            int[,] arr = new int[x,y];
            long result_ = 0;
            
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (long i=0;i<x;i++)
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
            TraceLog.WriteLn("Elapsed :" + watch.Elapsed + " for " + x + " " + y + " With result:"+ result_);
        }       
    }
}