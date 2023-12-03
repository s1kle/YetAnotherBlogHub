import { Injectable } from '@angular/core';
import { Config } from '../config';
import { User, UserManager, UserManagerSettings } from 'oidc-client-ts';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  isAuthenticated$: Observable<boolean>;

  private _isAuthenticated: BehaviorSubject<boolean>;
  private _userManager: UserManager;
  private get identitySettings(): UserManagerSettings {
    return {
      authority: Config.identityUrl,
      client_id: Config.clientId,
      client_secret: Config.clientSecret,
      scope: 'openid profile BlogHubAPI',
      response_type: 'code',
      redirect_uri: `${Config.baseUrl}/signin-callback`,
      post_logout_redirect_uri: `${Config.baseUrl}/signout-callback`
    }
  }
  
  
  constructor() { 
    this._userManager = new UserManager(this.identitySettings)
    this._isAuthenticated = new BehaviorSubject<boolean>(false);
    this.isAuthenticated$ = this._isAuthenticated.asObservable();
  }
  

  signinRedirect() {
    return this._userManager.signinRedirect();
  }
  signinCallback() {
    return this._userManager.signinCallback();
  }
  signoutRedirect(user: User) {
    this._userManager.clearStaleState();
    this._userManager.removeUser();
    return this._userManager.signoutRedirect({ 'id_token_hint': user.id_token });
  }
  signoutCallback() {
    this._userManager.clearStaleState();
    this._userManager.removeUser();
    return this._userManager.signoutCallback();
  }
  setAccessToken(access_token: string) {
    localStorage.setItem('access_token', access_token ?? '');
  }
  updateAuthState(state: boolean) {
    this._isAuthenticated.next(state);
  }
  getUser() {
    return this._userManager.getUser();
  }
}
