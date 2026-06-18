import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Cart } from './models/cart.model';
import { LibraryItem } from './models/library-item.model';

// Service dédié aux appels d'API d'achat et de bibliothèque.
@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/cart`;

  // POST du panier pour l'achat (auth simplifiée : on passe l'id utilisateur dans l'URL)
  checkout(userId: number, cart: Cart): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.baseUrl}/checkout/${userId}`, cart);
  }

  // GET de la bibliothèque (jeux possédés) d'un utilisateur
  getLibrary(userId: number): Observable<LibraryItem[]> {
    return this.http.get<LibraryItem[]>(`${this.baseUrl}/library/${userId}`);
  }
}
