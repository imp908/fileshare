import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import {Injectable } from '@angular/core';

import {QuizItemNew} from 'src/app/applist/Models/POCOnew.component'

import {Observable} from 'rxjs/Observable';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({providedIn: 'root' })
export class HttpService{

  nodesPassed_:QuizItemNew;

  private url:string = 'http://localhost:63282//api//quiz2/any';

  constructor(private http: HttpClient){
    // this.nodesPassed_=new QuizItemNew(null);
  }

  addQuiz(url_:string,quiz: QuizItemNew): Observable<QuizItemNew[]>{
    if(url_==null){url_=this.url};
    return this.http.post<QuizItemNew[]>(url_, quiz, httpOptions);
  }

  getQuiz(url_:string){
    if(url_==null){url_=this.url};
    return this.http.get<QuizItemNew[]>(url_);
  }

  addQuizTs(str_:string){

    this.addQuiz(str_,this.nodesPassed_)
    .subscribe(
      (s:QuizItemNew[]) =>{
        console.log(["addQuizTs: ",s])
      }
    );

  }

  getQuizTs(str_:string){
    this.getQuiz(str_).subscribe(
      (s:QuizItemNew[]) =>{
        console.log(["getQuizTs: ",s])
      }
    );
  }

}
