import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MdDialog } from '@angular/material';

import { ApiService } from '../services/api.service';

import { ChangelogDialog } from './changelog.component';

@Component({
  templateUrl: './download.component.html'
})
export class DownloadComponent {

  public link: string;
  public downloading: boolean;
  public remaining: number;

  public selectedVersion: string;
  public allVersions: string[];

  public changelogs: string[];

  constructor(private api: ApiService, private dialog: MdDialog, private activatedRoute: ActivatedRoute) {
    this.activatedRoute.params.filter(p => p['version']).subscribe(p => {
      this.selectVersion(p['version']);
      this.api.getLatestVersion().subscribe(v => this.selectedVersion = v);
      this.startDownloading();
    });

    this.api.getAllVersions().subscribe(v => {
      this.allVersions = v;
      this.selectedVersion = this.allVersions[0];
      this.selectVersion(this.selectedVersion);
    });
    this.api.getChangelogs().subscribe(c => this.changelogs = c);
  }

  public selectVersion(version: string) {
    if (version === 'latest') {
      this.link = '/api/download/latest';
    } else {
      this.link = `/api/download/specific/${version}`;
    }
  }

  public timeout: any;
  public interval: any;

  public startDownloading() {
    this.cancelTimer();

    const time = 5;
    this.downloading = true;
    this.remaining = time;
    this.timeout = setTimeout(() => this.downloadLink(), time * 1000);
    this.interval = setInterval(() => {
        if (this.remaining > 0) {
          this.remaining--;
        } else {
          clearInterval(this.interval);
        }
      },
      1000);
  }

  public cancelTimer() {
    clearTimeout(this.timeout);
    clearInterval(this.interval);
    this.downloading = false;
  }

  private downloadLink() {
    if (window.location.hostname !== 'localhost' && window.location.hostname !== '127.0.0.1') {
      ga('send', 'event', { eventCategory: 'download', eventAction: 'download', eventLabel: this.link });
    }
    console.log(this.link);
    window.location.href = this.link;
  }

  public clickHere(event: MouseEvent) {
    event.preventDefault();
    this.cancelTimer();
    this.downloadLink();
  }

  public openChangelog(changelog: string) {
    this.api.getChangelog(changelog).subscribe(c => {
      this.dialog.open(ChangelogDialog,
        {
          data: {
            text: c,
            title: `Changelog for version ${changelog}`
          }
        });
    });
  }
}