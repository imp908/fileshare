import {IPerson,QuestionExport} from "./ipers_";

//function returning Iperson objects
function greeter(person: IPerson) {

  return "Hello, " + person.firstName+ " " + person.lastName;
}

//class implements Iperson implicitly
class Student{
  fullName: string;

  constructor(public firstName:string,public middleName:string,public lastName: string){
    this.fullName=firstName+" "+middleName+" "+lastName;
  }
}

//mooved to IPers_.ts
/*
interface IPerson{
  firstName: string;
  lastName: string;
}
*/

//new Person interface
let user = {firstName:"User", lastName:"Name"};

//ref user to new Student class Student:Person
user = new Student("FName","MName","LName")

//build time type check error
//let user=[0,1,2];

document.body.innerHTML = greeter(user);





interface Itoc
{
  id:number;
  name:string;
  value:string;
}

//check object constructor class
class toc
{
  id:number;
  name:string;
  value:string;

  constructor(
    public id_?:number,public name_?:string ,public value_?:string
  )
  {
    this.id=id_;this.name=name_;this.value=value_;
  }
}

//check new class initializer
class tocinit
{
  id:number;
  name:string;
  value:string;

  constructor(){}

}

//prints class instance
function drawElement(tc: toc){
  var elem=document.createElement("div")
  var op={id:0,name:"a",value:"b"}
  //OR
  //let tc=new toc(op );
  console.log( tc.id + " " + tc.name);
  return tc.id + " " + tc.name;
}

//prints interface instance
function drawElementInt(tc: Itoc){
  var elem=document.createElement("div")
  var op={id:0,name:"a",value:"b"}
  //OR
  //let tc=new toc(op );
  console.log( tc.id + " " + tc.name);
  return tc.id + " " + tc.name;
}

let tocinitItm = new toc (0,"a","b");

//document.body.innerHTML=drawElement(tocinit);

window.onload = () => {
  let tc=new toc(1,"c");
  console.log(tc)
  console.log(drawElement(tc))
  console.log(drawElementInt(tc));

  let questMd=new QuestionExport();
  let questMdArr=questMd.getQuestions();
  console.log(questMdArr);
};
