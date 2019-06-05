using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

namespace NSQLManager
{

  class LinqContextPOC
  {
    
  }

  public class TestContext
  {
    public static string nodeResult;
    Stack<NodeStack> nodeStack;
    
    public TestContext()
    {
      this.nodeStack = new Stack<NodeStack>();
      nodeResult = string.Empty;
    }

    public string ConditionFromExpressionTypes<T>(Expression<Func<T,bool>> expr_)
      where T:TestEntity
    {
      var b = expr_;
      var c = b.Body;
      var gtp = c.GetType();
      var nt = c.NodeType;
      var pm = expr_.Parameters;
      var nm = expr_.Name;
      var tp = expr_.Type;

      var typeName = typeof(T);

    
      BinaryExpression binaryE = (BinaryExpression)expr_.Body;
      MemberExpression leftMember = (MemberExpression)binaryE.Left;
      ConstantExpression rightConstantE = (ConstantExpression)binaryE.Right;
      
      Type rightType=rightConstantE.Type;
      string propName = leftMember.Member.Name;     
      Type leftType=leftMember.Type;

      if(leftMember.Expression!=null){
        Visit(leftMember);}

      ParameterExpression leftPe=Expression.Parameter(typeof(TestEntity),typeName.Name);
      ParameterExpression rightPe=Expression.Parameter(rightType,rightType.Name);
      
      Expression leftE=Expression.Property(leftPe, propName);
      Expression rightEtp=Expression.Constant(rightConstantE.Value, rightType);

      Expression e2=Expression.Assign(leftE,rightEtp);

      BinaryExpression newB = Expression.MakeBinary(nt,leftE,rightEtp);

      ParameterExpression peL = Expression.Parameter(typeof(bool),"ToggleProp.isTrue");
      ParameterExpression peR = Expression.Parameter(typeof(bool),"true");
      BinaryExpression beT = Expression.MakeBinary(ExpressionType.Equal,peL,peR);

      string ets2 = e2.ToString();
      string bE = newB.ToString();
      return ets2;
    }
   
    public Expression Visit(Expression e)
    {      
      
      return e;
    }
  }
  public class NodeStack
  {  
    public string NodeName {get;set;}
    public Type nodeType {get;set;}
  }

  public class TestEntity
  {
    public string name {get;set;}
    static int id { get; set; } = 0;
    public ToggleProp tp { get; set; }
    public int Id
    {
      get{ 
        if(id==0){ id += 1; }
        return id; 
      }
      set{ id = value; }
    }
    public bool intrinsicIsTrue  { get; set; }
  }
  public class ToggleProp
  {
    public bool isTrue { get; set; }
  }

}
