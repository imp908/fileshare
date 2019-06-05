
import { Component, Input, OnInit }   from '@angular/core';
import { FormGroup,FormControl }      from '@angular/forms';

import {QuestionTypes,QuestionItems,QuestionType,QuestionItem} from '../questionMd.component';

@Component({
  selector: 'question-model'
  ,templateUrl: './QuestionModel.component.html'
  ,providers:[QuestionTypes,QuestionItems]
})

export class QuestionModels{
  @Input() selectedType: QuestionType;
  form_: FormGroup;
  questionItems: QuestionItems ;

  constructor ()
  {

  }

  ngOnInit(){
    
  }

}
