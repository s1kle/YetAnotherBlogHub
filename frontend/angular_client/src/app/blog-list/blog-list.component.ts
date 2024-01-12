import { Component } from '@angular/core';
import { blogListVm } from '../shared/entities';
import { ApiService } from '../shared/services/api.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blog-list.component.html',
  styleUrl: './blog-list.component.css'
})
export class BlogListComponent {
  blogList: blogListVm | null = null;
  size: number = 10;
  page: number = 0;

  constructor(private _api: ApiService, private _router: Router) { }

  ngOnInit() {
    this._api.getAllBlogs(this.page, this.size).subscribe(
      response => {
        this.blogList = response;
      }, 
      error => console.log(error))
  }

  goToBlogDetails = (blogId: string) =>
    this._router.navigate([`/blog/${blogId}`]);

  goToNextPage = () => {
    this._api.getAllBlogs(++this.page, this.size).subscribe(
      response => {
        this.blogList = response
      }, 
      error => console.log(error))
  }

  goToPreviousPage = () => {
    this._api.getAllBlogs(--this.page, this.size).subscribe(
      response => {
        this.blogList = response
      }, 
      error => console.log(error))
  }
}