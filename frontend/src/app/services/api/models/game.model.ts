// Modèle d'un jeu reçu de l'API (cours TypeScript : interface décrivant la structure d'un objet).
export interface Game {
  id: number;
  titre: string;
  description?: string;
  prix: number;
  dateSortie?: string;
  scoreMetascore?: number;
  developpeur: string;
  plateforme: string;
  genres: string[];
  prixReduit?: number | null;
}
