import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'changelog-dialog',
  template:
    '<h1 mat-dialog-title>{{data.title}}</h1><mat-dialog-content><markdown>{{data.text}}</markdown></mat-dialog-content>'
})
export class ChangelogDialog {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}
}