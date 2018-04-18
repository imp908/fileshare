import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';
import { App7Module } from './app/app7/app7.module';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(App7Module)
  .catch(err => console.log(err));
