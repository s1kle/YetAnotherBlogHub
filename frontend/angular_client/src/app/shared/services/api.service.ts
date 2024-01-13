import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import {
  blogListVm, createBlogVm, createCommentVm, createLinkVm, createTagVm, detailedBlogVm,
  searchOptions, sortOptions, tagVm, updateBlogVm
} from '../entities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private _api = Config.apiUrl;

  constructor(private _http: HttpClient) { }

  //#region blogs
  getAllBlogs = (page: number, size: number, searchOptions?: searchOptions, sortOptions?: sortOptions) => {
    let httpParams = new HttpParams()
      .set('list.page', page)
      .set('list.size', size)

    if (searchOptions) {
      httpParams = httpParams
        .set('search.query', searchOptions.searchQuery)
        .set('search.properties', searchOptions.searchProperties ?? 'title');
    }

    if (sortOptions) {
      httpParams = httpParams
        .set('sort.property', sortOptions.sortProperty)
        .set('sort.direction', sortOptions.sortDirection ?? 'asc');
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

  linkTag = (blogId: string, vm: createLinkVm) =>
    this._http.post<string>(`${this._api}/blog/${blogId}/tag/link`, vm);

  unlinkTag = (blogId: string, tagId: string) =>
    this._http.delete<string>(`${this._api}/blog/${blogId}/tag/${tagId}/unlink`);

  createComment = (blogId: string, vm: createCommentVm) =>
    this._http.post<string>(`${this._api}/blog/${blogId}/comment/create`, vm);

  deleteComment = (blogId: string, commentId: string) =>
    this._http.delete<string>(`${this._api}/blog/${blogId}/comment/${commentId}/delete`);
  //#endregion

  //#region tags
  getAllTags = () =>
    this._http.get<tagVm[]>(`${this._api}/tags`);

  getUserTags = () =>
    this._http.get<tagVm[]>(`${this._api}/my-tags`);

  createTag = (vm: createTagVm) =>
    this._http.post<string>(`${this._api}/tag/create`, vm);

  deleteTag = (tagId: string) =>
    this._http.delete<string>(`${this._api}/tag/${tagId}/create`);
  //#endregion
}
