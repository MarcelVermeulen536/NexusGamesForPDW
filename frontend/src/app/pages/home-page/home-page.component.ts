import { Component, OnInit, inject, signal } from '@angular/core';
import { GameService } from '../../services/api/game.service';
import { CartStateService } from '../../services/cart-state.service';
import { Game } from '../../services/api/models/game.model';

// Page d'accueil : affiche le catalogue de jeux récupéré depuis l'API.
@Component({
  selector: 'app-home-page',
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
})
export class HomePageComponent implements OnInit {
  private readonly gameService = inject(GameService);
  private readonly cartState = inject(CartStateService);

  // État local du composant via signals (cours Angular : signals)
  readonly games = signal<Game[]>([]);
  readonly isLoading = signal(true);
  readonly hasError = signal(false);

  ngOnInit(): void {
    // Appel HTTP via le service dédié, puis mise à jour des signals
    // (cours Angular : HttpClient + subscribe avec gestion du chargement et de l'erreur)
    this.gameService.getAll().subscribe({
      next: (games) => {
        this.games.set(games);
        this.isLoading.set(false);
      },
      error: () => {
        this.hasError.set(true);
        this.isLoading.set(false);
      },
    });
  }

  // Prix effectif : le prix réduit s'il existe, sinon le prix normal
  prixEffectif(game: Game): number {
    return game.prixReduit ?? game.prix;
  }

  ajouterAuPanier(game: Game): void {
    this.cartState.add({ gameId: game.id, titre: game.titre, prix: this.prixEffectif(game) });
  }

  estDansPanier(id: number): boolean {
    return this.cartState.has(id);
  }
}
