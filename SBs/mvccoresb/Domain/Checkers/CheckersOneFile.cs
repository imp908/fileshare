
namespace NetPlatformCheckers
{

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class Check
    {
        public static void GO()
        {
            EqualIsCheck.GO();
            OverrideCheck.GO();
            GenericSwapCheck.GO();
            DelegateCheck.GO();
            EventsCheck.GO();
            //GenericvalueItemsCHeck.GO();
            
        }
    }





    /* overriding ---------------------------------------------
        prent 
            virtual m()
        child 
            override m() 		
        parent o = new child
            o.m - > parent realization
    */
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
    public static class EqualIsCheck{
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




    /** generic delegate swap --------------------------------------------- */
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





    /** delegate ---------------------------------------------
    
    */
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



    /*events --------------------------------------------- */
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
            AsyncCheck();
            NamedEventsCheck();
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
            PriorityDocumentManager.NodesInit();

            LinkedListSwitch ls = new LinkedListSwitch();
        }
    }


    public class StringBuilderChecker
    {
        
    }

}

namespace TipsAndTricks{
    using System;

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
}