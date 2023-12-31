import { Component } from '@angular/core';
import { BlogListComponent } from '../blog-list/blog-list.component';
import { CreateBlogComponent } from '../create-blog/create-blog.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, CreateBlogComponent, BlogListComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  isCreating = false;

  changeState = () =>
    this.isCreating = !this.isCreating;
}