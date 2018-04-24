using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;
using System.Reflection;

using Newtonsoft.Json;

using System.Diagnostics;

using System.Configuration;

namespace LinqToContextPOC
{

    /// <summary>
    /// Custom linq proof of concept. traversing expression treee with custom strings.
    /// </summary>
    #region POC
    //testing different expression types
    public class TestContext
    {
        ParameterExpression leftParamExpr;
        Type leftType_=null;       

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

    
    public interface ITokenBuilder
    {
        string GetCommand();
        List<IQueryManagers.ITypeToken> GetTokens();
        IQueryManagers.ITypeToken NewToken(string res_);
        void Refresh();

        void AddLeft(IQueryManagers.ITypeToken tk);
        void AddRight(IQueryManagers.ITypeToken tk);
        
        void AddLeft(List<IQueryManagers.ITypeToken> tk);
        void AddRight(List<IQueryManagers.ITypeToken> tk);
       
        void AddLeft(ITokenBuilder tk);
        void AddRight(ITokenBuilder tk);
    }
    internal interface ITokenChain
    {
        string GetCommand();
        ITokenBuilder GetTokenBuilder();
        void Refresh();

        ITokenChain AddRight(string res_);
        ITokenChain AddLeft(string res_);

        ITokenChain AddLeft(IQueryManagers.ITypeToken input_);
        ITokenChain AddRight(IQueryManagers.ITypeToken input_);

        ITokenChain AddLeft(ITokenChain input_);
        ITokenChain AddRight(ITokenChain input_);

        ITokenChain Expand();
    }
    internal interface ITokenQueryChain : ITokenChain
    {             
        ITokenChain Select();
        ITokenChain From();
        ITokenChain Where();
        ITokenChain WhereParam();
        ITokenChain And();
        ITokenChain Or();
        ITokenChain AndParam();

        ITokenChain Equal();
        ITokenChain NotEqual();
        ITokenChain EqualSign();
        ITokenChain GreaterThan();
        ITokenChain LessThan();
        ITokenChain GreaterOrEqualThan();
        ITokenChain LessOrEqualThan();   

        ITokenChain @in();
        ITokenChain @out();

        ITokenChain E();
        ITokenChain V();

        ITokenChain RdBrLt();
        ITokenChain RdBrRt();

        ITokenChain SqBrLt();
        ITokenChain SqBrRt();

        ITokenChain Dot();
        ITokenChain Gap();

        ITokenChain Quote();
        ITokenChain Comma();
    }
    internal interface IChainBuilderFactory
    {
        ITokenChain GetChainBuilder();
        ITokenQueryChain GetQueryChainBuilder();
    }

    internal class ChainBuilderFactory : IChainBuilderFactory
    {
        public ITokenChain GetChainBuilder()
        {
            return new TokenChain();
        }
        public ITokenQueryChain GetQueryChainBuilder() {
            return new TokenQueryChain();
        }
    }
    /// <summary>
    /// Build tokens to collection from tokens,or from collection or from builder with left or right padding
    /// </summary>
    internal class TokenBuilder : ITokenBuilder
    {
        public List<IQueryManagers.ITypeToken> tokens
        = new List<IQueryManagers.ITypeToken>();
        
        StringBuilder sb = new StringBuilder();

        public TokenBuilder(){
            
        }
        public void Refresh(){
            this.tokens.Clear();
            sb.Clear();
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
        public List<IQueryManagers.ITypeToken> GetTokens()
        {
            return this.tokens;
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
            for(int i=cnt-1;i>=0;i--)
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
        public void AddLeft(ITokenBuilder tb)
        {
            AddLeft(tb.GetTokens());
        }
        public void AddRight(ITokenBuilder tb)
        {
            AddRight(tb.GetTokens());
        }
    }
    /// <summary>
    /// Main token chaining operaions, add left,right,expand
    /// </summary>
    internal class TokenChain : ITokenChain
    {
        internal ITokenBuilder tb;

        public TokenChain()
        {
            this.tb = new TokenBuilder();
        }

        public string GetCommand()
        {
            return tb.GetCommand();
        }
        public ITokenBuilder GetTokenBuilder()
        {
            return tb;
        }
        public void Refresh()
        {
            tb.Refresh();
        }

        public ITokenChain AddRight(string input_)
        {
            tb.AddRight(tb.NewToken(input_));
            return this;
        }
        public ITokenChain AddLeft(string input_)
        {
            tb.AddLeft(tb.NewToken(input_));
            return this;
        }
        public ITokenChain AddRight(IQueryManagers.ITypeToken input_)
        {
            tb.AddRight(input_);
            return this;
        }
        public ITokenChain AddLeft(IQueryManagers.ITypeToken input_)
        {
            tb.AddLeft(input_);
            return this;
        }
        public ITokenChain AddRight(ITokenChain input_)
        {
            tb.AddRight(input_.GetTokenBuilder());
            return this;
        }
        public ITokenChain AddLeft(ITokenChain input_)
        {
            tb.AddLeft(input_.GetTokenBuilder());
            return this;
        }

        public ITokenChain Expand()
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Chains tokens via Tokenbuilder to command, with command specific tokens. Nesting realization (From,expand,traverse).
    /// </summary>
    internal class TokenQueryChain : TokenChain, ITokenQueryChain
    {       

        public TokenQueryChain()
        {
            //tb = new TokenBuilder();
        }
      
        public ITokenChain Select()
        {
            tb.AddLeft(new OrientRealization.OrientGapToken());
            tb.AddLeft(new OrientRealization.OrientSelectToken());            
            return this;
        }      
        public ITokenChain From()
        {
            tb.AddRight(new OrientRealization.OrientGapToken());
            tb.AddRight(new OrientRealization.OrientFromToken());
            tb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }      
        public ITokenChain WhereParam()
        {
            tb.AddLeft(new OrientRealization.OrientGapToken());
            tb.AddLeft(new OrientRealization.OrientWhereToken());
            tb.AddLeft(new OrientRealization.OrientGapToken());
            return this;
        }        
        public ITokenChain Where()
        {
            tb.AddRight(new OrientRealization.OrientGapToken());
            tb.AddRight(new OrientRealization.OrientWhereToken());
            tb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }  
        public ITokenChain AndParam()
        {
            tb.AddLeft(new OrientRealization.OrientGapToken());
            tb.AddLeft(new OrientRealization.OrientAndToken());
            tb.AddLeft(new OrientRealization.OrientGapToken());
            return this;
        }
        public ITokenChain And()
        {
            tb.AddRight(new OrientRealization.OrientGapToken());
            tb.AddRight(new OrientRealization.OrientAndToken());
            tb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }
        public ITokenChain Or()
        {
            tb.AddRight(new OrientRealization.OrientGapToken());
            tb.AddRight(new OrientRealization.OrientOrToken());
            tb.AddRight(new OrientRealization.OrientGapToken());
            return this;
        }

        public ITokenChain Equal()
        {
            tb.AddRight(new OrientRealization.OrientEqualsToken());            
            tb.AddRight(new OrientRealization.OrientEqualsToken()); 
            return this;
        }
        public ITokenChain NotEqual()
        {
            tb.AddRight(new OrientRealization.OrientExclamationToken());            
            tb.AddRight(new OrientRealization.OrientEqualsToken()); 
            return this;
        }       
        public ITokenChain GreaterThan()
        {
            tb.AddRight(tb.NewToken(">"));
            return this;
        }
        public ITokenChain LessThan()
        {
            tb.AddRight(tb.NewToken("<"));
            return this;
        }       
        public ITokenChain GreaterOrEqualThan()
        {
            GreaterThan();
            EqualSign();
            return this;
        }
        public ITokenChain LessOrEqualThan()
        {
            LessThan();
            EqualSign();
            return this;
        }
      
        public ITokenChain @in()
        {
            tb.AddRight(new OrientRealization.OrientInToken());            
            return this;
        }
        public ITokenChain @out()
        {
            tb.AddRight(new OrientRealization.OrientOutToken());            
            return this;
        }

        public ITokenChain E()
        {
            tb.AddRight(new OrientRealization.OrientEToken());            
            return this;
        }
        public ITokenChain V()
        {
            tb.AddRight(new OrientRealization.OrientVertexToken());            
            return this;
        }
       
        
        public ITokenChain Exclamation()
        {
            tb.AddRight(new OrientRealization.OrientExclamationToken());            
            return this;
        }
        public ITokenChain EqualSign()
        {
            tb.AddRight(new OrientRealization.OrientEqualsToken());            
            return this;
        }

        public ITokenChain RdBrLt()
        {
            tb.AddRight(new OrientRealization.OrientRoundBraketLeftToken());            
            return this;
        }
        public ITokenChain RdBrRt()
        {
            tb.AddRight(new OrientRealization.OrientRoundBraketRightToken());            
            return this;
        }

        public ITokenChain SqBrLt()
        {
            tb.AddRight(new OrientRealization.OrientSquareBraketLeftToken());            
            return this;
        }
        public ITokenChain SqBrRt()
        {
            tb.AddRight(new OrientRealization.OrientSquareBraketRightToken());            
            return this;
        }
      
        public ITokenChain Dot()
        {
            tb.AddRight(new OrientRealization.OrientDotToken());            
            return this;
        }
        public ITokenChain Gap()
        {
            tb.AddRight(new OrientRealization.OrientGapToken());            
            return this;
        }

        public ITokenChain Comma()
        {
            tb.AddRight(new OrientRealization.OrientCommaToken());            
            return this;
        }
        public ITokenChain Quote()
        {
            tb.AddRight(new OrientRealization.OrientApostropheToken());            
            return this;
        }
      
    }

    internal class TokenCommandChain : TokenChain
    {
        public TokenCommandChain() {

    }

    ///Main idea description and overall expression logic examples.
    //https://blogs.msdn.microsoft.com/mattwar/2007/07/31/linq-building-an-iqueryable-provider-part-ii/
    /// <summary>
    /// Custom expression visitor Interface
    /// </summary>
    internal interface IExpressionVisitor
    {
        string Translate();
        void Visit<T>(Expression<Func<T, bool>> expr_);
        void Visit<T>(Expression<Func<T, T>> expr_);
        void VisitIn<T>(Expression<Func<T, bool>> expr_,Action<Expression> direction_);

        void VisitLeft(Expression expr_);
        void VisitRight(Expression expr_);
        ITokenQueryChain GetBuilder();
    }
    /// <summary>
    /// Custom expression visitor. Converts tree to orient string syntax.
    /// </summary>
    internal class ExpressionVisitorCustom : ExpressionVisitor,IExpressionVisitor
    {
        ITokenQueryChain cb;
        IChainBuilderFactory cbf;

        PropertyReader pr = new PropertyReader();
        Stack<string> nestedProperties;

        public ExpressionVisitorCustom(IChainBuilderFactory cb_)
        {
            this.cbf = cb_;
            this.cb = cbf.GetQueryChainBuilder();
            nestedProperties = new Stack<string>();
        }
        public ITokenQueryChain GetBuilder()
        {
            if(this.cb!=null)
            {
                return this.cb;
            }
            throw new Exception("No command builder");
        }
        public void StackToStringBuilder(Func<ITokenChain> delimeter_=null)
        {        
            while(nestedProperties.Count()!=0){
            
                cb.AddRight(nestedProperties.Pop());
         
                if(delimeter_!=null){
                if(nestedProperties.Count()!=0){
                    delimeter_.Invoke();
                }}
            }
        }
        public string Translate()
        {
            return cb.GetCommand();
        }

        public void Visit<T>(Expression<Func<T, T>> expr_)
        {
            Type expType = expr_.GetType();
            Type lmdTp = expr_.ReturnType;
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Expression db = expr_.Body;
            this.cb = cbf.GetQueryChainBuilder();
            Visit(db);
        }
        public void Visit<T>(Expression<Func<T,bool>> expr_)
        {
            Type expType = expr_.GetType();
            Type lmdTp = expr_.ReturnType;
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Expression db = expr_.Body;
            this.cb = cbf.GetQueryChainBuilder();
            Visit(db);
        }    
        public new void Visit(Expression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
        
            if(ndType==ExpressionType.AndAlso){ Visit(((BinaryExpression)expr_).Left); cb.And(); Visit(((BinaryExpression)expr_).Right); }
            if(ndType==ExpressionType.OrElse){ Visit(((BinaryExpression)expr_).Left); cb.Or(); Visit(((BinaryExpression)expr_).Right); }

            if(ndType==ExpressionType.Equal){ Visit(((BinaryExpression)expr_).Left); cb.EqualSign(); Visit(((BinaryExpression)expr_).Right); }
            if(ndType==ExpressionType.NotEqual){  Visit(((BinaryExpression)expr_).Left); cb.NotEqual(); Visit(((BinaryExpression)expr_).Right);}
            if(ndType==ExpressionType.GreaterThan){ Visit(((BinaryExpression)expr_).Left); cb.GreaterThan(); Visit(((BinaryExpression)expr_).Right);}
            if(ndType==ExpressionType.LessThan){Visit(((BinaryExpression)expr_).Left); cb.LessThan(); Visit(((BinaryExpression)expr_).Right);}
            if(ndType==ExpressionType.GreaterThanOrEqual){ Visit(((BinaryExpression)expr_).Left); cb.GreaterOrEqualThan(); Visit(((BinaryExpression)expr_).Right);}
            if(ndType==ExpressionType.LessThanOrEqual){ Visit(((BinaryExpression)expr_).Left); cb.LessOrEqualThan(); Visit(((BinaryExpression)expr_).Right);}

            if(ndType==ExpressionType.Constant){ VisitConstant((ConstantExpression)expr_); StackToStringBuilder();}
            if(ndType==ExpressionType.MemberAccess){ VisitMember((MemberExpression)expr_); StackToStringBuilder(cb.Dot);}
            if(ndType==ExpressionType.MemberInit){ VisitMemberInit((MemberInitExpression)expr_); StackToStringBuilder(cb.Comma);}

            if(ndType==ExpressionType.Convert){ VisitUnary((UnaryExpression)expr_); StackToStringBuilder();}

            //StackToStringBuilder();
        }                           

        public void VisitIn<T>(Expression<Func<T,bool>> expr_,Action<Expression> direction_)
        {
            Type expType = expr_.GetType();
            Type lmdTp = expr_.ReturnType;
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Expression db = expr_.Body;
            this.cb = cbf.GetQueryChainBuilder();
            VisitIn(db,direction_);
        }
        public void VisitIn(Expression expr_,Action<Expression> direction_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;

            if(ndType==ExpressionType.Equal){direction_(((BinaryExpression)expr_));}
            
            if(ndType==ExpressionType.MemberAccess){VisitMember((MemberExpression)expr_); StackToStringBuilder();}
            if(ndType==ExpressionType.Constant){ VisitConstant((ConstantExpression)expr_); StackToStringBuilder();}
        }
        public void VisitLeft(Expression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            
            if(ndType==ExpressionType.Equal){ VisitIn(((BinaryExpression)expr_).Left,VisitLeft); }
        }
        public void VisitRight(Expression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
        
            if(ndType==ExpressionType.Equal){ VisitIn(((BinaryExpression)expr_).Right,VisitRight); }
        }

        /*
        public void VisitEqual(BinaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Visit(expr_.Left);
            cb.EqualSign();
            Visit(expr_.Right);
        }
        public void VisitNotEqual(BinaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Visit(expr_.Left);
            cb.NotEqual();
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
        public void VisitLessThan(BinaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;
            Visit(expr_.Left);
            cb.GreaterThan();
            Visit(expr_.Right);
        }
        */

        public new void VisitConstant(ConstantExpression expr_)
        {
            string res = expr_.ToString();
            if(res=="\"\""){
                cb.AddRight(res);
            }else{
                cb.AddRight(expr_.Value.ToString());
            }
       
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
        public new void VisitMemberInit(MemberInitExpression expr_)
        {
            //cb.append("memberExpr Type,Member=");
            //cb.append(expr_.Type.Name);
            IReadOnlyCollection<MemberBinding> memeberBinding=expr_.Bindings;
            
            foreach(MemberBinding mb in memeberBinding)
            {
                MemberInfo mi = mb.Member;
                if(JsonPropertyAttribute.IsDefined(mi,typeof(JsonPropertyAttribute)))
                {
                    Attribute atr=pr.IsUsingAttribute<JsonPropertyAttribute>(mi);
                    Attribute atr2=JsonPropertyAttribute.GetCustomAttribute(mi, typeof(JsonPropertyAttribute));
                    string res=pr.GetJsonPropertyName(atr);
                    nestedProperties.Push(res);
                }else{
                    
                    string mn=mi.Name;
                    nestedProperties.Push(mn);
                }
            }

            /*
            if(JsonPropertyAttribute.IsDefined(mi,typeof(JsonPropertyAttribute)))
            {
                Attribute atr=pr.IsUsingAttribute<JsonPropertyAttribute>(mi);
                Attribute atr2=JsonPropertyAttribute.GetCustomAttribute(mi, typeof(JsonPropertyAttribute));
                string res=pr.GetJsonPropertyName(atr);
                nestedProperties.Push(res);
            }else{
                string mn=expr_.Member.ReflectedType.Name;
                nestedProperties.Push(expr_.Member.Name);
            }
            */
            
        }
        public new void VisitUnary(UnaryExpression expr_)
        {
            Type expType = expr_.GetType();
            Type type_=expr_.GetType().BaseType;
            ExpressionType ndType = expr_.NodeType;

            if(ndType==ExpressionType.Convert )
            {
                if(expr_.Operand.Type == typeof(DateTime)){
                    Expression expr=Expression.Convert(expr_.Operand, expr_.Operand.Type );
                    DateTime exprCpl=Expression.Lambda<Func<DateTime>>(expr).Compile()();
                
                    nestedProperties.Push("'");
                    nestedProperties.Push(exprCpl.ToString(ConfigurationManager.AppSettings["OrientDateTime"]));
                    nestedProperties.Push("'");
                }
                if(expr_.Operand.Type == typeof(int)){
                    int? exprCpl=Expression.Lambda<Func<int?>>(expr_).Compile()();
                    nestedProperties.Push(exprCpl.ToString());
                }
                if(expr_.Operand.Type == typeof(double)){
                    double? exprCpl=Expression.Lambda<Func<double?>>(expr_).Compile()();
                    nestedProperties.Push(exprCpl.ToString());
                }
            }
        }
    
    }
    
    /// <summary>
    /// Cheks property. Used for attribute existing check and parameters reading.
    /// </summary>
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

    //with help of expression trees replaces query shemas + command builder. Makes them AllInOne
    
    /// <summary>
    /// Generates command strings, from types or classes, with custom LINQ Entity look like query style
    /// intended order - Select from where command order
    /// instead of from select where 
    /// </summary>
    internal class CommandBuilder
    {
        IExpressionVisitor expressionVisitor;
        ITokenQueryChain cb;

        public string Translate()
        {
            return cb.GetCommand();
        }
        public void Refresh(){
            cb.Refresh();
        }

        public CommandBuilder(IExpressionVisitor ie_,ITokenQueryChain cb_)
        {            
            expressionVisitor=ie_;
            cb=cb_;
        }       

        ITokenChain ExpressionRead<T>(Expression<Func<T,bool>> expr_){
            expressionVisitor.Visit<T>(expr_);
            return expressionVisitor.GetBuilder();
        }
        ITokenChain ExpressionRead<T>(Expression<Func<T,T>> expr_){
            expressionVisitor.Visit<T>(expr_);
            return expressionVisitor.GetBuilder();
        }

        public CommandBuilder Field<T>(Expression<Func<T,T>> expr_)
        {
            cb.AddRight(ExpressionRead<T>(expr_));
            return this;
        }

        public CommandBuilder Select()
        {
            cb.Select();
            cb.Gap();
            return this;
        }
        //SELECT [{PROPERTY},..,{PROPERTY}]
        public CommandBuilder Select<T>(Expression<Func<T,T>> expr_)
        {
            expressionVisitor.Visit(expr_);      
            expressionVisitor.GetBuilder().Select();
            cb.AddLeft(expressionVisitor.GetBuilder());
            
            return this;
        }
        //FROM {TYPE}
        public CommandBuilder From<T>()
        {            
            cb.AddLeft(typeof(T).Name);
            cb.From();
           
            
            return this;
        }
        public CommandBuilder From(Type T)
        {
            cb.From();
            cb.AddRight(T.Name);
            
            return this;
        }
        
        //add {LEFT NODE}{COMMAND BUILDER TOKENS}{RIGHT NODE}
        //exmpl select from n -> (select from n)
        public CommandBuilder FromNest()
        {
            
            return this;
        }

        //WHERE {PROPERTY}{CONDITION}{VALUE}
        public CommandBuilder Where<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Where();
            if (expr_ != null)
            {
                cb.AddRight(ExpressionRead<T>(expr_));
            }           
            return this;
        }
        
        public CommandBuilder And()
        {
            cb.Gap();
            cb.And();
            return this;
        }
        //and {PROPERTY}{CONDITION}{VALUE}
        public CommandBuilder And<T>(Expression<Func<T,bool>> expr_)
        {          
            cb.Gap();
            cb.And();

            cb.AddRight(ExpressionRead<T>(expr_));
            return this;
        }      
        
        //add
        //select  from Person where  out('Authorship').@rid = 22:0
        //select  from Person where 22:0 in out('Authorship').@rid
        //int=0
        public CommandBuilder in_<T>(Expression<Func<T,bool>> expr_)
        {
            expressionVisitor.VisitIn<T>(expr_,expressionVisitor.VisitRight);
            cb.AddRight(expressionVisitor.GetBuilder());

            cb.Gap();
            cb.@in();
            cb.Gap();
            

            expressionVisitor.GetBuilder().Refresh();
            expressionVisitor.VisitIn<T>(expr_,expressionVisitor.VisitLeft);
            cb.AddRight(expressionVisitor.GetBuilder());

            //cb.@in();
            //cb.AddRight(typeof(T).Name);
            return this;
        }
        
        //[in,out]('{TYPE}')
        public CommandBuilder @in(Type type)
        {
            cb.@in();
            cb.RdBrLt();
            cb.Quote();
            cb.AddRight(type.Name);
            cb.Quote();
            cb.RdBrRt();
            return this;
        }
        public CommandBuilder @out(Type type)
        {
            cb.@out();
            cb.RdBrLt();
            cb.Quote();
            cb.AddRight(type.Name);
            cb.Quote();
            cb.RdBrRt();
            return this;
        }        
        //[in,out]('{Type}').[0].@{PROPERTY}
        public CommandBuilder @in<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Gap();
            cb.@in();         
            cb.RdBrLt();
            cb.Quote();
            cb.AddRight(typeof(T).Name);
            cb.Quote();
            cb.RdBrRt();

            cb.SqBrLt();
            cb.AddRight("0");
            cb.SqBrRt();

            cb.Dot();
            
            cb.AddRight(ExpressionRead<T>(expr_));
            return this;
        }
        public CommandBuilder @out<T>(Expression<Func<T,bool>> expr_)
        {
            cb.Gap();
            cb.@out();           
            cb.RdBrLt();
            cb.Quote();
            cb.AddRight(typeof(T).Name);
            cb.Quote();
            cb.RdBrRt();

            cb.SqBrLt();
            cb.AddRight("0");
            cb.SqBrRt();

            cb.Dot();
            
            cb.AddRight(ExpressionRead<T>(expr_));
            return this;
        }       


        public CommandBuilder Dot()
        {
            cb.Dot();
            return this;
        }
        public CommandBuilder Gap()
        {
            cb.Gap();
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
            CommandBuilder evc=new CommandBuilder(new ExpressionVisitorCustom(new ChainBuilderFactory()), new TokenQueryChain());
            CommandBuilder commandBuilder;

            evc.@in<POCO.Authorship>(s=>s.id=="'25:26'");
            string res=evc.Translate();

            POCO.Unit unit = new POCO.Unit() {Name="НСПК"};

            evc.Refresh();
            evc
            .From(typeof(POCO.News))
            .Where<POCO.News>(s => s.pinned.isTrue == false
                    && s.name == "'Author eats his own book'"
                    || s.Likes != 0 && s.created <= DateTime.Now
                    && s.commentDepth >= 1)
            .Select<POCO.News>(s => new POCO.News { id = s.id, authAcc = s.authAcc, GUID = s.GUID })
          ;
            string res2=evc.Translate();

            evc.Refresh();
            evc.From(typeof(POCO.Comment))
                .Where<POCO.Comment>(s => s.changed == new DateTime(2018, 01, 26, 12, 52, 46))
                .And<POCO.Comment>(s => s.class_ != "")            
            .Select<POCO.Comment>(s => new POCO.Comment { id = s.id, class_ = s.class_ });
            string res4=evc.Translate();
            
            evc.Refresh();
            evc                
                .Gap().@in(typeof(POCO.CommonSettings)).Dot().@out(typeof(POCO.Commentary))
                .Dot().Field<POCO.Comment>(s => new POCO.Comment{GUID=s.GUID})
              .From(typeof(POCO.News))
            .Where<POCO.News>(null)
            .@in<POCO.Comment>(s => s.GUID == "123")
            .And().in_<POCO.Authorship>(s => s.id == "123")
            .Select();
            string res5 = evc.Translate();

            evc.Refresh();
            evc.in_<POCO.Commentary>(s => s.GUID == "abc");

            commandBuilder=new CommandBuilder(new ExpressionVisitorCustom(new ChainBuilderFactory()), new TokenQueryChain());
            commandBuilder.Select<POCO.Person>(s => new POCO.Person {id=s.id,Birthday=s.Birthday,FirstName=s.FirstName,LastName=s.LastName});
            string res3 = commandBuilder.Translate();
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
