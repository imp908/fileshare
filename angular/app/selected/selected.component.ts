import { Component, OnInit , Input} from '@angular/core';
import {Hero} from '../hero'

@Component({
  selector: 'app-selected',
  templateUrl: './selected.component.html',
  styleUrls: ['./selected.component.css']
})
export class SelectedComponent implements OnInit {
  @Input() heroDetails: Hero;

  hero : Hero = {
    id:0,
    name: 'QuickSilver',
    birthdate: new Date(2011,1,1)
  };

  constructor() { }

  ngOnInit() {
  }

}
