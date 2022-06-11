import { IcartItemsAdd } from './../../Shared/Models/response';
import { DataService } from './../../Shared/Services/data.service';
import { ServerService } from 'src/app/Shared/Services/server.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  PartImages,
  Product,
  Response,
} from 'src/app/Shared/Models/response';

@Component({
  selector: 'app-view-product',
  templateUrl: './view-product.component.html',
  styleUrls: ['./view-product.component.css'],
})
export class ViewProductComponent implements OnInit {
  rPid: string = '';
  productData: Product = <Product>{};
  currentImage: PartImages = <PartImages>{};
  prtIndex: number = 0;

  constructor(
    private service: ServerService,
    private actRoute: ActivatedRoute,
    private route: Router,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.actRoute.queryParams.subscribe(
      (x: any) => {
        if (!x.q) {
          this.route.navigateByUrl('/');
        }
        this.rPid = atob(x.q);
        this.getPartDetails();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  changeImage(i: number) {
    this.currentImage = this.productData.Images[i];
    this.prtIndex = i;
  }

  getPartDetails() {
    this.service.GetPartbyID(this.rPid).subscribe(
      (x: Response) => {
        this.productData = x.Data;
        this.currentImage = this.productData.Images[0];
      },
      (err) => {
        console.log(err);
      }
    );
  }

  addItemstoCart() {
    //   this.dataService.cartData.push(cartItem);
    //   this.dataService.ShareCartItem();
    var cartAdd: IcartItemsAdd = <IcartItemsAdd>{};
    cartAdd.CIPrid = this.productData.ID;
    cartAdd.CIQty = 1;
    this.service.addCartItems(cartAdd).subscribe(
      (x: Response) => {
        console.log(x);
        this.dataService.getCartDetails();
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
