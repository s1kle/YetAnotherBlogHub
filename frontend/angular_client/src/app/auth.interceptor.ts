import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CoolLocalStorage } from '@angular-cool/storage';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const coolLocalStorage = inject(CoolLocalStorage);
  const access_token = coolLocalStorage.getItem('access_token');
  
  if (!access_token) return next(req);

  const authorization_token = `Bearer ${access_token}`;
  const authReq = req.clone({ setHeaders: { Authorization: authorization_token }});

  return next(authReq);
};
