import { Component } from '@angular/core';

@Component({
  selector: 'app-click-me',
  templateUrl: 'click_me.component.html'
})

export class ClickMeComponent {
  clickMessage = '';

  onClickMe() {
    if(this.clickMessage==''){
      this.clickMessage = 'Cliked!';
    }else{
      this.clickMessage = '';
    }
  }
}
