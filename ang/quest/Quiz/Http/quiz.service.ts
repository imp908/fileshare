import { HttpClient, HttpHeaders , HttpResponse} from '@angular/common/http';

import { Injectable } from '@angular/core';

import {Quiz,IPrimitiveCollection} from '../Model/QtMd.component';
import {Observable} from 'rxjs/Observable';

import 'rxjs/add/operator/map';

const _httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
    //,    'Authorization': 'my-auth-token'
  })
};

export interface value_cl {
  prop1: any;
  prop2: any;
}

@Injectable()
export class HS {

  private url:string =
  'http://msk1-vm-ovisp01:8185/api/quiz2'
  //'http://localhost:63282/api/quiz2'

  ;
  className: string;

  value_:value_cl;
  quizes_:IPrimitiveCollection<Quiz>;

  constructor(
    private http: HttpClient
  ) {
      this.className = "HS";
      this.quizes_=null;
    }

  //get action
  //return whole response object
  getQuizResponse(url_:string): Observable<HttpResponse<IPrimitiveCollection<Quiz>>> {
      return this.http.get<IPrimitiveCollection<Quiz>>(
      url_, { observe: 'response' });
  }

  //simplier Shorter version? returns array of objects but not IPrimitiveCollection
  getQuizResponse2(url_:string): Observable<Array<Quiz>> {
      return this.http.get<Array<Quiz>>( url_)
  }

  getQuizResponse3(url_:string): Observable<Array<Quiz>> {
      return this.http.get<Array<Quiz>>( url_)
      .map(r=>{
          return r.map(
            c=>{
              return new Quiz(c.key,c.name,c.value,c.dateFrom,c.dateTo,c.questions_)
            }
          );
      })
      ;
  }

  //POST action
  addQuiz (url_:string,quiz: IPrimitiveCollection<Quiz>): Observable<IPrimitiveCollection<Quiz>> {
      console.log("addQuiz",url_,quiz,_httpOptions)
      return this.http.post<IPrimitiveCollection<Quiz>>(url_, quiz.array, _httpOptions);
  }
  quizPost(url_:string,quiz: IPrimitiveCollection<Quiz>){
      this.addQuiz(url_,quiz)
      .subscribe(q=>this.quizes_ =q);
  }


  //get listener
  quizResp(url_:string){
    this.getQuizResponse(url_)
    .subscribe(
      response=>{
        this.quizes_ = { ... response.body };
      }
    );
  }
  quizResp2(url_:string){
    console.log("quizResp2")
    this.getQuizResponse(url_)
    .subscribe(
      response=> {

        this.quizes_=response.body;
        console.log(this.quizes_);

        /*
        response.body.map(function(value_,index,array){
            //console.log(value_);
            //this._quizes.push(new Quiz(value_.key,value_.value));
        });
        */

      }
    );
  }


  //get value action
  getValueResponse(url_:string): Observable<HttpResponse<value_cl>> {
  return this.http.get<value_cl>(
    url_, { observe: 'response' });
  }

  //get value listener
  showValueResponse(url_:string) {
    this.getValueResponse(url_)
      // resp is of type `HttpResponse<Config>`
      .subscribe(resp => {
        // display its headers
        const keys = resp.headers.keys();
        //this.headers = keys.map(key =>`${key}: ${resp.headers.get(key)}`);

        // access the body directly, which is typed as `Config`.
        this.value_ = { ... resp.body };
        console.log('Value:',this.value_)
      });
  }

}
