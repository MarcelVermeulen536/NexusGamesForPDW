import { CartItem } from './cart-item.model';

// Panier envoyé à l'API lors de l'achat (checkout).
export interface Cart {
  items: CartItem[];
  methodePaiement: string;
}
