import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule5 } from './app5/app5.module';
import {App6Module} from './app/app6/app6.module';


import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(App6Module)
  .catch(err => console.log(err));
