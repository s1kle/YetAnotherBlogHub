import { Component } from '@angular/core';
import { AuthService } from '../shared/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin-callback',
  standalone: true,
  templateUrl: './signin-callback.component.html',
  styleUrl: './signin-callback.component.css'
})
export class SigninCallbackComponent {

  constructor(private _auth: AuthService, private _router: Router) { }

  ngOnInit() {
    this._auth.signinCallback().then(() => this._router.navigate(['']));
  }
}