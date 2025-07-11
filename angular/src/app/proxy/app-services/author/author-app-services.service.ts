import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { GetBaseList } from '../../ibase-services/models';
import type { AuthorDto, Create_Update_Author } from '../../iservices/author/models';

@Injectable({
  providedIn: 'root',
})
export class AuthorAppServicesService {
  apiName = 'Default';
  

  deleteByIdGuidById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'DELETE',
      url: `/api/app/author-app-services/${id}/by-id-guid`,
    },
    { apiName: this.apiName,...config });
  

  getAllByInput = (input: GetBaseList, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<AuthorDto>>({
      method: 'GET',
      url: '/api/app/author-app-services',
      params: { filter: input.filter, sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  getByIdGuidById = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuthorDto>({
      method: 'GET',
      url: `/api/app/author-app-services/${id}/by-id-guid`,
    },
    { apiName: this.apiName,...config });
  

  insertByInput = (input: Create_Update_Author, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuthorDto>({
      method: 'POST',
      url: '/api/app/author-app-services',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  updateByInput = (input: Create_Update_Author, config?: Partial<Rest.Config>) =>
    this.restService.request<any, AuthorDto>({
      method: 'PUT',
      url: '/api/app/author-app-services',
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
