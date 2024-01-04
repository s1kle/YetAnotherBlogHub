import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import { blogListVm, createBlogVm, detailedBlogVm, searchOptions, sortOptions, updateBlogVm } from '../entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = Config.apiUrl;
  private identityUrl = Config.identityUrl;
  
  constructor(private _http: HttpClient) {
  }

  getAllBlogs = (page: number, size: number, searchOptions?: searchOptions, sortOptions?: sortOptions) => {
    let httpParams = new HttpParams()
      .set('page', page)
      .set('size', size)

    if (searchOptions) {
      httpParams = httpParams
        .set('searchQuery', searchOptions.searchQuery)
        .set('searchProperties', searchOptions.searchProperties ?? 'title');
    }

    if (sortOptions) {
      httpParams = httpParams
        .set('sortProperty', sortOptions.sortProperty)
        .set('sortDirection', sortOptions.sortDirection ?? 'asc');
    }

    return this._http.get<blogListVm>(`${this.apiUrl}/Blog`, { params: httpParams });
  }

  getAllBlogsByUserId = (page: number, size: number, searchOptions?: searchOptions, sortOptions?: sortOptions) => {
    var httpParams = new HttpParams()
      .set('page', page)
      .set('size', size);

    if (searchOptions)
      httpParams
        .set('searchQuery', searchOptions.searchQuery)
        .set('searchProperties', searchOptions.searchProperties ?? 'title')

    if (sortOptions)
      httpParams
        .set('sortProperty', sortOptions.sortProperty)
        .set('sortDirection', sortOptions.sortDirection ?? 'asc')

    return this._http.get<blogListVm>(`${this.apiUrl}/Blog/All`, { params: httpParams });
  }

  getBlogById = (id: string) =>
    this._http.get<detailedBlogVm>(`${this.apiUrl}/Blog/Get/${id}`);

  createBlog = (vm: createBlogVm) =>
    this._http.post<string>(`${this.apiUrl}/Blog/Create`, vm);

  deleteBlog = (id: string) =>
    this._http.delete<string>(`${this.apiUrl}/Blog/Update${id}`);

  updateBlog = (id: string, vm: updateBlogVm) =>
    this._http.put<string>(`${this.apiUrl}/Blog/Delete${id}`, vm);
}
