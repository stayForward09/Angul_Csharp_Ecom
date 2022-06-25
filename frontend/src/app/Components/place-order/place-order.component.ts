import { ServerService } from './../../Shared/Services/server.service';
import {
  ICart,
  IOrderReq,
  IUserDetail,
  Response,
} from './../../Shared/Models/response';
import { DataService } from './../../Shared/Services/data.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-place-order',
  templateUrl: './place-order.component.html',
  styleUrls: ['./place-order.component.css'],
})
export class PlaceOrderComponent implements OnInit {
  cartItems: Array<ICart> = [];
  userDetails: IUserDetail = <IUserDetail>{};
  isChanged: boolean = false;
  prevStateAddress: string = '';
  paymentType: number = 0;

  constructor(
    private route: Router,
    private dataService: DataService,
    private server: ServerService
  ) {
    this.dataService.cartItems.subscribe((x: Array<ICart>) => {
      this.cartItems = x;
      // if (this.cartItems.length <= 0) {
      //   this.route.navigateByUrl('/Cart');
      // }
    });
  }

  ngOnInit(): void {
    this.getUserDetails();
  }

  async getUserDetails() {
    this.dataService.setGlobalLoading(true);
    await this.promDetails().then((x: Response | undefined) => {
      if (x?.Data === null) {
        this.userDetails = <IUserDetail>{
          Address: '',
          CompanyCategory: '_',
          CompanyName: '_',
        };
      } else {
        this.userDetails = x?.Data as IUserDetail;
      }
      this.prevStateAddress = this.userDetails.Address;
    });
    this.dataService.setGlobalLoading(false);
  }

  promDetails() {
    return this.server.getUserAddressDetails().toPromise();
  }

  async saveAddress() {
    this.dataService.setGlobalLoading(true);
    await this.server
      .saveUserDetails(this.userDetails)
      .toPromise()
      .then((x: Response | undefined) => {
        if (x?.Succeeded) {
          // move next area
        }
      })
      .catch((err) => {
        console.log(err);
      });
    this.dataService.setGlobalLoading(false);
  }

  addressKeypress(e: any) {
    this.isChanged = true;
  }

  async PlaceOrder() {
    var OrderReq: Array<IOrderReq> = [];
    var order: IOrderReq = <IOrderReq>{};
    this.cartItems.forEach((x) => {
      let dis = x?.Disount?.Did ?? null;
      order = <IOrderReq>{
        Cid: x.Cid,
        DId: dis,
        prdId: x.Pid,
        Qty: x.Qty,
      };
      OrderReq.push(order);
    });
    console.log(OrderReq);
    this.dataService.setGlobalLoading(true);
    await this.server
      .createOrder(this.paymentType, OrderReq)
      .toPromise()
      .then(
        (x: Response | undefined) => {
          let url = x?.Data.short_url;
          console.log(url);
          location.href = url;
        },
        (err) => {
          console.log(err);
        }
      );
    this.dataService.setGlobalLoading(false);
  }
}
