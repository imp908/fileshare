"use strict";
exports.__esModule = true;
//var ipers_1 = require("./ipers_");
//function returning Iperson objects
function greeter(person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}
//class implements Iperson implicitly
var Student = /** @class */ (function () {
    function Student(firstName, middleName, lastName) {
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.fullName = firstName + " " + middleName + " " + lastName;
    }
    return Student;
}());
//mooved to IPers_.ts
/*
interface IPerson{
  firstName: string;
  lastName: string;
}
*/
//new Person interface
var user = { firstName: "User", lastName: "Name" };
//ref user to new Student class Student:Person
user = new Student("FName", "MName", "LName");
//build time type check error
//let user=[0,1,2];
document.body.innerHTML = greeter(user);
//check object constructor class
var toc = /** @class */ (function () {
    function toc(id_, name_, value_) {
        this.id_ = id_;
        this.name_ = name_;
        this.value_ = value_;
        this.id = id_;
        this.name = name_;
        this.value = value_;
    }
    return toc;
}());
//check new class initializer
var tocinit = /** @class */ (function () {
    function tocinit() {
    }
    return tocinit;
}());
//prints class instance
function drawElement(tc) {
    var elem = document.createElement("div");
    var op = { id: 0, name: "a", value: "b" };
    //OR
    //let tc=new toc(op );
    console.log(tc.id + " " + tc.name);
    return tc.id + " " + tc.name;
}
//prints interface instance
function drawElementInt(tc) {
    var elem = document.createElement("div");
    var op = { id: 0, name: "a", value: "b" };
    //OR
    //let tc=new toc(op );
    console.log(tc.id + " " + tc.name);
    return tc.id + " " + tc.name;
}
var tocinitItm = new toc(0, "a", "b");
//document.body.innerHTML=drawElement(tocinit);
window.onload = function () {
    var tc = new toc(1, "c");
    console.log(tc);
    console.log(drawElement(tc));
    console.log(drawElementInt(tc));
    var questMd = new QuestionExport();
    var questMdArr = questMd.getQuestions();
    console.log(questMdArr);
};
