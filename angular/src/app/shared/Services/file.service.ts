import { HttpClient, HttpParams, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiUrl = environment.apis.default.url; // Update to your backend base URL

  constructor(private http: HttpClient) {}

  uploadFile(id: string, file: File): Observable<boolean> {
    const formData = new FormData();
    formData.append('file', file);
    const params = new HttpParams().set('Id', id);
    return this.http.post<boolean>(`${this.apiUrl}/UploadFileAsync`, formData, { params });
  }
  deleteFile(id: string): Observable<boolean> {
  const params = new HttpParams().set('Id', id);
  return this.http.post<boolean>(`${this.apiUrl}/DeleteFileAsync`, null, { params });
}
}
