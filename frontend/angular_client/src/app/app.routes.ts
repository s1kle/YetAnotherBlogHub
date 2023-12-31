import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { SigninCallbackComponent } from './signin-callback/signin-callback.component';
import { SignoutCallbackComponent } from './signout-callback/signout-callback.component';
import { BlogComponent } from './blog/blog.component';
import { CreateBlogComponent } from './create-blog/create-blog.component';

export const routes: Routes = [
    {path: 'signin-callback', component: SigninCallbackComponent},
    {path: 'signout-callback', component: SignoutCallbackComponent},
    {path: 'blog/create', component: CreateBlogComponent},
    {path: 'blog/:id', component: BlogComponent},
    {path: '**', component: HomeComponent }
];
