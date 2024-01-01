import { Component } from '@angular/core';
import { createBlogVm } from '../shared/entities';
import { ApiService } from '../shared/services/api.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-create-blog',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './create-blog.component.html',
  styleUrl: './create-blog.component.css'
})
export class CreateBlogComponent {
  title: string = '';
  details: string = '';

  constructor(private _api: ApiService) { }

  createBlog() {
    const blog: createBlogVm = {
      title: this.title,
      details: this.details ?? null
    };

    this._api.createBlog(blog).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }
}