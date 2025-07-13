import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookComponent } from './Component/book/book.component';
import { authGuard } from '@abp/ng.core';

const routes: Routes = [
  {
      path:'',
      pathMatch:'full',
      component:BookComponent,
      canActivate: [authGuard],
    },
    {
      path:'books',
      pathMatch:'full',
      component:BookComponent,
      canActivate: [authGuard],
    },
    {
      path:'**',
      component:BookComponent,
      canActivate: [authGuard],
    }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BookRoutingModule { }
