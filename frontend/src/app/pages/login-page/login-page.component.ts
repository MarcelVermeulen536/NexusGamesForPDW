import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/api/auth.service';
import { AuthStateService } from '../../services/auth-state.service';

// Page de connexion utilisant un formulaire réactif (cours Angular : Reactive Forms).
@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css',
})
export class LoginPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);

  // Message d'erreur d'authentification (signal pour réactivité)
  readonly erreur = signal<string | null>(null);

  // Définition du formulaire réactif avec validateurs (cours : FormBuilder + Validators)
  readonly loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    motDePasse: ['', [Validators.required]],
  });

  onSubmit(): void {
    this.erreur.set(null);

    // Si le formulaire est invalide, on marque les champs comme « touchés » pour afficher les erreurs
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const { email, motDePasse } = this.loginForm.value;
    this.authService.login({ email: email!, motDePasse: motDePasse! }).subscribe({
      next: (user) => {
        // On enregistre l'utilisateur dans le service d'état puis on redirige (navigation programmatique)
        this.authState.login(user);
        this.router.navigate(['/']);
      },
      error: () => this.erreur.set('Email ou mot de passe incorrect.'),
    });
  }
}
