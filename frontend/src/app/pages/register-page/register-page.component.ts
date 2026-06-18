import { Component, inject, signal } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/api/auth.service';
import { AuthStateService } from '../../services/auth-state.service';

// Page d'inscription utilisant un formulaire réactif avec validations (cours Angular : Reactive Forms).
@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.css',
})
export class RegisterPageComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly authState = inject(AuthStateService);
  private readonly router = inject(Router);

  readonly erreur = signal<string | null>(null);

  readonly registerForm = this.fb.group({
    pseudo: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    motDePasse: ['', [Validators.required, Validators.minLength(6)]],
    nom: [''],
    prenom: [''],
  });

  onSubmit(): void {
    this.erreur.set(null);

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const v = this.registerForm.value;
    this.authService
      .register({
        pseudo: v.pseudo!,
        email: v.email!,
        motDePasse: v.motDePasse!,
        nom: v.nom ?? undefined,
        prenom: v.prenom ?? undefined,
      })
      .subscribe({
        next: (user) => {
          // Inscription réussie : on connecte directement l'utilisateur et on va à l'accueil
          this.authState.login(user);
          this.router.navigate(['/']);
        },
        // L'API renvoie un message d'erreur (ex. 409 si l'email/pseudo existe déjà)
        error: (err) => this.erreur.set(err?.error?.error ?? "Inscription impossible."),
      });
  }
}
