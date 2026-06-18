import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { CartPageComponent } from './pages/cart-page/cart-page.component';
import { LibraryPageComponent } from './pages/library-page/library-page.component';
import { authGuard } from './services/auth.guard';

// Routes de l'application (cours Angular : routage).
export const routes: Routes = [
  { path: '', component: HomePageComponent },
  { path: 'login', component: LoginPageComponent },
  { path: 'register', component: RegisterPageComponent },
  // Routes protégées par le guard : accès réservé aux utilisateurs connectés
  { path: 'cart', component: CartPageComponent, canActivate: [authGuard] },
  { path: 'library', component: LibraryPageComponent, canActivate: [authGuard] },
  // Wildcard : toute URL inconnue redirige vers l'accueil (doit être en dernier)
  { path: '**', redirectTo: '' },
];
