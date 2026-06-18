// Modèle d'un utilisateur reçu de l'API (sans le mot de passe).
export interface User {
  id: number;
  pseudo: string;
  email: string;
  nom?: string;
  prenom?: string;
  dateInscription: string;
  soldeCompte: number;
  statut: string;
}
