import { Injectable, computed, effect, signal } from '@angular/core';
import { User } from './api/models/user.model';

// Gestion de l'état d'authentification via un service singleton + signals
// (cours Angular : gestion de l'état avec un service singleton — PAS de NgRx).
// Comme providedIn: 'root', ce service est unique pour toute l'application :
// tous les composants qui l'injectent partagent le même utilisateur connecté.
@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly storageKey = 'nexus_user';

  // signal privé mutable contenant l'utilisateur courant (ou null si déconnecté)
  private readonly _currentUser = signal<User | null>(this.loadFromStorage());

  // exposé en lecture seule à l'extérieur (cours : .asReadonly())
  readonly currentUser = this._currentUser.asReadonly();

  // signal calculé : vrai si un utilisateur est connecté (cours : computed())
  readonly isLoggedIn = computed(() => this._currentUser() !== null);

  constructor() {
    // effet de bord : on synchronise le localStorage à chaque changement de l'utilisateur
    // (cours : effect() pour réagir aux changements, ex. synchronisation localStorage)
    effect(() => {
      const user = this._currentUser();
      if (user) {
        localStorage.setItem(this.storageKey, JSON.stringify(user));
      } else {
        localStorage.removeItem(this.storageKey);
      }
    });
  }

  login(user: User): void {
    this._currentUser.set(user);
  }

  logout(): void {
    this._currentUser.set(null);
  }

  // Recharge l'utilisateur depuis le localStorage (pour rester connecté après un rafraîchissement)
  private loadFromStorage(): User | null {
    const raw = localStorage.getItem(this.storageKey);
    return raw ? (JSON.parse(raw) as User) : null;
  }
}
