import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import { blogListVm, createBlogVm, detailedBlogVm, updateBlogVm } from '../entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = Config.apiUrl;
  private identityUrl = Config.identityUrl;
  
  constructor(private _http: HttpClient) {
  }

  getAllBlogs = (page: number, size: number) =>
    this._http.get<blogListVm>(`${this.apiUrl}/Blog`, { params: new HttpParams()
      .set('page', page)
      .set('size', size)
    });

  getBlogById = (id: string) =>
    this._http.get<detailedBlogVm>(`${this.apiUrl}/Blog/${id}`);

  createBlog = (vm: createBlogVm) =>
    this._http.post<string>(`${this.apiUrl}/Blog`, vm);

  deleteBlog = (id: string) =>
    this._http.delete<string>(`${this.apiUrl}/Blog/${id}`);

  updateBlog = (id: string, vm: updateBlogVm) =>
    this._http.put<string>(`${this.apiUrl}/Blog/${id}`, vm);
}
