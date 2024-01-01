import { Injectable } from '@angular/core';
import { Config } from '../config';
import { User, UserManager, UserManagerSettings } from 'oidc-client-ts';
import { CoolLocalStorage } from '@angular-cool/storage';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private _userManager: UserManager;
  private _userManagerSettings: UserManagerSettings = {
      authority: Config.identityUrl,
      client_id: Config.clientId,
      client_secret: Config.clientSecret,
      scope: Config.scope,
      response_type: 'code',
      redirect_uri: `${Config.baseUrl}/signin-callback`,
      post_logout_redirect_uri: `${Config.baseUrl}/signout-callback`
    }
  
  constructor(private _localStorage: CoolLocalStorage) { 
    this._userManager = new UserManager(this._userManagerSettings);
    this._userManager.events.addUserLoaded((user) => this.onUserLoaded(user));
    this._userManager.events.addUserUnloaded(() => this.onUserUnloaded());
  }

  getEvents = () =>
    this._userManager.events;

  getUser = () => this._userManager.getUser();

  private async onUserLoaded(user: User) {
    this._localStorage.setItem('access_token', user.access_token)
  }

  private onUserUnloaded() {
    this._localStorage.clear();
  }

  //#region signin/out
  signinRedirect() {
    return this._userManager.signinRedirect();
  }

  signinCallback() {
    return this._userManager.signinCallback();
  }

  async signoutRedirect() {
    this._userManager.clearStaleState();
    this._userManager.removeUser();
    return this._userManager.signoutRedirect({ 'id_token_hint': (await this._userManager.getUser())?.id_token });
  }
  
  signoutCallback() {
    this._userManager.clearStaleState();
    this._userManager.removeUser();
    return this._userManager.signoutCallback();
  }
  //#endregion
}
