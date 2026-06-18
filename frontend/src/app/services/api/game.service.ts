import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Game } from './models/game.model';

// Service dédié à la communication avec l'API pour les jeux
// (cours Angular : service injectable + HttpClient pour parler au serveur).
@Injectable({ providedIn: 'root' })
export class GameService {
  // inject() remplace l'injection par constructeur (cours Angular : inject)
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/games`;

  // GET de tous les jeux du catalogue
  getAll(): Observable<Game[]> {
    return this.http.get<Game[]>(this.baseUrl);
  }

  // GET d'un jeu par son identifiant
  getById(id: number): Observable<Game> {
    return this.http.get<Game>(`${this.baseUrl}/${id}`);
  }
}
