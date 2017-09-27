import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Http, Headers } from '@angular/http';

import { Observable } from 'rxjs/Observable';


@Injectable()
export class ApiService {
  constructor(private http: Http, private router: Router) {
    this.getLatestVersion().subscribe();
  }

  private _latestVersion: string;

  public get latestVersion(): string {
    return this._latestVersion;
  }

  public getLatestVersion(): Observable<string> {
    return this.http.get('/api/version/latest')
      .map(response => response.json(), err => console.log(err))
      .do(lv => {
        this._latestVersion = (lv as string);
      });
  }

  public getAllVersions(): Observable<string[]> {
    return this.http.get('/api/version/all')
      .map(response => response.json(), err => console.log(err));
  }

  public postForm(formValue: {}): Observable<boolean> {
    return this.http.post('/api/form/submit',
        JSON.stringify(formValue),
        { headers: new Headers({ 'content-type': 'application/json' }) })
      .map(response => response.json() as boolean, err => console.log(err));
  }

  public getChangelogs(): Observable<string[]> {
    return this.http.get('/api/changelog/list').map(response => response.json() as string[]);
  }

  public getChangelog(changelog: string) {
    return this.http.get(`/api/changelog/detail/${changelog}`).map(response => response.json() as string);
  }
}