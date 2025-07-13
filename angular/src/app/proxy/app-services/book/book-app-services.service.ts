import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetBaseList } from '../../ibase-services/models';
import type { BookDto, Create_Update_Book } from '../../iservices/book/models';

@Injectable({
  providedIn: 'root',
})
export class BookAppServicesService {
  apiName = 'Default';
  

  deleteByIdGuidById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'DELETE',
      url: `/api/app/book-app-services/${id}/by-id-guid`,
    },
    { apiName: this.apiName,...config });
  

  getAllByInput = (input: GetBaseList, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<BookDto>>({
      method: 'GET',
      url: '/api/app/book-app-services',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getByIdGuidById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'GET',
      url: `/api/app/book-app-services/${id}/by-id-guid`,
    },
    { apiName: this.apiName,...config });
  

  insertByInput = (input: Create_Update_Book, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'POST',
      url: '/api/app/book-app-services',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateByInput = (input: Create_Update_Book, config?: Partial<Rest.Config>) =>
    this.restService.request<any, BookDto>({
      method: 'PUT',
      url: '/api/app/book-app-services',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
