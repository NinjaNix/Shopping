import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Cart, CartItem } from '../../shared/models/cart';
import { Product } from '../../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  cart = signal<Cart | null>(null);

  getCart(id: string){
    return this.http.get<Cart>(this.baseUrl + 'cart?id=' + id).subscribe({
      next: (cart) => this.cart.set(cart),
      error: (error) => {
        console.error('Error fetching cart:', error);
        this.cart.set(null);
      }
    })
  }

  setCart(cart: Cart) {
    return this.http.post<Cart>(this.baseUrl + 'cart', cart).subscribe({
      next: (cart) => this.cart.set(cart),
      error: (error) => {
        console.error('Error setting cart:', error);
      }
    });
  }

  //Adding item to cart
  addItemToCart(item: CartItem | Product, quantity = 1){
    const cart = this.cart() ?? this.createCart();
    if(this.isProduct(item)){
      item = this.mapProductToCartItem(item);
    }
    cart.items = this.addOrUpdateCartItem(cart.items, item, quantity);
    this.setCart(cart);
  }

  private addOrUpdateCartItem(items: CartItem[], item: CartItem, quantity: number)
    : CartItem[] {
    const index = items.findIndex(i => i.productId === item.productId); 
  }

  private mapProductToCartItem(item: Product): CartItem {
    return{
      productId: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.brand,
      type: item.type
    }
  }

  //Check item is a Product and not CartItem
  private isProduct(item: CartItem | Product): item is Product {
    return (item as Product).id !== undefined;
  }
  //Create a new cart if it doesn't exist
  private createCart(): Cart {
    const cart = new Cart();
    localStorage.setItem('cart_id', cart.id);
    return cart;
  }
}
