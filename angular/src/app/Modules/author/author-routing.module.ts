import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorComponent } from './Component/author/author.component';
import { authGuard } from '@abp/ng.core';

const routes: Routes = 
[{
    path:'',
    pathMatch:'full',
    component:AuthorComponent,
    canActivate: [authGuard],
  },
  {
    path:'Authors',
    pathMatch:'full',
    component:AuthorComponent,
    canActivate: [authGuard],
  },
  {
    path:'**',
    component:AuthorComponent,
    canActivate: [authGuard],
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule { }
