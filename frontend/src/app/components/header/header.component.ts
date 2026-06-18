import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthStateService } from '../../services/auth-state.service';
import { CartStateService } from '../../services/cart-state.service';

// En-tête de navigation, partagé par toute l'application.
// Il lit directement les services d'état (singleton + signals) : aucune communication
// directe avec les autres composants n'est nécessaire (cours Angular : état partagé via service).
@Component({
  selector: 'app-header',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent {
  readonly authState = inject(AuthStateService);
  readonly cartState = inject(CartStateService);
  private readonly router = inject(Router);

  deconnexion(): void {
    this.authState.logout();
    this.router.navigate(['/']);
  }
}
