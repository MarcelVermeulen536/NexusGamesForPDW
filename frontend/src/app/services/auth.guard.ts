import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStateService } from './auth-state.service';

// Garde de route fonctionnelle (cours Angular : gardes fonctionnelles avec inject()).
// Empêche d'accéder aux pages protégées (panier, bibliothèque) si l'utilisateur n'est pas connecté.
export const authGuard: CanActivateFn = () => {
  const authState = inject(AuthStateService);
  const router = inject(Router);

  if (authState.isLoggedIn()) {
    return true;
  }

  // Sinon, on redirige vers la page de connexion
  return router.createUrlTree(['/login']);
};
