import { Component } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { detailedBlogVm, tagVm, updateBlogVm } from '../shared/entities';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../shared/services/auth.service';
import { User } from 'oidc-client-ts';
import { response } from 'express';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.css'
})
export class BlogComponent {
  user: User | null = null;
  isEditMode = false;
  isCommentMode = false;
  blog: detailedBlogVm | null = null;
  content: string = '';
  name: string = '';
  tags: tagVm[] | undefined;

  constructor(private _api: ApiService,
    private _activatedRoute: ActivatedRoute,
    private _router: Router,
    private _auth: AuthService) { }

  async ngOnInit() {
    this.user = await this._auth.getUser();
    console.log(this.user);

    this._activatedRoute.params.subscribe(params => {
      const id = params['id'];

      this._api.getBlog(id).subscribe(
        response => this.blog = response,
        error => {
          console.log(error);
          this._router.navigate(['']);
        })
    })
  }

  commentStart = () =>
    this.isCommentMode = true;
  commentCancel = () =>
    this.isCommentMode = false;

  editStart = () =>
    this.isEditMode = true;
  editCancel = () =>
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
  delete = () => {
    const id = this.blog?.id ?? '';

    this._api.deleteBlog(id).subscribe(
      () => this._router.navigate(['']).then(() => window.location.reload()),
      error => console.log(error)
    );
  }

  createComment = () => {
    const id = this.blog?.id ?? '';

    this._api.createComment(id, { content: this.content }).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  addTag = (tagId: string) => {
    const id = this.blog?.id ?? '';

    this._api.linkTag(id, { tagId: tagId }).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  removeTag = (tagId: string) => {
    const id = this.blog?.id ?? '';

    this._api.unlinkTag(id, tagId).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  createTag = () => {
    this._api.createTag({ name: this.name }).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  deleteTag = (tagId: string) => {
    this._api.deleteTag(tagId).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  deleteComment = (commentId: string) => {
    const id = this.blog?.id ?? '';

    this._api.deleteComment(id, commentId).subscribe(
      () => window.location.reload(),
      error => console.log(error));
  }

  getTags = () => {
    this._api.getAllTags().subscribe(
      (response) => this.tags = response,
      (error) => console.log(error)
    );
  }

  goBack = () =>
    this._router.navigate(['']);
}
