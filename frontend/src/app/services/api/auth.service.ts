import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { User } from './models/user.model';
import { AuthenticationRequest } from './models/authentication-request.model';
import { RegisterRequest } from './models/register-request.model';

// Service dédié aux appels d'API d'authentification (connexion / inscription).
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/users`;

  // POST des identifiants ; l'API renvoie l'utilisateur si la connexion réussit
  login(request: AuthenticationRequest): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/login`, request);
  }

  // POST des informations d'inscription ; l'API renvoie l'utilisateur créé
  register(request: RegisterRequest): Observable<User> {
    return this.http.post<User>(`${this.baseUrl}/register`, request);
  }
}
