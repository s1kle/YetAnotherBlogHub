import { Component } from '@angular/core';
import { ApiService } from '../shared/services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { detailedBlogVm } from '../shared/entities';
import { CommonModule } from '@angular/common';
import { error } from 'console';

@Component({
  selector: 'app-blog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './blog.component.html',
  styleUrl: './blog.component.css'
})
export class BlogComponent {
  blog: detailedBlogVm | null = null;

  constructor(private _api: ApiService, private _activatedRoute: ActivatedRoute, private _router: Router) { }

  ngOnInit() {
    this._activatedRoute.params.subscribe(params => {
      const id = params['id'];

      console.log('Getting blog ', id);

      this._api.getById(id).subscribe(
        response => this.blog = response, 
        error => {
          console.log(error);
          this._router.navigate(['']);
      })
    })
  }
}
