using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace NSQLManager
{
  
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
    
    public string TraverseExpression<T>(Expression<Func<T,bool>> expr_)
    {
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
