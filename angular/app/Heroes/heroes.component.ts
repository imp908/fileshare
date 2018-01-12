import { Component, OnInit } from '@angular/core';
import { Hero } from '../hero';
import { HEROES } from '../mocked-heroes';

@Component({
  selector: 'app-heroes',
  templateUrl: './heroes.component.html',
  styleUrls: ['./heroes.component.css']
})

export class HeroesComponent implements OnInit {

  heroSelected:Hero;

  heroes=HEROES;

  constructor() { }

  ngOnInit() {
  }

  clicked(hero1:Hero)
  {
  this.heroSelected=hero1;
  }

}
