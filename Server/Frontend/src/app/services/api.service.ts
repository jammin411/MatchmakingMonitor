import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, pipe } from 'rxjs/Rx';
import { tap } from 'rxjs/operators';


@Injectable()
export class ApiService {
  constructor(private http: HttpClient, private router: Router) {
    this.getLatestVersion().subscribe();
  }

  private _latestVersion: string;

  public get latestVersion(): string {
    return this._latestVersion;
  }

  public getLatestVersion(): Observable<string> {
    return this.http.get<string>('/api/version/latest')
      .pipe(tap(lv => this._latestVersion = (lv as string)));
  }

  public getAllVersions(): Observable<string[]> {
    return this.http.get<string[]>('/api/version/all');
  }

  public postForm(formValue: {}): Observable<boolean> {
    return this.http.post<boolean>('/api/form/submit',
      JSON.stringify(formValue),
      { headers: new HttpHeaders({ 'content-type': 'application/json' }) });
  }

  public getChangelogs(): Observable<string[]> {
    return this.http.get<string[]>('/api/changelog/list');
  }

  public getChangelog(changelog: string): Observable<string> {
    return this.http.get<string>(`/api/changelog/detail/${changelog}`);
  }
}