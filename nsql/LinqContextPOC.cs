using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Reflection;

using Newtonsoft.Json;

using System.Diagnostics;

namespace LinqToContextPOC
{
    
    #region POC
    //testing different expression types
    public class TestContext
    {
        ParameterExpression leftParamExpr;
        Type leftType_=null;
        string memberName;
        Expression expr;

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
            Expression e0=BuildExpressionType(nodeType,leftExpr,rightParameter);
            Type tp3=e0.GetType();

            string ets=e0.ToString();
      
            string lb=this.VisitBinary((MemberExpression)binaryE.Left,"oper",binaryE);
            string lb2=this.VisitBinary(binaryE,"converted",leftExpr,nodeType,rightParameter);
            string lb3=this.BuildExpressionStr(nodeType,leftExpr,rightParameter);

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
        public Expression BuildExpressionType(ExpressionType nodeType_,Expression lt,Expression rt)
        {
            Expression res = null;
            if(nodeType_==ExpressionType.GreaterThan)
            {
                res = Expression.GreaterThan(lt, rt);
            }
            if(nodeType_==ExpressionType.GreaterThanOrEqual)
            {
                res = Expression.GreaterThanOrEqual(lt, rt);
            }
            if(nodeType_==ExpressionType.LessThan)
            {
                res = Expression.LessThan(lt, rt);
            }
            if(nodeType_==ExpressionType.LessThanOrEqual)
            {
                res = Expression.LessThanOrEqual(lt, rt);
            }
            if(nodeType_==ExpressionType.Assign)
            {
                res = Expression.Assign(lt, rt);
            }
            if(nodeType_==ExpressionType.Equal)
            {
                res = Expression.Equal(lt, rt);
            }
            if(nodeType_==ExpressionType.NotEqual)
            {
                res = Expression.NotEqual(lt, rt);
            }
            return res;
        }
        public string BuildExpressionStr(ExpressionType nodeType_,Expression lt,Expression rt)
        {
            string res = null;
            res = $"{lt}{nodeType_}{rt}";
            return res;
        }

        public string VisitBinary(MemberExpression binary,string @operator,BinaryExpression expression) =>
            $"{@operator}({binary},{expression})";
        public string VisitBinary(BinaryExpression expression,string op,Expression left,ExpressionType nt,Expression right) =>
            $"{expression}({op}),{left}|{nt}|{right}";
        public string VisitReturnString<T>(Expression<Func<T,bool>> expr_)
        {
            BinaryExpression be=(BinaryExpression)expr_.Body;
            string res=$"{be}";
            return res;
        }

        public string TraverseExpression<T>(Expression<Func<T,bool>> expr_)
        {
            //traversing expression
            Pv pv=new Pv();
            string traversed=pv.VisitBody(expr_);
            return traversed;
        }

        public string BuildExpressionManualy()
        {
            string res = string.Empty;
            ParameterExpression pe = Expression.Parameter(typeof(TestEntity), "te");
            ConstantExpression ce = Expression.Constant(new TestEntity() { Id = 0 }, typeof(TestEntity));
            BinaryExpression be = Expression.Equal(pe,ce);

            Expression<Func<TestEntity, bool>> lmb1 =
              Expression.Lambda<Func<TestEntity, bool>>(
              be,new ParameterExpression[]{pe}
            );
            
            res = lmb1.ToString();

            return res;
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
    
    //test POCO
    public class TestEntity
    {
      public string name {get;set;}
      static int id {get;set;}=0;
      public ToggleProp tp {get;set;}
      public int Id
      {
        get{ 
          if(id==0){id += 1;}
          return id; 
        }
        set{ id=value;}
      }
      public bool intrinsicIsTrue {get;set;}
    }
    //toggle property for test POCO
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
            return this.VisitGreaterOrEqual((BinaryExpression)node, expression);

          case ExpressionType.MemberAccess:
            return this.VisitMemberAccess((MemberExpression)node, expression);

          case ExpressionType.Constant:
            return this.VisitConstatnt((ConstantExpression)node, expression);

          default:
            throw new ArgumentOutOfRangeException(nameof(node));
        }

      }

      protected abstract TResult VisitEqual(BinaryExpression equal, LambdaExpression expression);
      protected abstract TResult VisitGreaterOrEqual(BinaryExpression equal, LambdaExpression expression);
      protected abstract TResult VisitMemberAccess(MemberExpression equal, LambdaExpression expression);
      protected abstract TResult VisitConstatnt(ConstantExpression equal, LambdaExpression expression);
    }
    internal class Pv : Ba<string>{
        protected override string VisitEqual
          (BinaryExpression add, LambdaExpression expression) => this.VisitBinary(add, "Equal", expression);

        protected override string VisitGreaterOrEqual
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


   

    internal interface ICommandBuilder
    {
        ICommandBuilder append(string res_);
        string GetCommand();

        ICommandBuilder Expand();

        ICommandBuilder Select();
        ICommandBuilder From();
        ICommandBuilder Where();
        ICommandBuilder And();

        ICommandBuilder Equal();
        ICommandBuilder EqualSign();
        ICommandBuilder GreaterThan();
        ICommandBuilder LessThan();

        ICommandBuilder @in();
        ICommandBuilder @out();
        
        ICommandBuilder RdBrLt();
        ICommandBuilder RdBrRt();

        ICommandBuilder SqBrLt();
        ICommandBuilder SqBrRt();

        ICommandBuilder Dot();
        ICommandBuilder Gap();
        ICommandBuilder Quote();
    }
    internal class Chaining : ICommandBuilder
    {
        StringBuilder sb;

        public Chaining()
        {
            sb = new StringBuilder();
        }

        public string GetCommand()
        {
            return sb.ToString();
        }

        public ICommandBuilder append(string res_)
        {
            sb.Append(res_);
            return this;
        }

        public ICommandBuilder Expand()
        {            
            sb.Append("expand");
            Gap();
            return this;
        }

        public ICommandBuilder Select()
        {
            sb.Append("Select");
            Gap();
            return this;
        }
        public ICommandBuilder From()
        {
            sb.Append("from");
            Gap();
            return this;
        }
        public ICommandBuilder Where()
        {
            sb.Append("where");
            Gap();
            return this;
        }
        public ICommandBuilder And()
        {
            Gap();
            sb.Append("and");
            Gap();
            return this;
        }
        
        public ICommandBuilder @in()
        {        
            Gap();
            sb.Append("in");            
            return this;
        }
        public ICommandBuilder @out()
        {
            Gap();
            sb.Append("out");            
            return this;
        }

        public ICommandBuilder Equal()
        {
            sb.Append("==");
            return this;
        }
        public ICommandBuilder EqualSign()
        {
            sb.Append("=");
            return this;
        }
        public ICommandBuilder GreaterThan()
        {
            sb.Append(">");
            return this;
        }
        public ICommandBuilder LessThan()
        {
            sb.Append(">");
            return this;
        }

        public ICommandBuilder RdBrLt()
        {
            sb.Append("(");
            return this;
        }
        public ICommandBuilder RdBrRt()
        {
            sb.Append(")");
            return this;
        }

        public ICommandBuilder SqBrLt()
        {
            sb.Append("[");
            return this;
        }
        public ICommandBuilder SqBrRt()
        {
            sb.Append("]");
            return this;
        }

        public ICommandBuilder Quote()
        {
            sb.Append("");
            return this;
        }

        public ICommandBuilder Dot()
        {
            sb.Append(".");
            return this;
        }
        public ICommandBuilder Gap()
        {
            sb.Append(" ");
            return this;
        }
    }
    
    //new builder
    internal class TokenBuilder
    {
        public List<IQueryManagers.ITypeToken> tokens
        = new List<IQueryManagers.ITypeToken>();
        
        StringBuilder sb = new StringBuilder();

        public TokenBuilder()
        {
            
        }
        
        public string GetCommand()
        {
            string res=null;
            foreach(IQueryManagers.ITypeToken tk in tokens){
                sb.Append(tk.Text);
                res = sb.ToString();
            }
            return res;
        }
        public IQueryManagers.ITypeToken NewToken(string res_)
        {
            QueryManagers.TextToken res=new QueryManagers.TextToken() { Text = res_ };
            return res;
        }

        //for single token
        public void AddRight(IQueryManagers.ITypeToken tk)
        {
            this.tokens.Add(tk);
        }
        public void AddLeft(IQueryManagers.ITypeToken tk)
        {
            this.tokens.Insert(0,tk);
        }
        
        //for collection of tokens
        public void AddLeft(List<IQueryManagers.ITypeToken> tk)
        {
            int cnt = tk.Count();
            for(int i=cnt;i>0;i--)
            {
                this.tokens.Insert(0,tk[i]);
            }
            
        }
        public void AddRight(List<IQueryManagers.ITypeToken> tk)
        {
            int cnt = tk.Count();
            for(int i=0;i<cnt;i++)
            {
                this.tokens.Add(tk[i]);
            }            
        }

        //for other builders addition
        public void AddLeft(TokenBuilder tb)
        {
            AddLeft(tb.tokens);
        }
        public void AddRight(TokenBuilder tb)
        {
            AddRight(tb.tokens);
        }
    }
    internal class ChainBuilder : ICommandBuilder
    {
        TokenBuilder cb;
                
        public ChainBuilder()
        {
            this.cb = new TokenBuilder();
        }
        
        public string GetCommand()
        {
            return cb.GetCommand();
        }
        public ICommandBuilder append(string input_)
        {
            cb.AddRight(cb.NewToken(input_));            
            return this;
        }      

        public ICommandBuilder Select()
        {
            cb.AddLeft(new OrientRealization.OrientSelectToken());
            cb.AddLeft(new OrientRealization.OrientGapToken());
            return this;
        }      
        public ICommandBuilder From()
        {
            cb.AddLeft(new OrientRealization.OrientFromToken());
            cb.AddLeft(new OrientRealization.OrientGapToken());
            return this;
        }      
        public ICommandBuilder Where()
        {
            cb.AddRight(new OrientRealization.OrientWhereToken());
            cb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }        
        public ICommandBuilder And()
        {
            cb.AddRight(new OrientRealization.OrientAndToken());
            cb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }
        
        public ICommandBuilder Equal()
        {
            cb.AddRight(new OrientRealization.OrientEqualsToken());            
            cb.AddRight(new OrientRealization.OrientEqualsToken()); 
            return this;
        }
        public ICommandBuilder EqualSign()
        {
            cb.AddRight(new OrientRealization.OrientEqualsToken());            
            return this;
        }
        public ICommandBuilder GreaterThan()
        {
            cb.AddRight(cb.NewToken(">"));
            return this;
        }
        public ICommandBuilder LessThan()
        {
            cb.AddRight(cb.NewToken("<"));
            return this;
        }       
        
        public ICommandBuilder @in()
        {
            cb.AddRight(new OrientRealization.OrientInToken());            
            return this;
        }
        public ICommandBuilder @out()
        {
            cb.AddRight(new OrientRealization.OrientOutToken());            
            return this;
        }

        public ICommandBuilder Expand()
        {
            throw new NotImplementedException();
        }

        public ICommandBuilder RdBrLt()
        {
            cb.AddRight(new OrientRealization.OrientRoundBraketLeftToken());            
            return this;
        }
        public ICommandBuilder RdBrRt()
        {
            cb.AddRight(new OrientRealization.OrientRoundBraketRightToken());            
            return this;
        }

        public ICommandBuilder SqBrLt()
        {
            cb.AddRight(new OrientRealization.OrientSquareBraketLeftToken());            
            return this;
        }
        public ICommandBuilder SqBrRt()
        {
            cb.AddRight(new OrientRealization.OrientSquareBraketRightToken());            
            return this;
        }
      
        public ICommandBuilder Dot()
        {
            cb.AddRight(new OrientRealization.OrientDotToken());            
            return this;
        }
        public ICommandBuilder Gap()
        {
            cb.AddRight(new OrientRealization.OrientGapToken());            
            return this;
        }

        public ICommandBuilder Quote()
        {
            cb.AddRight(new OrientRealization.OrientApostropheToken());            
            return this;
        }
      
    }

    //https://blogs.msdn.microsoft.com/mattwar/2007/07/31/linq-building-an-iqueryable-provider-part-ii/
    //custom expression traverse
    internal interface IExpressionVisitor
    {
        string Translate();
        void Visit<T>(Expression<Func<T, bool>> expr_);
        void VisitRelation<T>(Expression<Func<T, bool>> expr_);
        ICommandBuilder GetBuilder();
    }
    internal class ExpressionVisitorCustom : ExpressionVisitor,IExpressionVisitor
    {
        ICommandBuilder cb;
        
        PropertyReader pr = new PropertyReader();

        Stack<string> nestedProperties;
        public ExpressionVisitorCustom(ICommandBuilder cb_)
        {
            this.cb = cb_;
            nestedProperties = new Stack<string>();
        }
        public ICommandBuilder GetBuilder()
        {
            if(this.cb!=null)
            {
                return this.cb;
            }
            throw new Exception("No command builder");
        }
        
        public void Visit<T>(Expression<Func<T,bool>> expr_)
        {
            Type expType = expr_.GetType();
            Type lmdTp = expr_.ReturnType;
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Expression db = expr_.Body;

            Visit(db);
        }
        public new void Visit(Expression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;

            if(ndType==ExpressionType.Equal){ VisitEqual((BinaryExpression)expr_); }
            if(ndType==ExpressionType.GreaterThan){ VisitGreaterThan((BinaryExpression)expr_); }

            if(ndType==ExpressionType.Constant){ VisitConstant((ConstantExpression)expr_); }
            if(ndType==ExpressionType.MemberAccess){ VisitMember((MemberExpression)expr_); }

            StackToStringBuilder();
        }

        public void VisitRelation<T>(Expression<Func<T,bool>> expr_)
        {
            Type expType = expr_.GetType();
            Type lmdTp = expr_.ReturnType;
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Expression db = expr_.Body;

            VisitRelation(db);
        }
        public void VisitRelation(Expression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;

            if(ndType==ExpressionType.Equal){ VisitEqual((BinaryExpression)expr_); }
            if(ndType==ExpressionType.GreaterThan){ VisitGreaterThan((BinaryExpression)expr_); }

            if(ndType==ExpressionType.Constant){ VisitConstant((ConstantExpression)expr_); }
            if(ndType==ExpressionType.MemberAccess){ VisitMember((MemberExpression)expr_); }

            StackToStringBuilder();
        }

        public void VisitEqual(BinaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Visit(expr_.Left);
            cb.EqualSign();
            Visit(expr_.Right);
        }
        public void VisitGreaterThan(BinaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Visit(expr_.Left);
            cb.GreaterThan();
            Visit(expr_.Right);
        }

        public new void VisitConstant(ConstantExpression expr_)
        {
            //cb.append("constantExpr Value,Type=");
            cb.append(expr_.Value.ToString());
            //cb.append(expr_.Type.Name);            
        }
        public new void VisitMember(MemberExpression expr_)
        {
            //cb.append("memberExpr Type,Member=");
            //cb.append(expr_.Type.Name);
            MemberInfo mi=expr_.Member;

            if(JsonPropertyAttribute.IsDefined(mi,typeof(JsonPropertyAttribute)))
            {
                Attribute atr= pr.IsUsingAttribute<JsonPropertyAttribute>(mi);
                Attribute atr2= JsonPropertyAttribute.GetCustomAttribute(mi, typeof(JsonPropertyAttribute));
                string res=pr.GetJsonPropertyName(atr);
                nestedProperties.Push(res);
            }else{            
                string mn=expr_.Member.ReflectedType.Name;            
                nestedProperties.Push(expr_.Member.Name);
            }
            Visit(expr_.Expression);
        }
        public void StackToStringBuilder()
        {
            while(nestedProperties.Count()!=0){
                cb.append(nestedProperties.Pop());
                if(nestedProperties.Count()!=0){
                  cb.Dot();
                }
            }
        }
        public string Translate()
        {
            return cb.GetCommand();
        }      
    }
    internal class PropertyReader
    {
        public T IsUsingAttribute<T>(PropertyInfo p_) 
        where T:System.Attribute
        {
            if(p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).Any()){ 
                return (T)p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).FirstOrDefault();
            }
            return null;
        }
        public T IsUsingAttribute<T>(MemberInfo p_) 
        where T:System.Attribute
        {
            if(p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).Any()){ 
                return (T)p_.GetCustomAttributes().Where(s=>s.GetType().Equals(typeof(T))).FirstOrDefault();
            }
            return null;
        }
        public string GetJsonPropertyName(Attribute atr)
        {
            string res=string.Empty;
            if(atr.GetType()==typeof(JsonPropertyAttribute)){
                JsonPropertyAttribute jp=(JsonPropertyAttribute)atr;
                res = jp.PropertyName;
            }
            return res;
        }
    }
    internal static class ExpressionVisitorFactory
    {
        internal static IExpressionVisitor NewVisitor(ICommandBuilder cb)
        {
            return new ExpressionVisitorCustom(cb);
        }
    }

    //repository
    internal class ExpressionRepository
    {
        IExpressionVisitor expressionVisitor;
        ICommandBuilder cb;      

        public ExpressionRepository(IExpressionVisitor ie_)
        {
            this.expressionVisitor = ie_;
            cb = expressionVisitor.GetBuilder();
        }
        public string Translate()
        {
            return expressionVisitor.Translate();
        }      

        public ExpressionRepository Where<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Where();
            expressionVisitor.Visit(expr_);
            
            return this;
        }
        public ExpressionRepository And<T>(Expression<Func<T,bool>> expr_)
        {
            cb.And();
            expressionVisitor.Visit(expr_);            
            return this;
        }
        public ExpressionRepository And()
        {
            cb.And();
            return this;
        }
        public ExpressionRepository @in<T>(Expression<Func<T,bool>> expr_)
        {
            cb.@in();
            cb.RdBrLt();
            cb.Quote();
            cb.append(typeof(T).Name);
            cb.Quote();
            cb.RdBrRt();

            cb.SqBrLt();
            cb.append("0");
            cb.SqBrRt();

            cb.Dot();

            expressionVisitor.VisitRelation<T>(expr_);
            return this;
        }
    }
    internal class ExpressionChainVisitor
    {
        IExpressionVisitor expressionVisitor;
        ICommandBuilder cb;      

        public ExpressionChainVisitor(IExpressionVisitor ie_)
        {            
            expressionVisitor = ie_;
            cb = expressionVisitor.GetBuilder();
        }
        public string Translate()
        {
            return cb.GetCommand();
        }      

        public ExpressionChainVisitor Where<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Where();
            expressionVisitor.Visit(expr_);
            
            return this;
        }
        public ExpressionChainVisitor And<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Gap();
            cb.And();
            expressionVisitor.Visit(expr_);            
            return this;
        }
        public ExpressionChainVisitor And()
        {
            cb.Gap();
            cb.And();
            return this;
        }
        public ExpressionChainVisitor @in<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Gap();
            cb.@in();
            cb.RdBrLt();
            cb.Quote();
            cb.append(typeof(T).Name);
            cb.Quote();
            cb.RdBrRt();

            cb.SqBrLt();
            cb.append("0");
            cb.SqBrRt();

            cb.Dot();

            expressionVisitor.VisitRelation<T>(expr_);
            return this;
        }
    }

    //check Linq to context
    public static class LinqToContextCheck
    {
        //testing of Expression traversing and nodes visiting
        public static void TestContextCheck()
        {        
            TestContext ts = new TestContext();
            TestEntity te=new TestEntity() {Id=0,name=null};
            var a = te?.name;

            ts.ExpressionBuild();

            string st4=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.tp.isTrue==false);
            string st1=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.Id>=1);      

            string st2=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.name=="test name");
            string st3=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.intrinsicIsTrue==true);
            
            string returnedString=ts.VisitReturnString<TestEntity>(s=>s.name=="tn");

            string traverseStr = ts.TraverseExpression<TestEntity>(s => s.tp.isTrue == false);
            
            string manualExpressionStr = ts.BuildExpressionManualy();

        }
        //
        public static void ExpressionVisitorCustomCheck()
        {
            Chaining ch = new Chaining();
            ChainBuilder cb = new ChainBuilder(); 
            
            ExpressionRepository er = new ExpressionRepository(ExpressionVisitorFactory.NewVisitor(ch));

            ExpressionChainVisitor evc = new ExpressionChainVisitor(ExpressionVisitorFactory.NewVisitor( new ChainBuilder()));
            evc.@in<POCO.Authorship>(s => s.id == "'25:26'");
            string res = evc.Translate();
            
            evc = new ExpressionChainVisitor(ExpressionVisitorFactory.NewVisitor( new ChainBuilder()));
            evc.Where<POCO.News>(s => s.pinned.isTrue==false).And<POCO.News>(s=>s.name=="'NewsOne'").And().@in<POCO.Authorship>(s=>s.id=="'25:27'");
            string res2 = evc.Translate();

            evc = new ExpressionChainVisitor(ExpressionVisitorFactory.NewVisitor( new ChainBuilder()));
            er.@in<POCO.Authorship>(s=>s.GUID=="'25:28'");
            er.Where<POCO.News>(s => s.pinned.isTrue==false).And<POCO.News>(s=>s.name=="'NewsOne'").@in<POCO.Authorship>(s=>s.id=="'25:29'");
            string res3 = er.Translate();
        }
        public static string ListInsertVsRearrange()
        {
            string res = string.Empty;
            List<string> str1 = new List<string>();
            List<string> str2 = new List<string>();
            List<CheckResult> checkRes = new List<CheckResult>();

            List<string> strToIns = new List<string>(){"111","112","113"};

            string insertElapsed,addElapsed,
            insertRangeElps,addRangeElp;

            for(int i =0;i<10000000;i++)
            {
                str1.Add("str "+i.ToString());
                str2.Add("str "+i.ToString());
            }

            Stopwatch sw = new Stopwatch();
            
            sw.Reset();
            sw.Start();
            str1.Insert(0, "-1");
            sw.Stop();

            insertElapsed = sw.Elapsed.ToString();
            checkRes.Add(new CheckResult() { MethodName = "insert", elapsed = sw.Elapsed });

            sw.Reset();
            sw.Start();
            List<string> str3 = new List<string>();
            str3.Add("-2");
            str3.AddRange(from s in str2 select s);
            str2 = str3;
            sw.Stop();
            addElapsed = sw.Elapsed.ToString();
            checkRes.Add(new CheckResult() { MethodName = "add", elapsed = sw.Elapsed });

            sw.Reset();
            sw.Start();
            foreach(string str in strToIns){
                str1.Insert(0, str);}
            sw.Stop();
            insertRangeElps = sw.Elapsed.ToString();
            checkRes.Add(new CheckResult() { MethodName = "insert large", elapsed = sw.Elapsed });

            sw.Reset();
            sw.Start();
            str3 = new List<string>();
            str3.Add("-2");
            str3.AddRange(from s in strToIns select s);
            str3.AddRange(from s in str2 select s);
            str2 = str3;
            sw.Stop();
            addRangeElp = sw.Elapsed.ToString();
            checkRes.Add(new CheckResult() { MethodName = "add large", elapsed = sw.Elapsed });

            res = $"Insert elapsed:{insertElapsed}; AddElapsed:{addElapsed}; Insert range:{insertRangeElps}; Insert range add:{addRangeElp}" ;
            System.Diagnostics.Trace.WriteLine(res);

            return res;
        }
        public class CheckResult
        {
            public string MethodName { get; set; }
            public TimeSpan elapsed { get; set; }
        }
        
        public static void GO()
        {
            //TestContextCheck();
            //string elapsed=ListInsertVsRearrange();
            ExpressionVisitorCustomCheck();
        }
    
    }

}
