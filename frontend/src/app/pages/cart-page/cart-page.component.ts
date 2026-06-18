import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, Router } from '@angular/router';
import { CartStateService } from '../../services/cart-state.service';
import { CartService } from '../../services/api/cart.service';
import { AuthStateService } from '../../services/auth-state.service';
import { Cart } from '../../services/api/models/cart.model';

// Page du panier : lit l'état du panier (service + signals) et déclenche l'achat.
@Component({
  selector: 'app-cart-page',
  imports: [FormsModule, RouterLink],
  templateUrl: './cart-page.component.html',
  styleUrl: './cart-page.component.css',
})
export class CartPageComponent {
  // Service d'état du panier exposé au template (signals : items(), total(), count())
  readonly cartState = inject(CartStateService);
  private readonly cartService = inject(CartService);
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);

  // Lié au <select> avec [(ngModel)] (cours Angular : two-way binding)
  methodePaiement = 'carte';
  readonly erreur = signal<string | null>(null);

  retirer(gameId: number): void {
    this.cartState.remove(gameId);
  }

  acheter(): void {
    this.erreur.set(null);
    const user = this.authState.currentUser();
    if (!user) {
      return; // le guard garantit normalement qu'on est connecté
    }

    const cart: Cart = {
      items: this.cartState.items(),
      methodePaiement: this.methodePaiement,
    };

    this.cartService.checkout(user.id, cart).subscribe({
      next: () => {
        // Achat réussi : on vide le panier et on redirige vers la bibliothèque
        this.cartState.clear();
        this.router.navigate(['/library']);
      },
      error: (err) => this.erreur.set(err?.error?.error ?? "L'achat a échoué."),
    });
  }
}
