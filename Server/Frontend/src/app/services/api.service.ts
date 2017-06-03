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
				this.setDbValue(this._latestVersion);
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

	private setDbValue(version: string) {
		let dbRequest = indexedDB.open('versionDb', 1);
		let db: IDBDatabase;
		let store: IDBObjectStore;

		dbRequest.onsuccess = () => {
			db = dbRequest.result;
			store = db.transaction('version', 'readwrite').objectStore('version');
			this.checkAndSetVersion(store, version).then(() => {
				db.close();
			});
		}
		dbRequest.onupgradeneeded = () => {
			db = dbRequest.result;
			store = db.createObjectStore('version');
			this.checkAndSetVersion(store, version).then(() => {
				db.close();
			});
		}
	}

	private checkAndSetVersion(store: IDBObjectStore, version: string): Promise<any> {
		return new Promise((resolve) => {
			try {
				const dbVersionR = store.get('current');
				dbVersionR.onsuccess = () => {
					const dbVersion = dbVersionR.result;
					if (dbVersion !== version) {
						const setRequest = store.put(version, 'current');
						setRequest.onsuccess = () => {
							this.showNotification(version);
							resolve();
						}
					}
					resolve();
				}
			} catch (e) {
				console.log('Error when checking for new version');
			}
		});
	}

	private showNotification(version: string) {
		if ('Notification' in window && (Notification as any).permission === 'granted') {
			const title = 'MatchmakingMonitor';
			const options: NotificationOptions = {
				body: `New version released (${version})`,
				icon: 'favicon.ico'
			};
			const notification = new Notification(title, options);
			notification.onclick = () => {
				notification.close();
				this.router.navigateByUrl('/download/latest');
			}
		}
	}
}