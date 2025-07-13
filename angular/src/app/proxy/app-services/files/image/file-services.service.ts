import { RestService, Rest } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { IFormFile } from '../../../microsoft/asp-net-core/http/models';

@Injectable({
  providedIn: 'root',
})
export class FileServicesService {
  apiName = 'Default';
  

  deleteFile = (Id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/DeleteFileAsync',
      params: { id: Id },
    },
    { apiName: this.apiName,...config });
  

  getFileUrl = (Id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, string>({
      method: 'GET',
      responseType: 'text',
      url: '/GetFileUrlAsync',
      params: { id: Id },
    },
    { apiName: this.apiName,...config });
  

  uploadFile = (Id: string, file: IFormFile, config?: Partial<Rest.Config>) =>
    this.restService.request<any, boolean>({
      method: 'POST',
      url: '/UploadFileAsync',
      params: { id: Id },
      body: file,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
