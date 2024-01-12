import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { CommonModule } from '@angular/common';
import { SigninRedirectComponent } from '../signin-redirect/signin-redirect.component';
import { SignoutRedirectComponent } from '../signout-redirect/signout-redirect.component';
import { User } from 'oidc-client-ts';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, SigninRedirectComponent, SignoutRedirectComponent],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  user: User | null = null;
  isSearching = false;

  constructor(private _auth: AuthService, private _router: Router) {
    this._auth.getEvents().addUserLoaded((user) => this.onUserLoaded(user));
    this._auth.getEvents().addUserUnloaded(() => this.onUserUnloaded())
  }

  redirect = () =>
    this._router.navigate([`/blog/create`]);

  async ngOnInit() {
    this.user = await this._auth.getUser();
  }

  onUserLoaded = (user: User) => {
    console.log('User logged in');
    this.user = user;
  }
  onUserUnloaded = () => {
    console.log('User logged out');
    this.user = null;
  }

  search = () => {
    console.log('Searching');
  }
}
