using System;
using System.Collections.Generic;
using System.Linq;


public interface IQuizItem
{
    int _key { get; set; }
    string _name { get; set; }
    string _value { get; set; }
    string _typeName { get; set; }

    string cssClass { get; set; }
    string HtmlTypeAttr { get; set; }
    string HtmlSubmittedValue { get; set; }
    bool Show { get; set; }

    IEnumerable<IQuizItem> array { get; set; }
}
public class HtmlItem : IQuizItem
{   
    public int _key { get; set; }
    public string _name { get; set; }
    public string _value { get; set; }
    public string _typeName { get; set; }

    public string cssClass { get; set; }
    public string HtmlTypeAttr { get; set; }
    public string HtmlSubmittedValue { get; set; }
    public bool Show { get; set; }

    public IEnumerable<IQuizItem> array { get; set; } = new List<HtmlItem>();

    public HtmlItem()
    {
        this._typeName = this.GetType().ToString();
    }

}

public class QuizItem : HtmlItem, IQuizItem
{
    public List<QuizItem> itemControlls { get;set;} 
}
public class Question : QuizItem
{
    
}
public class Answer : QuizItem
{
    
}
public class QuizNew : QuizItem
{

}

public class QuizNewGet : QuizItem
{
    public DateTime dateFrom { get; set; }
    public DateTime dateTo { get; set; }
    public List<Question> questions_ { get; set; }
}

public class QuizUOW
{
  public static QuizItem QuizGenerate()
        {
            QuizItem r = new QuizItem()
            {
                array = new List<QuizNew>()
                {
                    new QuizNew()
                    {
                        _key = 0,
                        _name = "QuizName",
                        _value = "QuizValue",
                        Show = true
                        ,
                        array = new List<Question>() {
                        new Question() { _key = 0, _name = "name0", _value = "value0", Show = true
                            ,array = new List<Answer>() {
                                new Answer(){_key = 0, _name = "name0", _value = "value0", Show = true}
                                }
                            }
                        }
                    }
                }
            };

            return r;
        }   
}