import { RoutesService, eLayoutType } from '@abp/ng.core';
import { inject, provideAppInitializer } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  provideAppInitializer(() => {
    configureRoutes();
  }),
];

function configureRoutes() {
  const routes = inject(RoutesService);
  routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },      
      {
        path: '/Authors',
        name: 'author',
        iconClass: 'fas fa-list',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'BookStore.Authors.List',
      },
      {
        path: '/books',
        name: 'Books',
        iconClass: 'fas fa-list',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'BookStore.Books.List',
      },
  ]);
}
