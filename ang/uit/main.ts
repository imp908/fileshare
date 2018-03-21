import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { environment } from './environments/environment';

import { AppModule } from './app/app.module';
import { AppModule2 } from './app2/app2.module';
import { AppModule3 } from './app3/app3.module';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule3)
  .catch(err => console.log(err));
