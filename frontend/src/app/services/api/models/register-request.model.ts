// Données envoyées à l'API pour créer un compte.
export interface RegisterRequest {
  pseudo: string;
  email: string;
  motDePasse: string;
  nom?: string;
  prenom?: string;
}
