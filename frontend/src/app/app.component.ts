import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

// Composant racine standalone (cours Angular : composants standalone par défaut depuis Angular 17+).
// Il déclare lui-même ses dépendances dans `imports` — plus de NgModule.
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {}
