import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SigninCallbackComponent } from './signin-callback/signin-callback.component';
import { SignoutCallbackComponent } from './signout-callback/signout-callback.component';

export const routes: Routes = [
    {path: 'signin-callback', component: SigninCallbackComponent},
    {path: 'signout-callback', component: SignoutCallbackComponent},
    {path: '**', component: HomeComponent}
];
