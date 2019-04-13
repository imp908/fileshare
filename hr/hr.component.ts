import { Component, Input } from '@angular/core';

import { Hero } from './hero';

@Component({
  selector: 'app-hero-child',
  templateUrl: './hr.component.html'
})

export class HeroChildComponent {
  @Input() hero_ch: Hero;
  @Input('master_ch') masterName: string;
}
