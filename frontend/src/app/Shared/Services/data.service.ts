import { ServerService } from 'src/app/Shared/Services/server.service';
import { ILoginUser, ICart, Response } from './../Models/response';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  title = new BehaviorSubject<string>('');
  isLogged = new BehaviorSubject<boolean>(false);
  loggedUser = new BehaviorSubject<ILoginUser>(<ILoginUser>{});
  cartItems = new BehaviorSubject<Array<ICart>>([]);
  cartData: Array<ICart> = [];

  constructor(private server: ServerService) {}

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
    console.log('Emiting Changes..');
    this.cartItems.next(this.cartData);
  }

  getCartDetails() {
    this.server.getCartItems().subscribe(
      (x: Response) => {
        var d = x.Data as Array<ICart>;
        this.cartData = d;
        this.ShareCartItem();
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
