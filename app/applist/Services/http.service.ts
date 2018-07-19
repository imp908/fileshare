import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import {Injectable } from '@angular/core';

import {QuizItemNew} from 'src/app/applist/Models/POCOnew.component'
import {FactoryNew} from 'src/app/applist/Models/initsNew.component'

import {Observable} from 'rxjs/Observable';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({providedIn: 'root' })
export class HttpService{

  nodesPassed_:QuizItemNew;

  private url:string = 'http://localhost:63282/api/quiz2/';

  constructor(private http: HttpClient){
    // this.nodesPassed_=new QuizItemNew(null);
  }

  addQuiz(url_:string,quiz: QuizItemNew): Observable<QuizItemNew>{
    if(url_==null || url_=="" || url_==" "){url_=this.url};
    let body = JSON.stringify(quiz);
    console.log(body);
    return this.http.post<QuizItemNew>(url_, body, httpOptions);
  }

  getQuiz(url_:string): Observable<QuizItemNew>{
    if(url_==null || url_=="" || url_==" "){url_=this.url};
    console.log("Get quiz" + url_)
    return this.http.get<QuizItemNew>(url_);
  }
  getQuiz2(url_:string): Observable<HttpResponse<QuizItemNew>>{
    if(url_==null){url_=this.url};
    return this.http.get<QuizItemNew>(url_, { observe: 'response' });
  }

  getQuizResp(url_:string){
    this.getQuiz2(url_).subscribe(s=>{
        // console.log(s);
      }
    )
  }
  getQuizObj(url_:string):QuizItemNew {
    let r0=new QuizItemNew(null);
    let r1=new QuizItemNew(null);
    let r2=new QuizItemNew(null);

    this.getQuiz(url_).subscribe((s:QuizItemNew)=>{

        FactoryNew.cloneFromProt(r0,s);
        FactoryNew.cloneFromObj(r1,s);
        r2=FactoryNew.cloneByKey(s);
        console.log(r2,s);

      }
    )

    return r2;
  }


  addQuizTs(str_:string){

    this.addQuiz(str_,this.nodesPassed_)
    .subscribe(
      (s:QuizItemNew) =>{
        console.log(["addQuizTs: ",s])
      }
    );

  }

}
