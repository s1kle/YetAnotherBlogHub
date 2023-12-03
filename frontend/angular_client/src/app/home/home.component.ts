import { Component } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { Blog, createBlogVM } from '../shared/apiEntities';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private blog: any;
  private id = '';
  constructor(private _api: ApiService) {
    this.blog = {
      title: 'new String123',
      details: 'new String123'
    }
  }

  getBlogList() { 
    this._api.getBlogList().subscribe(response => {
      console.log(response.blogs);}, 
      (error) => console.log(error));
  }

  updateBlog() {
    this._api.updateBlog(this.id, this.blog)
    .subscribe((response) => console.log(response),
      (error) => console.log(error));
  }

  createBlog() {
    this._api.createBlog(this.blog)
    .subscribe((response) => console.log(response),
      (error) => console.log(error));
  }

  deleteBlog() {
    this._api.deleteBlog(this.id)
    .subscribe((response) => console.log(response),
      (error) => console.log(error));
  }

  getBlogDetains() {
    this._api.getBlogDetails(this.id)
    .subscribe((response) => console.log(response),
      (error) => console.log(error));
  }
}