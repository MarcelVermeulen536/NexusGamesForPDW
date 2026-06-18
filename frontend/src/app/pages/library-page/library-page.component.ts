import { Component, OnInit, inject, signal } from '@angular/core';
import { CartService } from '../../services/api/cart.service';
import { AuthStateService } from '../../services/auth-state.service';
import { LibraryItem } from '../../services/api/models/library-item.model';

// Page « Ma bibliothèque » : liste les jeux possédés par l'utilisateur connecté.
@Component({
  selector: 'app-library-page',
  imports: [],
  templateUrl: './library-page.component.html',
  styleUrl: './library-page.component.css',
})
export class LibraryPageComponent implements OnInit {
  private readonly cartService = inject(CartService);
  private readonly authState = inject(AuthStateService);

  readonly items = signal<LibraryItem[]>([]);
  readonly isLoading = signal(true);
  readonly hasError = signal(false);

  ngOnInit(): void {
    const user = this.authState.currentUser();
    if (!user) {
      this.isLoading.set(false);
      return;
    }

    // Appel HTTP de la bibliothèque de l'utilisateur (cours Angular : HttpClient + subscribe)
    this.cartService.getLibrary(user.id).subscribe({
      next: (items) => {
        this.items.set(items);
        this.isLoading.set(false);
      },
      error: () => {
        this.hasError.set(true);
        this.isLoading.set(false);
      },
    });
  }
}
