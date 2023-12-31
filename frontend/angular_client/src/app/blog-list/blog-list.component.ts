import { Component } from '@angular/core';
import { blogListVm } from '../shared/entities';
import { ApiService } from '../shared/services/api.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blog-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './blog-list.component.html',
  styleUrl: './blog-list.component.css'
})
export class BlogListComponent {
  blogList: blogListVm | null = null;
  page: number = 0;
  size: number = 10;

  constructor(private _api: ApiService, private _router: Router) { }

  ngOnInit() {
    this._api.getAll(this.page, this.size).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }

  getDetailedBlog(id: string) {
    this._router.navigate([`/blog/${id}`]);
  }
}
