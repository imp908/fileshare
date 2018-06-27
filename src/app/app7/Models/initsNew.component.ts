import {ServiceCl} from '../Services/services.component';
import { EventEmitter,Output } from '@angular/core';

//TS Collections
//npm install typescript-collections [-g] --save

import * as Collections from 'typescript-collections';

import {INode,ICollection_,INodeCollection} from './POCO.component';

import {NodeNew,CollectionNew
  ,HtmlItemNew
  ,QuizItemNew
  ,AnswerNew,QuestionNew,QuizNew
  ,TextControlNew,CheckBoxControlNew
  ,DropDownControlMultiNew,DropDownControlNgNew,RadioButtonControlNew
} from './POCOnew.component';


export class FactoryNew{

  //intrinsic collections test

  static TextControlNewTest(n:number){
      var r = new HtmlItemNew(null);

        for(var i=0;i<n;i++){
          r.add(new TextControlNew({
            key_:i,
            name_:"Textctrl"+i,
            value_:"Textctrl"+i,
            typeName_:null
            ,array_:null
            ,cssClass_:"",show_:true
            ,HtmlTypeAttr_:"div"
            ,HtmlSubmittedValue_:"text value"+i
            ,pattern_:null
            ,maxLength_:null
            ,minLength_:null}));
        }

      return r;
  }
  static CheckBoxControlNewTest(n:number){
      var r= new CheckBoxControlNew(null);

      for(var i=0;i<n;i++){
        r.add(
          new CheckBoxControlNew({
              key_:0,
              name_:"Textctrl_"+i,
              value_:"Textctrl_"+i,
              typeName_:null
              ,array_:FactoryNew.TextControlNewTest(3).array
              ,cssClass_:"",show_:true
              ,HtmlTypeAttr_:"div"
              ,HtmlSubmittedValue_:null
            }));
      }

      return r;
  }
  static RadioButtonControlNewTest(n:number){
      var r= new RadioButtonControlNew(null);
      for(var i=0;i<n;i++){
        r.add( new TextControlNew({
              key_:0,
              name_:"Textctrl_"+i,
              value_:"Textctrl_"+i,
              typeName_:null
              ,array_:FactoryNew.TextControlNewTest(3).array
              ,cssClass_:"",show_:true
              ,HtmlTypeAttr_:"div"
              ,HtmlSubmittedValue_:"text value "+0
              ,pattern_:null
              ,maxLength_:null
              ,minLength_:null}));
      }

      return r;
  }
  static DropDownControlMultiNewTest(n:number){
      var r= new HtmlItemNew(null);
      for(var i=0;i<n;i++){
        r.add( new DropDownControlMultiNew({
              key_:0,
              name_:"Textctrl_"+i,
              value_:"Textctrl_"+i,
              typeName_:null
              ,array_:null
              ,cssClass_:"",show_:true
              ,HtmlTypeAttr_:"div"
              ,HtmlSubmittedValue_:new Date(2018,1,1)
              }));
      }

      return r;
  }

  static quizItemParametersNewGen(){

    let r = new HtmlItemNew({
      key_:0,
      name_:"Quiz controlls",
      value_:"Quiz controlls",
      typeName_:null
      ,array_:null
      ,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"div"
      ,HtmlSubmittedValue_:"Quiz controlls"
    });

    r.add(new CheckBoxControlNew({
      key_:1,
      name_:"IsAnonimous",
      value_:"is question anonimous?",
      typeName_:null
      ,array_:null
      ,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"div"
      ,HtmlSubmittedValue_:false
    }));

    r.add(new TextControlNew({
      key_:2,
      name_:"Textctrl",
      value_:"Textctrl ",
      typeName_:null
      ,array_:null
      ,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"div"
      ,HtmlSubmittedValue_:"text value "
      ,pattern_:null
      ,maxLength_:null
      ,minLength_:null
    }));

    return r;
  }

  //quiz objects generating

  static answers(n:number){
    var r= new Array<AnswerNew>();

    for(var i=0;i<n;i++){
      r.push(new AnswerNew(
        {key_:i,
        name_:"Answer"+i,
        value_:"Answer"+i,
        typeName_:"AnswerNew",array_:null
        ,itemControlls_:null,cssClass_:"",show_:true
        ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""}
      ));
    }

    return r;
  }
  static questions(n:number){
    var r=new Array<QuestionNew>();

    for(var i=0;i<n;i++){
      r.push(new QuestionNew(
        {key_:i,
        name_:"Question"+i,
        value_:"Question"+i,
        typeName_:"QuestionNew",array_:FactoryNew.answers(3)
        ,itemControlls_:null,cssClass_:"",show_:true
        ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""}
      ));
    }

    return r;
  }
  static quizes(n:number){
    var r=new Array<QuizNew>();

    for(var i=0;i<n;i++){

      r.push(new QuizNew(
        {key_:i,
        name_:"Quiz"+i,
        value_:"Quiz"+i,
        typeName_:"QuizNew",array_:null
        ,itemControlls_:FactoryNew.quizItemParametersNewGen().array
        ,cssClass_:"",show_:true
        ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""}
      ));
    }
    return r;
  }

  static GenQuizes(qn:number,qtn:number,an:number){

    let nodes=new QuizItemNew({key_:0,
    name_:"Quizes",
    value_:"Quizes",
    typeName_:null,array_:new Array<QuizNew>()
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    for(let i=0;i<qn;i++){
      let qzNew=new QuizNew({key_:i,
      name_:"Quiz_"+i,
      value_:"Quiz_"+i,
      typeName_:null,array_:new Array<QuestionNew>()
      ,itemControlls_:null,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

        for(let i2=0;i2<qtn;i2++){
          let qtNew=new QuestionNew({key_:i+i2,
          name_:"Question_"+(i+i2),
          value_:"Question_"+(i+i2),
          typeName_:"QuestionNew",array_:new Array<AnswerNew>()
          ,itemControlls_:null,cssClass_:"",show_:true
          ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

            for(let i3=0;i3<an;i3++){
              let awNew=new AnswerNew({key_:i+i2+i3,
              name_:"Answer"+(i+i2+i3),
              value_:"Answer"+(i+i2+i3),
              typeName_:"AnswerNew",array_:null
              ,itemControlls_:null,cssClass_:"",show_:true
              ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

              qtNew.array.push(awNew);
            }

          qzNew.array.push(qtNew);
        }

      nodes.array.push(qzNew);
    }

    return nodes;
  }

}

export class TestNew{

  public static QuizItemsCheck(){
    console.log(["TextControlNew ",FactoryNew.TextControlNewTest(3)])
    console.log(["CheckBoxControlNew ",FactoryNew.CheckBoxControlNewTest(3)])
    console.log(["RadioButtonControlNew ",FactoryNew.RadioButtonControlNewTest(3)])
    console.log(["DropDownControlMultiNewTest ",FactoryNew.DropDownControlMultiNewTest(3)])
  }

  public static NodeCollectionNewTypeNamesCheck(){

    let n = new NodeNew({key_:2,name_:"Answer1",value_:"Answer1",typeName_:null});
    let nc = new CollectionNew<NodeNew>({key_:2,name_:"Answer1",value_:"Answer1"
    ,typeName_:null,array_:null});
    let h = new HtmlItemNew({key_:2,name_:"Answer1",value_:"Answer1"
    ,typeName_:null,array_:null,cssClass_:"",show_:true,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let qw = new AnswerNew({key_:0,
    name_:"Answer1",
    value_:"Answer1",
    typeName_:null,array_:null
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    if(n._typeName=="NodeNew"){console.log(["NodeNew _typeName OK",n])}
    else{console.log(["NodeNew _typeName OK",n])}
    if(nc._typeName=="CollectionNew"){console.log(["CollectionNew _typeName OK",nc])}
    else{console.log(["CollectionNew _typeName NOT OK",nc])}
    if(h._typeName=="HtmlItemNew"){console.log(["HtmlItemNew _typeName OK",h])}
    else{console.log(["HtmlItemNew _typeName NOT OK",h])}
    if(qw._typeName=="AnswerNew"){console.log(["HtmlItemNew _typeName OK",qw])}
    else{console.log(["AnswerNew _typeName NOT OK",qw])}


    console.log(["NodeCollectionNewTypeNamesCheck",n,nc])

  }
  public static NodeCollectionNewCheck(){

    let n0:CollectionNew<NodeNew>=new CollectionNew<NodeNew>({key_:2,
    name_:"Answer1",
    value_:"Answer1",
    typeName_:null,array_:null});

    let n1: CollectionNew<NodeNew>=new  CollectionNew<NodeNew>({key_:1,
    name_:"Answer2",
    value_:"Answer2",
    typeName_:null,array_:null});

    let c0:CollectionNew<NodeNew>=new CollectionNew<NodeNew>({key_:1,
    name_:"Question1",
    value_:"Question1",
    typeName_:null,array_:[n0,n1]});


    let r:CollectionNew<NodeNew>=new CollectionNew<NodeNew>({key_:0,
    name_:"Quiz0",
    value_:"Quiz0",
    typeName_:null,array_:[c0]});

    if(r.array.length!=-1){console.log(["CollectionNew array OK",r]);}
    else{console.log(["CollectionNew array NOT OK",r]);}
    let nc=r.array[0];
    if( nc instanceof CollectionNew){
      if(nc.array[0].array.length!=-1){console.log(["CollectionNew nested array OK",r]);}
      else{console.log(["CollectionNew nested array NOT OK",r]);}
    }

    // console.log(JSON.stringify(r));

  }
  public static QuizHierarhyCheck(){

    let parentNode=new HtmlItemNew({key_:0,
    name_:"ParentNode",
    value_:"ParentNode",
    typeName_:"HtmlItemNew",array_:null,
    cssClass_:"",show_:true,
    HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let a0=new AnswerNew({key_:100,
    name_:"Answer1",
    value_:"Answer1",
    typeName_:"AnswerNew",array_:null
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let a1=new AnswerNew({key_:99,
    name_:"Answer2",
    value_:"Answer2",
    typeName_:"AnswerNew",array_:null
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let q0=new QuestionNew({key_:89,
    name_:"Question1",
    value_:"Question1",
    typeName_:"QuestionNew",array_: [a0,a1]
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let q1=new QuestionNew({key_:88,
    name_:"Question2",
    value_:"Question2",
    typeName_:"QuestionNew",array_:[a1]
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let qz0=new QuizNew({key_:87,
    name_:"Quiz1",
    value_:"Quiz1",
    typeName_:null,array_:[q1]
    ,itemControlls_:null,cssClass_:"",show_:true
    ,HtmlTypeAttr_:"",HtmlSubmittedValue_:""});

    let t0=new TextControlNew({
      key_:70,
      name_:"Textctrl1",
      value_:"Textctrl1",
      typeName_:"TextControlNew"
      ,array_:null
      ,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"div"
      ,HtmlSubmittedValue_:"text value"
      ,pattern_:null
      ,maxLength_:null
      ,minLength_:null});
      let cb0=new CheckBoxControlNew({key_:69,
      name_:"Checkbox1",
      value_:"Checkbox1",
      typeName_:"CheckBoxControlNew"
      ,array_:null
      ,cssClass_:"",show_:true
      ,HtmlTypeAttr_:"div"
      ,HtmlSubmittedValue_:true
    });

    let tbdd0=new TextControlNew({key_:60,name_:"Textctrl20",value_:"Textctrl20",
    typeName_:"TextControlNew",array_:null,cssClass_:"",show_:true,HtmlTypeAttr_:"div"
    ,HtmlSubmittedValue_:"drop box tb1",pattern_:null,maxLength_:null,minLength_:null});
    let tbdd1=new TextControlNew({key_:61,name_:"Textctrl21",value_:"Textctrl21",
    typeName_:"TextControlNew",array_:null,cssClass_:"",show_:true,HtmlTypeAttr_:"div"
    ,HtmlSubmittedValue_:"drop box tb2",pattern_:null,maxLength_:null,minLength_:null});
    let tbdd2=new TextControlNew({key_:62,name_:"Textctrl22",value_:"Textctrl22",
    typeName_:"TextControlNew",array_:null,cssClass_:"",show_:true,HtmlTypeAttr_:"div"
    ,HtmlSubmittedValue_:"drop box tb3",pattern_:null,maxLength_:null,minLength_:null});
    let tddArr=[tbdd0,tbdd1,tbdd2]
    let ddC0=new DropDownControlNgNew({key_:68,
    name_:"DropDownControlNgNew1",
    value_:"DropDownControlNgNew1",
    typeName_:"DropDownControlNgNew"
    ,array_:tddArr
    ,cssClass_:"col",show_:true
    ,HtmlTypeAttr_:"column"
    ,HtmlSubmittedValue_:true
    });

    let rb0=new RadioButtonControlNew({key_:59,name_:"Rbctrl1",value_:"Rbctrl1",
    typeName_:"RadioButtonControl",array_:tddArr,cssClass_:"row",show_:true,HtmlTypeAttr_:"div"
    ,HtmlSubmittedValue_:"radio controll 01"});

    qz0.itemControlls=[t0,cb0,ddC0,rb0];

    parentNode.array=[qz0];

    if(parentNode.array.length!=-1){
      if(parentNode.array[0].array.length!=-1){
        if(parentNode.array[0].array[0].array.length!=-1){
          console.log(["Quiz hierarchy OK",parentNode])
        }
      }
    }
  }
  public static FactoryQuizGenCheck(){
      let min=3;
      let max=10;

      let qzRnd=Math.floor(Math.random()*(max-min)+min);
      let qtRnd=Math.floor(Math.random()*(max-min)+min);
      let awRnd=Math.floor(Math.random()*(max-min)+min);

      //random check
      /*
      let arr:number[]=[];
      for(let z=0;z<1000;z++){
        arr.push(Math.floor(Math.random()*(max-min)+min))
      }
      console.log(["Rand arr: ", arr.length,Math.min.apply(min,arr),Math.max.apply(min,arr)])
      */

      let r=FactoryNew.GenQuizes(qzRnd,qtRnd,awRnd)

      if(r!=null){

        let qz=r.array[0]
        let qs=qz.array[0];
        qz.tolog=true;
        let qsf=qz.getByItem(qs);
        let qsn=qz.getByName(qs._name);

        let qzt=qz.getType();

        if(qs===qsf){console.log(["getByItem OK",qsf])}
        if(qs===qsn){console.log(["getByName OK",qsn])}

      }
  }

  public static FactoryCheck(){

    TestNew.QuizItemsCheck();

    TestNew.NodeCollectionNewTypeNamesCheck();
    TestNew.NodeCollectionNewCheck();
    TestNew.QuizHierarhyCheck();
    TestNew.FactoryQuizGenCheck();

  }

  public static GO(){
    //check node collection new
    //Test.NodeCollectionNewCheck();

    //Test.HtmlItemCheck();

    TestNew.FactoryCheck();
  }

}
