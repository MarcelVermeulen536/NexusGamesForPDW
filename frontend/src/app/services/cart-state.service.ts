import { Injectable, computed, signal } from '@angular/core';
import { CartItem } from './api/models/cart-item.model';

// Gestion de l'état du panier via un service singleton + signals
// (cours Angular : exemple du panier d'achats avec signal/computed, slide « PanierService »).
@Injectable({ providedIn: 'root' })
export class CartStateService {
  // état mutable : la liste des articles du panier
  private readonly _items = signal<CartItem[]>([]);

  // exposés en lecture seule + valeurs dérivées (computed se recalcule automatiquement)
  readonly items = this._items.asReadonly();
  readonly count = computed(() => this._items().length);
  readonly total = computed(() => this._items().reduce((somme, article) => somme + article.prix, 0));

  // Ajoute un jeu au panier (un même jeu ne peut pas être ajouté deux fois)
  add(item: CartItem): void {
    if (this._items().some((i) => i.gameId === item.gameId)) {
      return;
    }
    // update() construit une nouvelle liste à partir de l'actuelle (immutabilité)
    this._items.update((list) => [...list, item]);
  }

  remove(gameId: number): void {
    this._items.update((list) => list.filter((i) => i.gameId !== gameId));
  }

  clear(): void {
    this._items.set([]);
  }

  // Indique si un jeu est déjà dans le panier (utile pour désactiver le bouton « Ajouter »)
  has(gameId: number): boolean {
    return this._items().some((i) => i.gameId === gameId);
  }
}
