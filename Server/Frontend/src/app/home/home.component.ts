import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

import { ApiService } from '../services/api.service';

@Component({
  templateUrl: './home.component.html'
})
export class HomeComponent {

  public form: FormGroup;
  public formError: string;
  public formSend: boolean;

  constructor(private api: ApiService) {
    this.form = new FormGroup({
      'mode': new FormControl('0'),
      'title': new FormControl('', Validators.compose([Validators.required, Validators.maxLength(70)])),
      'contact': new FormControl('', Validators.required),
      'message': new FormControl('', Validators.required)
    });

    //this.http.get('https://api.github.com/repos/jammin411/MatchmakingMonitor/issues?state=open').subscribe(r => console.log(r.json()));
  }

  public submit(formValue: {}) {
    if (this.form.valid) {
      this.api.postForm(formValue).subscribe(result => {
        if (result) {
          this.formError = null;
          this.form.reset({ 'mode': 0 });
          this.formSend = true;
        } else {
          this.formError = 'Error while submitting form. Please try again.';
        }
      });
    }
  }

  public downloadApp(event: MouseEvent) {
    event.preventDefault();
    if (window.location.hostname !== 'localhost' && window.location.hostname !== '127.0.0.1') {
      ga('send', 'event', { eventCategory: 'download', eventAction: 'download-beta', eventLabel: 'android-app' });
    }
    window.location.href = '/wowsaio-beta.apk';
  }

  public downloadMonitor(event: MouseEvent) {
    event.preventDefault();
    if (window.location.hostname !== 'localhost' && window.location.hostname !== '127.0.0.1') {
      ga('send', 'event', { eventCategory: 'download', eventAction: 'download-beta', eventLabel: 'monitor' });
    }
    window.location.href = '/MatchmakingMonitor-Beta.zip';
  }
}