import { Component } from '@angular/core';

import { HEROES } from './hero';

import {ClickMeComponent} from '../click/click_me.component';

@Component({
  selector: 'app-hero-parent',
  templateUrl:'hr-pr.component.html'
})
export class HeroParentComponent {
  heroes = HEROES;
  master_p2 = 'Master inputed str';
}
