import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule, MdProgressSpinnerModule, MdSelectModule, MdCardModule, MdListModule } from '@angular/material';
import { MarkdownModule } from 'angular2-markdown';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { HomeComponent } from './home/home.component';
import { DownloadComponent } from './download/download.component';
import { ChangelogDialog } from './download/changelog.component';

import { ApiService } from './services/api.service';

@NgModule({
	declarations: [
		AppComponent,
		HomeComponent,
		DownloadComponent,
		ChangelogDialog
	],
	entryComponents: [ChangelogDialog],
	imports: [
		BrowserModule,
		HttpModule,
		FormsModule,
		ReactiveFormsModule,
		BrowserAnimationsModule,
		NoopAnimationsModule,
		AppRoutingModule,
		MaterialModule,
		MdProgressSpinnerModule,
		MdSelectModule,
		MdCardModule,
		MdListModule,
		MarkdownModule.forRoot()
	],
	providers: [ApiService],
	bootstrap: [AppComponent]
})
export class AppModule {
}