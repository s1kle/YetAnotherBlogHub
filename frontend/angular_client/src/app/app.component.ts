import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { SigninRedirectComponent } from './signin-redirect/signin-redirect.component';
import { AuthService } from './shared/services/auth.service';
import { SignoutRedirectComponent } from './signout-redirect/signout-redirect.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SigninRedirectComponent, SignoutRedirectComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angular_client';
  isAuthenticated = true;

  constructor(private _auth: AuthService) {
    _auth.isAuthenticated$.subscribe(state => {
      this.isAuthenticated = state;
    })
  }

  showToken() {
    console.log(localStorage.getItem('access_token'));
  }
}