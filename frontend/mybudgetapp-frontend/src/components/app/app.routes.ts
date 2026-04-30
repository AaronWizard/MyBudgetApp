import { Routes } from '@angular/router';
import { Home } from '../home/home';
import { Register } from '../register/register';
import { Login } from '../login/login';
import { VerifyRegistration } from '../verify-registration/verify-registration';

export const routes: Routes = [
  {
    path: '',
    component: Home,
  },
  {
    path: 'login',
    component: Login,
  },
  {
    path: 'register/verify/:token',
    component: VerifyRegistration,
  },
  {
    path: 'register',
    component: Register,
  },
  {
    path: '**',
    redirectTo: '',
  },
];
