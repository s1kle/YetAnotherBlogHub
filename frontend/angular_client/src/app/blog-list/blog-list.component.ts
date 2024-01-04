import { Component } from '@angular/core';
import { blogListVm, searchOptions, sortOptions } from '../shared/entities';
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
  sortOptions: sortOptions = { sortProperty: 'title', sortDirection: 'asc'};
  searchOptions: searchOptions = { searchQuery: '', searchProperties: 'title'}
  sortByTitle = true;
  sortByDetails = false;
  blogList: blogListVm | null = null;
  size: number = 10;
  page: number = 0;

  constructor(private _api: ApiService, private _router: Router) { }

  ngOnInit() {
    this._api.getAllBlogs(this.page, this.size).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }

  search = (query: string) => {
    const options = [];

    switch (this.sortByDetails + " " + this.sortByTitle)
    {
      case "true false":
        options.push('details');
        break;
      case "true true":
        options.push('details');
        options.push('title');
        break;
      default:
        options.push('title');
        break;
    }
    
    const properties = options.join(' ');

    this.searchOptions.searchQuery = query;
    this.searchOptions.searchProperties = properties;



    this._api.getAllBlogs(this.page, this.size, this.searchOptions, this.sortOptions).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }

  confirm = () => {
    this._api.getAllBlogs(this.page, this.size, this.searchOptions, this.sortOptions).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }

  goToBlogDetails = (id: string) =>
    this._router.navigate([`/blog/${id}`]);

  goToNextPage = () => {
    this.page++;

    this._api.getAllBlogs(this.page, this.size).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }

  goToPreviousPage = () => {
    this.page--;

    this._api.getAllBlogs(this.page, this.size).subscribe(
      response => this.blogList = response, 
      error => console.log(error))
  }
}