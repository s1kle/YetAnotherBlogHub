import { Component } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { detailedBlogVm, updateBlogVm } from '../shared/entities';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.css'
})
export class BlogComponent {
  isEditMode = false;
  blog: detailedBlogVm | null = null;

  constructor(private _api: ApiService, private _activatedRoute: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    this._activatedRoute.params.subscribe(params => {
      const id = params['id'];

      console.log('Getting blog ', id);

      this._api.getBlogById(id).subscribe(
        response => this.blog = response, 
        error => {
          console.log(error);
          this._router.navigate(['']);
      })
    })
  }

  delete = () => {
    const id = this.blog?.id ?? '';

    this._api.deleteBlog(id).subscribe(
      () => this._router.navigate(['']),
      error => console.log(error)
    );
  }

  edit = () =>
    this.isEditMode = true;

  cancel = () =>
    this.isEditMode = false;

  update = () => {
    const id = this.blog?.id ?? '';

    const blog: updateBlogVm = {
      title: this.blog?.title ?? '',
      details: this.blog?.details ?? null
    };

    this._api.updateBlog(id, blog).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  goBack = () => 
    this._router.navigate(['']);
}
