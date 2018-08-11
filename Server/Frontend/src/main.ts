import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic().bootstrapModule(AppModule).then(() => {
  window['cookieconsent'].initialise({
    "cookie": {
      "domain": environment.domain
    },
    "position": "bottom",
    "theme": "classic",
    "palette": {
      "popup": {
        "background": "#3f51b5",
        "text": "#ffffff",
        "link": "#ffffff"
      },
      "button": {
        "background": "#ff9800",
        "text": "#ffffff",
        "border": "transparent"
      }
    },
    "type": "info",
    "content": {
      "message": "This website uses cookies to ensure you get the best experience on our website.",
      "dismiss": "Got it!",
      "deny": "Refuse cookies",
      "link": "Learn more",
      "href": "/privacy"
    }
  });
});