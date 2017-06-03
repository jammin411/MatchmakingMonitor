import { Component, Inject } from '@angular/core';
import { MD_DIALOG_DATA } from '@angular/material';

@Component({
	selector: 'changelog-dialog',
	template:
		'<h1 md-dialog-title>{{data.title}}</h1><md-dialog-content><markdown>{{data.text}}</markdown></md-dialog-content>'
})
export class ChangelogDialog {
	constructor(@Inject(MD_DIALOG_DATA) public data: any) {}
}