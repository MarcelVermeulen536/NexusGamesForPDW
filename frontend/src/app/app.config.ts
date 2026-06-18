import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';

// Configuration de l'application (cours Angular : configuration via providers, plus de NgModule)
export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    // Routage de la SPA + liaison des paramètres de route comme input() des composants
    // (cours Angular : routage / withComponentInputBinding)
    provideRouter(routes, withComponentInputBinding()),
    // Fournit HttpClient pour communiquer avec l'API .NET (cours Angular : communication serveur via HttpClient)
    provideHttpClient(),
  ],
};
