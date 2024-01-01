import { ApplicationConfig } from '@angular/core';
import { provideRouter, withRouterConfig } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { authInterceptor } from './auth.interceptor';
import { CoolLocalStorage } from '@angular-cool/storage';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes), 
    CoolLocalStorage,
    provideClientHydration(), 
    provideHttpClient(withFetch(), withInterceptors([authInterceptor]))
  ]
};