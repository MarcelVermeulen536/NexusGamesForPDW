// Un article du panier côté client (référence un jeu + infos d'affichage).
export interface CartItem {
  gameId: number;
  titre: string;
  prix: number;
}
