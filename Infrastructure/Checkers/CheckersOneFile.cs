namespace InfrastructureCheckers
{
    using System;
    using System.Linq;
    
    using Microsoft.EntityFrameworkCore;
    using mvccoresb.Infrastructure.EF;

    using System.Collections.Generic;

    using mvccoresb.Domain.TestModels;
    using mvccoresb.Domain.GeoModel;

    using AutoMapper;

    public static class RepoAndUOWCheck
    {

        static string connectionStringSQL = "Server=HP-HP000114\\SQLEXPRESS02;Database=EFdb;Trusted_Connection=True;";
        //static string connectionStringSQL = "Server=AAAPC;Database=testdb;User Id=tl;Password=QwErT123;";

        public static void GO(){
            RepoCheck();
            UOWCheck();
        }
        
        public static void RepoCheck(){
                      
            using(TestContext context = new TestContext(
                new DbContextOptionsBuilder<TestContext>()
                    .UseSqlServer(connectionStringSQL).Options))
            {
                mvccoresb.Infrastructure.EF.RepositoryEF repo = new mvccoresb.Infrastructure.EF.RepositoryEF(context);
                
                BlogEF b = new BlogEF() { Url = "url", Rating = 2 };
                repo.Add(b);
                repo.Save();

                List<BlogEF> blogs = repo.QueryByFilter<BlogEF>(s => s.BlogId!=null).ToList();
                repo.DeleteRange(blogs);
                repo.Save();

            }
        }

        public static void UOWCheck(){
            using (TestContext context = new TestContext(
                new DbContextOptionsBuilder<TestContext>()
                    .UseSqlServer(connectionStringSQL).Options))
            {
                mvccoresb.Infrastructure.EF.RepositoryEF repo = new mvccoresb.Infrastructure.EF.RepositoryEF(context);

                mvccoresb.Infrastructure.EF.UOWBlogging UOW = new UOWBlogging(repo);

                BlogEF b = new BlogEF() { Url = "url", Rating = 2 };
                UOW.AddBlog(b);

                List<BlogEF> blogs = repo.QueryByFilter<BlogEF>(s => s.BlogId != null).ToList();
                repo.DeleteRange(blogs);
            }
        }
    }
}


namespace NetPlatformCheckers
{

    using System;
    
    using System.Diagnostics;

    using System.Collections.Generic;

    using System.Text;
        
    using System.Net;
    using System.Net.Sockets;
    
    using System.Threading.Tasks;

    using System.IO;

    using System.Reflection;
    using System.Linq;

    using System.Runtime.CompilerServices;

    public static class Check
    {
        public static void GO()
        {
            EqualIsCheck.GO();
            OverrideCheck.GO();
            GenericSwapCheck.GO();
            DelegateCheck.GO();
            EventsCheck.GO();            
            ReflectionsCheck.GO();
        }
    }


    /* overriding */
    /*--------------------------------------------- */
    public class parent
    {

        public int ID { get; set; }
        public parent() { }
        public virtual string printV() { return "printed from base " + this.GetType().Name; }
        public string log() { return "logged from base " + this.GetType().Name; }
    }
    class child1 : parent
    {
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
    public static class OverrideCheck
    {
        public static void GO()
        {
           System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            parent parent = new parent();
            parent parentAsChild1 = new child1();
            parent parentAsChild2 = new child2();
            child1 child = new child1();


           System.Diagnostics.Trace.WriteLine(parent.printV()); //base
           System.Diagnostics.Trace.WriteLine(parent.log()); //base
           System.Diagnostics.Trace.WriteLine(parentAsChild1.printV()); //child1
           System.Diagnostics.Trace.WriteLine(parentAsChild1.log()); //base
           System.Diagnostics.Trace.WriteLine(parentAsChild2.printV()); //base
           System.Diagnostics.Trace.WriteLine(parentAsChild2.log()); //base
           System.Diagnostics.Trace.WriteLine(child.printV()); //child1
           System.Diagnostics.Trace.WriteLine(child.log()); //child1
        }
    }
    public static class EqualIsCheck
    {
        public static void GO(){
            
           System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            parent parent = new parent();
            parent parentAsChild1 = new child1();
            parent parentAsChild2 = new child2();
            child1 child = new child1();

            bool parentAsChild2IsEqualsParentAsChild1 = parentAsChild2.Equals(parentAsChild1);

            bool parentAsChild1IsParent = parentAsChild1 is parent;
            bool parentAsChild1IsChild1= parentAsChild1 is child1;

            bool childIsParent = child is parent;

           System.Diagnostics.Trace.WriteLine($"parentAsChild2IsEqualsParentAsChild1: {parentAsChild2IsEqualsParentAsChild1}");

           System.Diagnostics.Trace.WriteLine($"parentAsChild1IsParent: {parentAsChild1IsParent}");
           System.Diagnostics.Trace.WriteLine($"parentAsChild1IsChild1: {parentAsChild1IsChild1}");
           System.Diagnostics.Trace.WriteLine($"childIsParent: {childIsParent}");
        }
    }




    /** generic delegate swap*/
    /*--------------------------------------------- */
    public interface IEntityID
    {
         int ID {get;set;}
    }
    public class EntityForSwap : IEntityID
    {
        public int ID {get;set;}
    }
    public static class SwapG
    {
        public static void Sort<T>(IList<T> arr, Func<T, T, int> cmpr) where T : IEntityID
        {
            bool sort = true;
            while (sort)
            {
                sort = false;
                for (int i = 0; i < arr.Count - 1; i++)
                {
                    if (cmpr(arr[i], arr[i + 1])>0)
                    {
                        sort = true;
                        SwapG.swap<T>(arr, arr.IndexOf(arr[i]), arr.IndexOf(arr[i + 1]));
                    }
                }
            }
        }

        static void swap<T>(IList<T> arr, int i1, int i2)
        {
            T item;
            item = arr[i1];
            arr[i1] = arr[i2];
            arr[i2] = item;
        }

    }
    static class Comparers
    {
        public static int desc<T>(T itm1, T itm2) where T : IEntityID
        {
            if (itm1.ID > itm2.ID) { return 1; }
            if (itm1.ID < itm2.ID) { return -1; }            
            return 0;
        }
        public static int asc<T>(T itm1, T itm2) where T : IEntityID
        {
            if (itm1.ID < itm2.ID) { return 1; }
            if (itm1.ID > itm2.ID) { return -1; }            
            return 0;
        }
    }
    public static class GenericSwapCheck
    {
        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            List<EntityForSwap> arr = 
            new List<EntityForSwap>(){
                new EntityForSwap(){ID=0},
                new EntityForSwap(){ID=3},
                new EntityForSwap(){ID=5},
                new EntityForSwap(){ID=2},
                new EntityForSwap(){ID=1}
            };

            System.Diagnostics.Trace.WriteLine("before swap:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

            SwapG.Sort<EntityForSwap>(arr, Comparers.desc<IEntityID>);

            System.Diagnostics.Trace.WriteLine("after swap desc:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

            SwapG.Sort<EntityForSwap>(arr, Comparers.asc<IEntityID>);

            System.Diagnostics.Trace.WriteLine("after swap asc:");
            foreach (EntityForSwap p in arr) { Console.Write(p.ID); }

        }
    }





    /** delegate */
    /*--------------------------------------------- */
    public delegate string Del1(int i);

    /** shows named, anonimous and lambda anonimous delegate type invokation */
    public class DelegateInvokation
    {
        public static void GO()
        {
            //named method instance
            Del1 d11 = print;
           System.Diagnostics.Trace.WriteLine(d11.Invoke(2));
           System.Diagnostics.Trace.WriteLine(d11(3));

            //anonimous method instance
            Del1 d12 = delegate (int i) { return "Anonimous to str: " + i.ToString(); };
           System.Diagnostics.Trace.WriteLine(d12.Invoke(4));
           System.Diagnostics.Trace.WriteLine(d12(5));

            //lambda instance
            Del1 d13 = s => "Lambd to str:" + s.ToString();
           System.Diagnostics.Trace.WriteLine(d13.Invoke(6));
           System.Diagnostics.Trace.WriteLine(d13(7));

        }

        static string print(int i)
        {
            return "Int of str:" + i.ToString();
        }


    }

    /** binds unbinds methods to delegate varible handler */
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

    /** displays delegate receiver work with print methods */
    public class DelegateEmitter
    {
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
            string ret = "Print2 of str:" + i.ToString();
           System.Diagnostics.Trace.WriteLine(ret);
            return ret;
        }
        string print3(int i)
        {
            string ret = "Print3 of str:" + i.ToString();
           System.Diagnostics.Trace.WriteLine(ret);
            return ret;
        }
    }
   

    public class DelegatesArray
    {
        //delegate type declaration
        delegate void GetStringDel(string i);
        //variables of delegate type declaratoin
        GetStringDel PrintString;
        GetStringDel PrintString2;
        GetStringDel PrintString3;
        Random rnd = new Random();
        GetStringDel[] arr;
        public DelegatesArray()
        {

        }

        public void GO()
        {

            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            //delegate initializations
            //with method name		  
            PrintString = PrintStringA;

            //#2.0 anonimous init
            PrintString2 = delegate (string i) { System.Diagnostics.Trace.WriteLine(@"Anonimous init for: " + i); };

            //#3.0 lambda
            PrintString3 = (x) => { System.Diagnostics.Trace.WriteLine(@"Lambda init for: " + x); };

            //change method order at runtime from Rand values
            arr = new GetStringDel[rnd.Next(1, 10)];
            for (int i = 0; i < arr.Length; i++)
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

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].Invoke(" invoked from Arr in position " + i);
            }
        }
        public void PrintStringA(string i)
        {
            System.Diagnostics.Trace.WriteLine(" Method init for  : " + @" " + i);
        }

    }

    public static class DelegateCheck
    {
        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            DelegateInvokation.GO();

            DelegateEmitter dr = new DelegateEmitter();
            dr.GO();

            DelegatesArray delArray = new DelegatesArray();
            delArray.GO();
        }

    }



    /*events*/
    /*--------------------------------------------- */
    /*event argument classes */
    public class SpeedChangedEventArgs : EventArgs { public float speed { get; set; } }
    public class EngineBrokeEventArgs : EventArgs { public bool broken { get; set; } }



    /*events with named classes */


    /*
    class ->
    eventType : EventArgs {}

    class ->
    EventHadler<eventType> eventToSubscribeAndFire;

    handler method(eventType e) ->
    EventHadler<eventType> handler=eventToSubscribeAndFire;
    handler(this,e)

    emitter method ->
    eventType et = new eventType();
    handler method(et) 


    method ->
    Listen(this,e)


    eventToSubscribeAndFire+=Listen;
    */


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
            System.Diagnostics.Trace.WriteLine(@"Event raised for sender: " + sender + @"; e: " + e.ID + @" " + e.Name);
        }
    }
    public static class Event
    {
        public static void Check()
        {
            Receiver rc = new Receiver();
            Emitter em = new Emitter();
            em._handler += rc.ReceiveEvent;
            em.Add(1, @"Added");
        }
    }



    /*event emitter class */
    public class Car
    {

        //event handlers by classes
        public event EventHandler<SpeedChangedEventArgs> speedChangeEvent;
        public event EventHandler<EngineBrokeEventArgs> brokeChangeEvent;

        //event handler method
        public virtual void OnSpeedChanged(SpeedChangedEventArgs e)
        {
            EventHandler<SpeedChangedEventArgs> handler = speedChangeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public virtual void OnEngineBroke(EngineBrokeEventArgs e)
        {
            EventHandler<EngineBrokeEventArgs> handler = brokeChangeEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        float speed { get; set; }
        public string name { get; set; }

        //event emitting method
        public void speedIncrease(float speed_)
        {
            this.speed += speed_;

            SpeedChangedEventArgs args = new SpeedChangedEventArgs();
            args.speed = this.speed;

            OnSpeedChanged(args);
            engineCheck();
        }
        public void engineCheck()
        {
            if (this.speed > 100)
            {
                EngineBrokeEventArgs args = new EngineBrokeEventArgs();
                args.broken = true;
                OnEngineBroke(args);
            }
        }
    }
    public class SpeedListener
    {
        public void ListenToSpeed(object cr_, SpeedChangedEventArgs e)
        {
            if (cr_.GetType().Equals(typeof(Car)))
            {
                Car cr = (Car)cr_;
               System.Diagnostics.Trace.WriteLine($"Speed changed: { cr.name}  {e.speed}");
            }
        }
    }
    public class EngineListener
    {
        public void ListenToEngine(object o, EngineBrokeEventArgs e)
        {
            if (o.GetType().Equals(typeof(Car)))
            {
                Car cr = (Car)o;
               System.Diagnostics.Trace.WriteLine($"Car broke: { cr.name}  {e.broken}");
            }
        }
    }

    /*events with cancellation*/
    public class PrintOrCancell : EventArgs { public string toPrint { get; set; } public bool stop { get; set; } = false; }
    public class PrinterEmitter
    {
        public event EventHandler<PrintOrCancell> onPrint;

        public void print(List<string> toPrint)
        {
            foreach (string str_ in toPrint)
            {
                PrintOrCancell args = new PrintOrCancell() { toPrint = str_ };
                onPrint?.Invoke(this, args);
                if (args.stop)
                {
                    break;
                }
            }

        }

    }
    public class PrintListener
    {
        public void lsiten(object e, PrintOrCancell args)
        {
           System.Diagnostics.Trace.WriteLine(args.toPrint);
            if (args.toPrint.Length > 6) { args.stop = true; }
        }
    }

    /*updated event from core, not need to inherit from EventArgs*/
    public class CountOrCall { public int toCount { get; set; } public bool stop { get; set; } = false; }
    public class CountEmitter
    {
        public event EventHandler<CountOrCall> onCount;
        public void count(List<int> cnt_)
        {
            EventHandler<CountOrCall> handler = onCount;
            foreach (int i_ in cnt_)
            {
                CountOrCall args = new CountOrCall() { toCount = i_ };
                handler?.Invoke(this, args);
                if (args.stop)
                {
                    break;
                }
            }
        }
    }
    public class CountListener
    {
        public void listen(object o, CountOrCall e)
        {
            for (int i = 0; i < e.toCount; i++)
            {
                Console.Write(i);
            }
           
            if (e.toCount > 2) { e.stop = true; }
        }
    }

    public class Countlarge { public int? sum { get; set; } public List<int> arr { get; set; } public int[] arr_ { get; set; } }
    public class CountAsync
    {
        static int st = 0;
        public event EventHandler<Countlarge> onCnt;
        public void toCount(List<int> arr_)
        {
            EventHandler<Countlarge> handler = onCnt;
            Countlarge args = new Countlarge() { arr = arr_ };
            handler?.Invoke(this, args);
            while (args.sum == null)
            {
                logToConsole();
            }
            logResult(args.sum);
        }
        public void toCount(int[] arr__)
        {
            EventHandler<Countlarge> handler = onCnt;
            Countlarge args = new Countlarge() { arr_ = arr__ };
            handler?.Invoke(this, args);
            while (args.sum == null)
            {
                logToConsole();
            }
            logResult(args.sum);
        }

        void logToConsole()
        {
            char[] progress = new char[] { '\\', '|', '/', '-' };
            Console.Write("\r Solving: {0}, {1}", DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"), progress[st].ToString());
            if (st < progress.Length - 1) { st += 1; } else { st = 0; }
        }
        void logResult(int? res)
        {
            System.Diagnostics.Trace.Write($"result={res}");
        }
    }
    public class ListenerAsync
    {
        public async void Listen(object o, Countlarge args)
        {
            //arr vs arr_ for list , int[] inmpl
            try
            {
                int i = await Countlarge(args.arr);
                args.sum = i;
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message); }
        }
        async Task<int> Countlarge(List<int> amt)
        {
            int res = 0;
            await Task.Run(() =>
            {
                foreach (int i in amt)
                {
                    for (int i_ = 0; i_ < i; i_++)
                    {
                        res += 1;
                    }
                }
            });

            return res;
        }
        async Task<int> Countlarge(int[] amt)
        {
            int res = 0;
            int amtLen = amt.Length;
            await Task.Run(() =>
            {
                for (int i = 0; i < amtLen; i++)
                {
                    for (int i_ = 0; i_ < i; i_++)
                    {
                        res += 1;
                    }
                }
            });

            return res;
        }
    }

    /*events check*/
    public class EventsCheck
    {
        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            SampleEventCheck();
            CancelationCheck();
            UpdatedCoreEventCheck();
            //AsyncCheck();
            //NamedEventsCheck();
        }

        static void SampleEventCheck()
        {
           System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            Car car0 = new Car() { name = "car0" };
            Car car1 = new Car() { name = "car1" };
            SpeedListener sl = new SpeedListener();
            EngineListener el = new EngineListener();

            car0.speedChangeEvent += sl.ListenToSpeed;
            car0.brokeChangeEvent += el.ListenToEngine;

            car0.speedIncrease(10.0F);
            car0.speedIncrease(80.0F);
            car0.speedIncrease(11.0F);
        }
        static void CancelationCheck()
        {
           System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            List<string> strs = new List<string>() { "a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa" };
            PrinterEmitter pe = new PrinterEmitter();
            PrintListener pl = new PrintListener();
            pe.onPrint += pl.lsiten;
            pe.print(strs);
        }
        static void UpdatedCoreEventCheck()
        {
           System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            CountEmitter ce = new CountEmitter();
            CountListener cl = new CountListener();
            List<int> cnt = new List<int>() { 1, 2, 3, 4, 5, 6 };
            ce.onCount += cl.listen;
            ce.count(cnt);
        }
        static void AsyncCheck()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");            
            CountAsync ca = new CountAsync();
            ListenerAsync la = new ListenerAsync();
            ca.onCnt += la.Listen;

            List<int> intL = new List<int>();
            int[] intArr = new int[100001];

            for (int i = 0; i < 100000; i++) { intL.Add(i); intArr[i] = i; }

            //list implementation, need to e.arr change
            ca.toCount(intL);

            //arr implementation, need to e.arr_ change
            //ca.toCount(intArr);
        }
    
        static void NamedEventsCheck(){
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
            Receiver rc = new Receiver();
            Emitter em = new Emitter();
            em._handler += rc.ReceiveEvent;
            em.Add(1, @"Added");
        }
    }



    /*Interfaces */
    /*--------------------------------------------- */
    /*Explicit Initialization */    
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



    /*Indexer */
    /*--------------------------------------------- */
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



    /* Linked List */
    /*--------------------------------------------- */
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

    public static class LinkedListsCheck
    {
        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            PriorityDocumentManager.NodesInit();

            LinkedListSwitch ls = new LinkedListSwitch();
        }
    }


    
    /*--------------------------------------------- */
    public class StringBuilderChecker
    {
        
    }



    /*Async,Multithreading,Parallell*/    
    /*--------------------------------------------- */
    //ADD new
    


    /*Reflections */
    /*--------------------------------------------- */
    public class ModelForReflectionOne{
        public Guid Id {get;set;}
        public string Name {get;set;}
    }
    public class ModelForReflectionTwo{
        public Guid Id {get;set;}
        public string Name {get;set;}
        [New(AttrType = true)]
        public string Sername {get;set;}
    }
    //custom attribute class
    public class NewAttribute : Attribute
    {
        public bool AttrType { get; set; }
    }
    public class Reflections
    {

        public void LoopThroughtAssemblyReflections()
        {

            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            Assembly asm = Assembly.GetCallingAssembly();
            Type[] types_ = asm.GetTypes();
            List<Type> types_filtered = types_.Where(s=>s.IsAbstract == false && s.IsSealed == false)
            .Where(t => !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)).ToList();

            foreach (Type b in types_filtered)
            {
                //defining non static type
                if (b.IsClass && b.IsAbstract == false && b.IsSealed == false)
                {
                    var constructor = b.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                    null, Type.EmptyTypes, null);

                    if(constructor!=null){
                        var c = Activator.CreateInstance(b);
                        System.Diagnostics.Trace.WriteLine($"TypeName instatiated from asm: {c.GetType()}");
                    }
                }
            }
        }

        public void LoopThroughtProperties()
        {

            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            List<ModelForReflectionTwo> ClassCollection = new List<ModelForReflectionTwo>(){
                new ModelForReflectionTwo(){Id= new Guid(), Name ="Name1", Sername="Sername2"}
            };

            Type type = typeof(ModelForReflectionTwo);
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
                foreach (ModelForReflectionTwo item in ClassCollection)
                {
                    var d = item.GetType().GetProperty(c.Name).GetValue(item);                    
                    System.Diagnostics.Trace.WriteLine($"Property Name, Value: {c.Name}, {d}");
                }
            }
        }
    }

    public class ReflectionsCheck
    {
        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

            Reflections r = new Reflections();
            r.LoopThroughtAssemblyReflections();
            r.LoopThroughtProperties();
        }
    }



    /*Sockets */
    /*--------------------------------------------- */
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
                        System.Diagnostics.Trace.WriteLine(e.Message);
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
                    System.Diagnostics.Trace.WriteLine(e.Message);
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
                    System.Diagnostics.Trace.WriteLine(e.Message);
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
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }

                if (sendMessage.IndexOf("[FINAL]") > -1)
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }

        }

    }



    /*IO */
    /*--------------------------------------------- */
    /*drive read*/
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
                if (dirInf_.GetDirectories().Length != 0)
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


}


namespace LINQtoObjectsCheck
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using System.Threading;
    using System.Reflection;
    using System.Diagnostics;

    using System.IO;

    /*Models */
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


    
    
    public interface Iid{
        int ID{get;set;}
    }

    public static class Log
    {
        public static void ToLog(IEnumerable<object> item) 
        {
            foreach(var i in item)
            {
                Console.WriteLine(i);
            }
        }
    }
    
    public class User 
    {
        public int ID {get;set;}
        public string name {get;set;}
        public Address Address {get;set;}

        private Address addresPriv;
        public readonly Address addrRdn;

        public void BindPrivAddr(string name){
            this.addresPriv.name=name;
        }
    }

    public class Address : Iid
    {
        public int ID {get;set;}
        public string name {get;set;}

    }

    public class Facility
    {
        public int ID {get;set;}
        public string Name {get;set;}
        public IEnumerable<ServiceTypes> Services {get;set;}
        
    }




    public class ServiceTypes :Iid
    {
        public int ID {get;set;}
        public string Name {get;set;}
    }

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SomeDate { get; set; }
    };



    public class LinqCheck
    {
        public static List<Racer> racers = new List<Racer>();
        public static List<Cup> cups = new List<Cup>();

        static LinqCheck lc = new LinqCheck();

        public static void GO()
        {
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");

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
            var b = (from r in racers select r.Name).Except(from s in cups select s.RacerName);            var c = (from s in cups select new { s.RacerName, s.Position })
            .Zip(from r in racers select new { r.Car }, (z, x) => (z.RacerName + x.Car));

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

            // var g =
            //     from t in cups
            //     group t by new { t.Competition } into t2
            //     select new {Cup = t2.Key, Count = t2.Key.Count(), 
            //Racer = from s in t2 select s.RacerName };

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

            bulkCheck();
            UpdateCheck();
            UpdateNestedCheck();
            MaxDateCheck();
        }   

        
        public static void bulkCheck(){

            List<Address> addresses = new List<Address> (){
		    new Address(){ID=0,name="Name_0"},
		    new Address(){ID=1,name="Name_1"},
            new Address(){ID=4,name="Name_4"},
            new Address(){ID=5,name="Name_4"},
	        };
	
	        List<User> users = new List<User> (){
            new User(){ID=0,name="User_0",Address=addresses[0]},
            new User(){ID=1,name="User_1",Address=addresses[1]},
            new User(){ID=2,name="User_2",Address=null}
	        };
	
            var query =
                addresses.Join(users,
                            u => u.ID,
                            a => a.ID,
                            (u, a) =>
                                new { OwnerName = u.name, Pet = a.name });

            List<Address> usersNUll = null;
            if(usersNUll?.ToList().Any()==true)
            {

            }
            usersNUll = new List<Address>();
            if(usersNUll?.ToList().Any()==true)
            {

            }
            if(addresses?.ToList().Any()==true)
            {

            }

            //testListA.Where(a => !testListB.Any(b => a.ProductID == b.ProductID && a.Category == b.Category));
            
            List<Address> intersect = addresses.Where(a => users.Any(b => b.ID == a.ID)).ToList();
            List<Address> all = addresses.Where(a => users.Any(b => b.ID == a.ID)).ToList();
            List<Address> except = addresses.Where(a => !users.Any(b => b.ID == a.ID)).ToList();
            
            Log.ToLog(query);
        }

        public static void UpdateCheck(){

            List<Address> addrLess = new List<Address> (){
                new Address(){ID=0,name="Name_0"},
                new Address(){ID=1,name="Name_1"},
                new Address(){ID=2,name="Name_2"},
                new Address(){ID=3,name="Name_3"},
	        };
            List<Address> addrMore= new List<Address> (){             
                new Address(){ID=3,name="Name_3"},
                new Address(){ID=4,name="Name_4"},
                new Address(){ID=5,name="Name_5"},
                new Address(){ID=6,name="Name_6"}
	        };
            
            var upd = lc.UpdateExceptAdd<Address>(addrMore,addrLess);
        }
        
        public static void UpdateNestedCheck(){
            List<Facility> fac = lc.GenFacilities(5);
            
            List<ServiceTypes> serv = new List<ServiceTypes>(){
                new ServiceTypes(){ID=4,Name="name 4"}
                ,new ServiceTypes(){ID=5,Name="name 5"}
                ,new ServiceTypes(){ID=6,Name="name 6"}
            };

            fac.ForEach(s=> 
                s.Services=lc.UpdateExceptAdd<ServiceTypes>(serv,s.Services)
            );

        }

        public List<ServiceTypes> GenServices(int num){
            List<ServiceTypes> ret = new List<ServiceTypes>();
            for (int i=0;i<num;i++){
                ret.Add(new ServiceTypes(){ID=i, Name=i.ToString()});
            }
            return ret;
        }
        public List<Facility> GenFacilities(int num){
             List<Facility> ret = new List<Facility>();
            for (int i=0;i<num;i++){
                ret.Add(new Facility(){ID=i, Name=i.ToString(), Services = GenServices(i)});
            }
            return ret;
        }
      
        public IEnumerable<T> UpdateExceptAdd<T>(IEnumerable<T> from, IEnumerable<T> into)
         where T : class, Iid
         {
            var ret = new List<T>();
            var toDelete = into.Where(c => !from.Any(s => c.ID==s.ID)).ToList();
            var toAdd = from.Where(c => !into.Any(s => c.ID==s.ID)).ToList();

            into = into.Except(into.Where(c => !from.Any(s => c.ID==s.ID))).ToList();
            into.ToList().AddRange(toAdd.ToList());

            return into;
        }

        public class MaxDateByName{
            public string Name{get;set;}
            public DateTime MaxDate { get; set; }
        }
        public static void MaxDateCheck()
        {
            List<Item> items = new List<Item>(){
            new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,02,01)}
                ,new Item(){Name="Name0", SomeDate = new DateTime(2019,01,01)}
                
                ,new Item(){Name="Name1", SomeDate = new DateTime(2019,02,01)}
                ,new Item(){Name="Name1", SomeDate = new DateTime(2019,03,01)}
            };

            //select max date
            var maxDate = items.OrderByDescending(c => c.SomeDate).First().SomeDate;

            //select max date by group
            var maxDateByName = 
            items
            .Where(v => v.SomeDate!=null && v.Name != null)
            .GroupBy(s => s.Name)
            .Select(c => new {
                c.Key, LastDate = c.OrderByDescending(z=>z.SomeDate)
                .Select(x=>x.SomeDate)
                .FirstOrDefault()
            });
            
            //selecting name from annonimous
            var Name = maxDateByName.Where(s => s.Key !=null && s.LastDate != null).OrderByDescending(c =>c.LastDate).FirstOrDefault()?.Key;
         
        }

        //if a?.Any() == true  
        //if a?.Any() != true  
        public static void AnyCheckBool()
        {
            List<Item> items2 = new List<Item>()
            {
                new Item{Id=0,Name="nm0"}
                ,new Item{Id=1,Name="nm1"}
            };

            List<Item> itemsEmpty = new List<Item>();
            List<Item> itemsNull = null;


            if (items2?.Any() == true)
            {

            }

            if (itemsEmpty?.Any() == true)
            {

            }
            if (itemsEmpty?.Any() != true)
            {

            }

            if (itemsNull?.Any() == true)
            {

            }
            if (itemsNull?.Any() != true)
            {

            }

        }

        //if (!servicesBefore?.SequenceEqual(toUpdate?.ServiceTypes) == true)
        public static void SequenceCheck()
        {
            Item i0 = new Item { Id = 0, Name = "nm0" };
            Item i1 = new Item { Id = 1, Name = "nm1" };
            Item i2 = new Item { Id = 2, Name = "nm2" };

            List<Item> checkList = new List<Item>()
            {
               i0,i1,i2
            };

            List<Item> listNotEqual = new List<Item>()
            {
                i0,i2
            };

            List<Item> listEqual = new List<Item>()
            {
               i0,i1,i2
            };

            if (!checkList?.SequenceEqual(listEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listEqual) != true)
            {

            }

            if (!checkList?.SequenceEqual(listNotEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listNotEqual) == true)
            {

            }
            if (checkList?.SequenceEqual(listNotEqual) != true)
            {

            }

        }


        public static void ContainsCheck()
        {
            List<Item> itemsToUpdate = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm1"}
            };

            List<Item> newItems = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=3, Name = "Nm3"}
                ,new Item(){Id=4, Name = "Nm4"}
            };

            //3 4
            var itemsToAdd = newItems.Where(s => !itemsToUpdate.Exists(c => c.Id == s.Id));

            //2
            var itemsToRemove = itemsToUpdate.Where(s => !newItems.Exists(c => c.Id == s.Id));

            //0 1 3 4
            var newList = itemsToUpdate.Except(itemsToRemove).Union(itemsToAdd);

        }

        public static void WhereAnyContains(){

            List<Item> allIds = new List<Item>(){
                new Item(){Id=0, Name = "Nm0"}
                ,new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
                ,new Item(){Id=3, Name = "Nm3"}
                ,new Item(){Id=4, Name = "Nm4"}
            };

            List<Item> parentIds = new List<Item>(){
                new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
                ,new Item(){Id=3, Name = "Nm3"}
            };

            List<Item> servicesIds = new List<Item>(){
                  new Item(){Id=1, Name = "Nm1"}
                ,new Item(){Id=2, Name = "Nm2"}
            };
            var res = allIds.Where(x =>
                servicesIds.Where(s => parentIds.Exists(c => c.Id == s.Id))
                    .ToList().Exists(z => x.Id==z.Id)
            );

        }


    }

}


namespace TipsAndTricks
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    using System.Security.Cryptography;

    public static class TnT
    {
        public static void Check()
        {
            NullEquality();
            StructsCompare();
        }

        public static void NullEquality()
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

        /*Prints current executing class and method names */
        public static void PrintCurrentMethodAndClassName(){
            System.Diagnostics.Trace.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().DeclaringType}.{System.Reflection.MethodBase.GetCurrentMethod().Name}----------");
        }

    }

    //hash from different collections compare check
    //---------------------------------------------
    public static class HashCodeCheck
    {
        public static void GO()
        {

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


namespace KATAS{

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

    using System.ServiceProcess;

    using System.ServiceModel;


    using System.ComponentModel;

    using System.Net;
    using System.Net.Sockets;

    using System.Web.Http;


    //custom linq
    using System.Linq.Expressions;



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
    

}


namespace Rewrite
{
    
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

    using System.ServiceProcess;

    using System.ServiceModel;


    using System.ComponentModel;

    using System.Net;
    using System.Net.Sockets;

    using System.Web.Http;


    //custom linq
    using System.Linq.Expressions;

    /*StreamReadWrite */
    /*--------------------------------------------- */
    /// CopyPast class
    /// Parses class to txt with JSON
    /// Reads result JSON file to class and creates files from content
    /// Detects minimal single directory and recreates folder structure in new folder
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





    /*ConsoleParameters */
    /*--------------------------------------------- */
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




    /*StreamsTesting */
    /*--------------------------------------------- */

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


    



    /*CutomLinq */    
    /*--------------------------------------------- */
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

    
}

