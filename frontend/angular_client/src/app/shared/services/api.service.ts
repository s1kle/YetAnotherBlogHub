import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import { blogListVm, createBlogVm, createLinkVm, createTagVm, detailedBlogVm,
  searchOptions, sortOptions, tagListVm, tagVm, updateBlogVm } from '../entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private _api = Config.apiUrl;
  
  constructor(private _http: HttpClient) { }
  
  //#region blogs
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

    return this._http.get<blogListVm>(`${this._api}/blogs`, { params: httpParams });
  }

  getUserBlogs = (page: number, size: number, searchOptions?: searchOptions, sortOptions?: sortOptions) => {
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

    return this._http.get<blogListVm>(`${this._api}/my-blogs`, { params: httpParams });
  }

  getBlog = (blogId: string) =>
    this._http.get<detailedBlogVm>(`${this._api}/blog/${blogId}`)
    
  createBlog = (vm: createBlogVm) =>
    this._http.post<string>(`${this._api}/blog/create`, vm);
  
  deleteBlog = (blogId: string) =>
    this._http.delete<string>(`${this._api}/blog/${blogId}/delete`);
  
  updateBlog = (blogId: string, vm: updateBlogVm) =>
    this._http.put<string>(`${this._api}/blog/${blogId}/update`, vm);

  getBlogTags = (blogId: string) =>
    this._http.get<tagListVm>(`${this._api}/blog/${blogId}/tags`);

  linkTag = (blogId: string, vm: createLinkVm) =>
    this._http.post<string>(`${this._api}/blog/${blogId}/tag/link`, vm);

  unlinkTag = (blogId: string, tagId: string) =>
    this._http.delete<string>(`${this._api}/blog/${blogId}/tag/${tagId}/unlink`);
  //#endregion

  //#region tags
  getAllTags = () =>
    this._http.get<tagListVm>(`${this._api}/tags`);
  
  getUserTags = () =>
   this._http.get<tagListVm>(`${this._api}/my-tags`);

  getTag = (tagId: string) =>
    this._http.get<tagVm>(`${this._api}/tag/${tagId}`);

  createTag = (vm: createTagVm) =>
    this._http.post<string>(`${this._api}/tag/create`, vm);

  deleteTag = (tagId: string) =>
    this._http.delete<string>(`${this._api}/tag/${tagId}/create`);
  //#endregion
}
