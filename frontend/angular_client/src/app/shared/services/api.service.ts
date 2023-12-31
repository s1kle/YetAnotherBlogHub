import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import { blogListVm, createBlogVm, detailedBlogVm } from '../entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = Config.apiUrl;
  private identityUrl = Config.identityUrl;
  
  constructor(private _http: HttpClient) {
  }

  getAll = (page: number, size: number) =>
    this._http.get<blogListVm>(`${this.apiUrl}/Blog`, { params: new HttpParams()
      .set('page', page)
      .set('size', size)
    });

  getById = (id: string) =>
    this._http.get<detailedBlogVm>(`${this.apiUrl}/Blog/${id}`);

  createBlog = (vm: createBlogVm) =>
    this._http.post<string>(`${this.apiUrl}/Blog`, vm);
}
