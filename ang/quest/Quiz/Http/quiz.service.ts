import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import { Injectable } from '@angular/core';

import {Quiz} from '../Model/QtMd.component';
import {Observable} from 'rxjs/Observable';

import 'rxjs/add/operator/map';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    'Authorization': 'my-auth-token'
  })
};

@Injectable()
export class HS{

  private url:string = 'http://msk1-vm-ovisp01:8185/api/quiz2';

  constructor(
    private http: HttpClient
  ) { }

  addQuiz (quiz: Quiz): Observable<Quiz[]> {
      return this.http.post<Quiz[]>(this.url, Quiz, httpOptions);
  }

  getQuiz(): Observable<Quiz[]> {
      //return this.http.get<Quiz[]>(this.url);
      return this.http.get<Quiz[]>(this.url);
  }

  getQuizes() {
      return this.http.get<Quiz>(this.url,{observe: 'response'});
  }

  getQuizResponse() {
      return this.http.get(this.url,{observe: 'response'})
      .subscribe(r=>{
        //const ks=r.headers.keys();
        //console.log(r.body);
      });
  }

}
