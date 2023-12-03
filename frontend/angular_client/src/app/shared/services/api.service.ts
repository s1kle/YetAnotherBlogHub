import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Config } from '../config';
import { Blog, DetailedBlog, createBlogVM, updateBlogVM } from '../apiEntities';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = Config.apiUrl;
  
  constructor(private _http: HttpClient) { }
  
  getBlogList() {
    return this._http.get<{blogs: Blog[]}>(`${this.apiUrl}/Blog`, { headers: this.getHeader() });
  }

  getBlogDetails(blogId: string) {
    return this._http.get<{blog: DetailedBlog}>(`${this.apiUrl}/Blog/${blogId}`, { headers: this.getHeader() });
  }

  createBlog(blog: createBlogVM) {
    return this._http.post(`${this.apiUrl}/Blog`, blog, { headers: this.getHeader() });
  }

  updateBlog(blogId: string, blog: updateBlogVM) {
    return this._http.put(`${this.apiUrl}/Blog/${blogId}`, blog, { headers: this.getHeader() });
  }

  deleteBlog(blogId: string) {
    return this._http.delete(`${this.apiUrl}/Blog/${blogId}`, { headers: this.getHeader() })
  }

  private getHeader() {
    return new HttpHeaders({
      'Cache-Control':  'no-cache, no-store, must-revalidate, post-check=0, pre-check=0',
      'Pragma': 'no-cache',
      'Authorization': `Bearer ${localStorage.getItem('access_token')}`
    });
  }
}
