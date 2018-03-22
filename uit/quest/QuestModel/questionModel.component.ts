
import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {QuestionTypes,QuestionItem}   from '../questionMd.component';

@Component({
  selector: 'question-model'
  ,templateUrl: './QuestionTp.component.html'
  ,providers:[QuestionTypes,QuestionItem]
})

export class QuestionItems{
  questions: QuestionItem ;
  form: FormGroup;

  constructor ()
  {

  }

}
