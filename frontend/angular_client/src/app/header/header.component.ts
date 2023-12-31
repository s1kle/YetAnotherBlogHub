import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SigninRedirectComponent } from '../signin-redirect/signin-redirect.component';
import { SignoutRedirectComponent } from '../signout-redirect/signout-redirect.component';
import { AuthService } from '../shared/services/auth.service';
import { User } from 'oidc-client-ts';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, SigninRedirectComponent, SignoutRedirectComponent],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  user: User | null = null;
  title = 'Angular Client';

  constructor(private _auth: AuthService) {
    this._auth.getEvents().addUserLoaded((user) => this.onUserLoaded(user));
    this._auth.getEvents().addUserUnloaded(() => this.onUserUnloaded())
  }

  async ngOnInit() {
    this.user = await this._auth.getUser();
  }

  onUserLoaded(user: User) {
    console.log('User logged in');
    this.user = user;
  }
  onUserUnloaded() {
    console.log('User logged out');
    this.user = null;
  }
}
