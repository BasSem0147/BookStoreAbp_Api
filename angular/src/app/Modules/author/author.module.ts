import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthorRoutingModule } from './author-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap'; // add this line
import { ThemeSharedModule } from '@abp/ng.theme.shared';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,ThemeSharedModule ,
    SharedModule,NgxPaginationModule,NgbDatepickerModule,
    AuthorRoutingModule
  ]
})
export class AuthorModule { }
