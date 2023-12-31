import { Component } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { BlogListComponent } from '../blog-list/blog-list.component';
import { CreateBlogComponent } from '../create-blog/create-blog.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CreateBlogComponent, BlogListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  constructor(private _api: ApiService) { }
  test() {
    this._api.getAll(0, 1).subscribe((response) => console.log(response.blogs[0]), (error) => console.log(error));
  }
  test1() {
    this._api.getById('037f39d9-a184-4d2f-bc4c-916ae3067f41').subscribe((response) => console.log(response), (error) => console.log(error));
  }
}