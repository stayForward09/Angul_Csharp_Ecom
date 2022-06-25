import { DataService } from './../../Shared/Services/data.service';
import { ServerService } from 'src/app/Shared/Services/server.service';
import {
  ICart,
  IcartItemsAdd,
  Response,
  IcartDiscount,
} from './../../Shared/Models/response';
import { Observable, empty } from 'rxjs';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  $CartItem: Observable<Array<ICart>> = empty();
  @Input() showCartItems:boolean = true;

  constructor(private dataServer: DataService, private server: ServerService) {
    this.$CartItem = this.dataServer.cartItems;
  }

  ngOnInit(): void {}

  incrementValue(value: string, Cid: string) {
    var item = this.dataServer.cartData[this.getIndex(Cid)];
    item.Qty = item.Qty + 1;
    var data: IcartItemsAdd = <IcartItemsAdd>{
      CIPrid: item.Pid,
      CIQty: item.Qty,
      CITId: item.Cid,
    };
    this.server.patchCartItems(data).subscribe(
      (x) => {
        console.log(x);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  decrementValue(value: string, Cid: string) {
    var item = this.dataServer.cartData[this.getIndex(Cid)];
    item.Qty = item.Qty - 1;
    var data: IcartItemsAdd = <IcartItemsAdd>{
      CIPrid: item.Pid,
      CIQty: item.Qty,
      CITId: item.Cid,
    };
    this.server.patchCartItems(data).subscribe(
      (x) => {
        // console.log(x);
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getIndex(id: string): number {
    return this.dataServer.cartData.findIndex((x) => x.Cid === id);
  }

  removeItem(id: string) {
    this.server.deleteCartItem(id).subscribe(
      (res: Response) => {
        this.dataServer.getCartDetails();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getPrice(actualPrice: number, discount: IcartDiscount | null) {
    if (discount !== null) {
      if (discount.type === 2) {
        let per = (discount.Price / 100) * actualPrice;
        return actualPrice - per;
      } else {
        return actualPrice - discount.Price;
      }
    }
    return actualPrice;
  }

  getTotalPriceWithouDiscount() {
    var total_ = this.dataServer.cartData.reduce(
      (prev, next) => prev + next.Price * next.Qty,
      0
    );
    return total_;
  }

  hasDiscount() {
    var hasDiscount = this.dataServer.cartData.filter((x) => {
      return x.Disount !== null;
    });
    return hasDiscount.length > 0;
  }

  getDiscountValue() {
    var discountData = this.dataServer.cartData.filter((x) => {
      return x.Disount !== null;
    });
    var total_ = discountData.reduce((prev, next) => {
      var disPrice = this.getDiscountPrice(next.Price, next.Disount);
      return prev + disPrice * next.Qty;
    }, 0);
    return total_;
  }

  getTotalPrice() {
    var totalprice = this.getTotalPriceWithouDiscount();
    var discountPrice = 0;
    if (this.hasDiscount()) {
      discountPrice = this.getDiscountValue();
    }
    return totalprice - discountPrice;
  }

  getDiscountPrice(actualPrice: number, discount: IcartDiscount) {
    if (discount.type === 2) {
      let per = (discount.Price / 100) * actualPrice;
      return per;
    } else {
      return discount.Price;
    }
  }
}
