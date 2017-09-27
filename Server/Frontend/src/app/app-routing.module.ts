import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { DownloadComponent } from './download/download.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'download',
    component: DownloadComponent
  },
  {
    path: 'download/:version',
    component: DownloadComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}