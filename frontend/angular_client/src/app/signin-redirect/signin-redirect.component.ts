import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-signin-redirect',
  standalone: true,
  imports: [],
  templateUrl: './signin-redirect.component.html',
  styleUrl: './signin-redirect.component.css'
})
export class SigninRedirectComponent {
  constructor(private _auth: AuthService) { }

  public signinRedirect() {
    this._auth.signinRedirect();
  }
}
