import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signout-callback',
  standalone: true,
  imports: [],
  templateUrl: './signout-callback.component.html',
  styleUrl: './signout-callback.component.css'
})
export class SignoutCallbackComponent {
  constructor(private _auth: AuthService, private _router: Router) { }

  ngOnInit() {
    this._auth.signoutCallback().then(result => {
      this._auth.updateAuthState(false);
      this._auth.setAccessToken('');
      this.redirect();
    });
  }

  redirect() {
    this._router.navigate(['']);
  }
}
