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

  private url:string =
  //'http://msk1-vm-ovisp01:8185/api/quiz2'
  'http://localhost:63282/api/quiz2'
  ;
  className: string;

  constructor(
    private http: HttpClient
  ) { this.className = "HS";}

  addQuiz (quiz: Quiz): Observable<Quiz[]> {
      return this.http.post<Quiz[]>(this.url, Quiz, httpOptions);
  }
  getQuiz(): Observable<Quiz[]> {
      //return this.http.get<Quiz[]>(this.url);
      return this.http.get<Quiz[]>(this.url);
  }
  getQuizes(url_:string) {
      return this.http.get<Quiz>(url_,{observe: 'response'});
  }
  getQuizResponse() {
      return this.http.get(this.url,{observe: 'response'})
      .subscribe(r=>{
        //const ks=r.headers.keys();
        //console.log(r.body);
      });
  }
  getQuizUrlResponse(url_:string) {
      console.log("Passed url: ",url_)
      return this.http.get(url_,{observe: 'response'})
      .subscribe(r=>{
        //const ks=r.headers.keys();
        //console.log(r.body);
      });
  }
}
