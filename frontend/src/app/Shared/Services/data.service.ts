import { ILoginUser, ICartItems } from './../Models/response';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  title = new BehaviorSubject<string>('');
  isLogged = new BehaviorSubject<boolean>(false);
  loggedUser = new BehaviorSubject<ILoginUser>(<ILoginUser>{});
  cartItems = new BehaviorSubject<Array<ICartItems>>([]);
  cartData: Array<ICartItems>=[];

  constructor() {}

  ShareTitle(tileC: string) {
    this.title.next(tileC);
  }
  ShareisLogged(status: boolean) {
    this.isLogged.next(status);
  }

  ShareLoginUser(data: ILoginUser) {
    this.loggedUser.next(data);
  }
  ShareCartItem() {
    this.cartItems.next(this.cartData);
  }
}
