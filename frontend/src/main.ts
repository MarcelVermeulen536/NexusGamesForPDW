import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

// Point d'entrée : on démarre l'application standalone (cours Angular : bootstrapApplication, plus de AppModule)
bootstrapApplication(AppComponent, appConfig).catch((err) => console.error(err));
