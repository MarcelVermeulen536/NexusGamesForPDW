// Un jeu possédé dans la bibliothèque de l'utilisateur (reçu de l'API).
export interface LibraryItem {
  gameId: number;
  titre: string;
  dateAcquisition?: string;
  tempsJeuHeures: number;
  completionPourcentage: number;
}
