import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  constructor(public api: ApiService, public router: Router) {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd &&
        window.location.hostname !== 'localhost' &&
        window.location.hostname !== '127.0.0.1') {
        ga('set', 'page', event.urlAfterRedirects);
        ga('send', 'pageview');
      }
    });

    try {
      if ('Notification' in window) {
        Notification.requestPermission();
      }

      navigator.serviceWorker.getRegistration('./check-for-update.worker.js')
        .then(r => { if (r != null) r.unregister(); });
      navigator.serviceWorker.getRegistration('./push').then(r => { if (r != null) r.unregister(); });

      navigator.serviceWorker.getRegistrations().then(() => {
        navigator.serviceWorker.register('./push-service.js', { scope: './push-service' })
          .then(registration => {
            return registration.pushManager.getSubscription()
              .then(subscription => {
                if (subscription)
                  return subscription;

                return registration.pushManager.subscribe({
                  userVisibleOnly: true,
                  applicationServerKey: urlB64ToUint8Array(
                    'BHPsziID1JZJtR8zHEgMWiaogmV9xT_U0grDDQUAB06MzaKrL1nBB9P2ifOu1qsNNThgk19l3K5x8Wh3doB3A44')
                });
              });
          })
          .then(subscription => {
            var postData = {};
            var rawKey = subscription.getKey ? subscription.getKey('p256dh') : '';
            postData['key'] =
              rawKey ? btoa(String.fromCharCode.apply(null, new Uint8Array(rawKey as ArrayBuffer))) : '';
            var rawAuthSecret = subscription.getKey ? subscription.getKey('auth') : '';
            postData['authSecret'] = rawAuthSecret
              ? btoa(String.fromCharCode.apply(null, new Uint8Array(rawAuthSecret as ArrayBuffer)))
              : '';

            postData['endpoint'] = subscription.endpoint;

            fetch('/api/push/register',
              {
                method: 'post',
                headers: {
                  'Content-type': 'application/json'
                },
                body: JSON.stringify(postData)
              }).then(r => {
              if (r.ok) {
                ga('send', 'event', { eventCategory: 'push', eventAction: 'subscribe', eventLabel: postData['key'] });
              }
            });
          });
      });
    } catch (err) {
      console.log(`Error when trying to register push service: ${err}`);
    }
  }


}

function urlB64ToUint8Array(base64String) {
  const padding = '='.repeat((4 - base64String.length % 4) % 4);
  const base64 = (base64String + padding)
    .replace(/\-/g, '+')
    .replace(/_/g, '/');

  const rawData = window.atob(base64);
  const outputArray = new Uint8Array(rawData.length);

  for (let i = 0; i < rawData.length; ++i) {
    outputArray[i] = rawData.charCodeAt(i);
  }
  return outputArray;
}