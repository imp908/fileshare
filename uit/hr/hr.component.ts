import { Component, Input } from '@angular/core';

import { Hero } from './hero';

@Component({
  selector: 'app-hero-child',
  templateUrl: './hr.component.html'
})

export class HeroChildComponent {
  @Input() hero_ch1: Hero;
  @Input('master_ch3') masterName: string;
}
