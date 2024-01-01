import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-signout-redirect',
  standalone: true,
  imports: [],
  templateUrl: './signout-redirect.component.html',
  styleUrl: './signout-redirect.component.css'
})
export class SignoutRedirectComponent {
  
  constructor(private _auth: AuthService) { }

  signoutRedirect() {
    this._auth.signoutRedirect();
  }
}
