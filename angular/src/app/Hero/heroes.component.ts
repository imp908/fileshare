import {Component,OnInit} from '@angular/core';
import {Hero} from './hero'
import { HEROES } from './mock-hero';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.css']
})
export class HeroesComponent implements OnInit {

//declare
hero0: Hero={id:1,name: 'Ws'};
selectedHero: Hero;

//init
heroes = HEROES;

  constructor() { }

  ngOnInit() {
  }

  onSelect(hero: Hero): void {
    this.selectedHero = hero;
  }
}
