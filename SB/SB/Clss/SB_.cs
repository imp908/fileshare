using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace SB_
{
    public static class DW
    {
        public static void cout(string str_)
        {
            System.Diagnostics.Debug.WriteLine(str_);
        }
    }


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