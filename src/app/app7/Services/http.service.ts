import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import { Injectable } from '@angular/core';

import {Test,HtmlItem,ModelContainer,NodeCollection,Quiz} from 'app/app7/Models/inits.component'

import {Observable} from 'rxjs/Observable';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable()
export class HttpService {

  nodesPassed_:NodeCollection;

  private url:string = 'http://localhost:3208//api//quiz';

  constructor(private http: HttpClient) {
    ModelContainer.Init();
    this.nodesPassed_=ModelContainer.nodesPassed_;
  }

  addQuiz (quiz: NodeCollection): Observable<NodeCollection[]> {
    return this.http.post<NodeCollection[]>(this.url, quiz, httpOptions);
  }

  getQuiz(){
    return this.http.get<NodeCollection[]>(this.url);
  }

  addQuizTs () {

    this.addQuiz(this.nodesPassed_)
    .subscribe(
      (s:NodeCollection[]) =>{
        console.log(["Post: ",s])
      }
    );

  }

  getQuizTs(){
    this.getQuiz().subscribe(
      (s:NodeCollection[]) =>{
        console.log(["Get: ",s])
      }
    );
  }

}
