import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import { Injectable } from '@angular/core';

import {HtmlItemNew} from 'src/app/applist/Models/POCOnew.component'

import {Observable} from 'rxjs/Observable';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable()
export class HttpService {

  nodesPassed_:HtmlItemNew;

  private url:string = 'http://localhost:3208//api//quiz';

  constructor(private http: HttpClient){
    this.nodesPassed_=new HtmlItemNew(null);
  }

  addQuiz (quiz: HtmlItemNew): Observable<HtmlItemNew[]>{
    return this.http.post<HtmlItemNew[]>(this.url, quiz, httpOptions);
  }

  getQuiz(){
    return this.http.get<HtmlItemNew[]>(this.url);
  }

  addQuizTs(){

    this.addQuiz(this.nodesPassed_)
    .subscribe(
      (s:HtmlItemNew[]) =>{
        console.log(["Post: ",s])
      }
    );

  }

  getQuizTs(){
    this.getQuiz().subscribe(
      (s:HtmlItemNew[]) =>{
        console.log(["Get: ",s])
      }
    );
  }

}
