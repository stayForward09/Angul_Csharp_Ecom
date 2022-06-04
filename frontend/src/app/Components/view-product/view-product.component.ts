import { DataService } from './../../Shared/Services/data.service';
import { ServerService } from 'src/app/Shared/Services/server.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  ICartItems,
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
        this.productData = x.Data[0];
        this.currentImage = this.productData.Images[0];
      },
      (err) => {
        console.log(err);
      }
    );
  }

  addItemstoCart() {
    const data = this.productData;
    let i = this.dataService.cartData.find((x) => x.ID == data.ID) ?? 0;
    let images: Array<string> = data.Images.filter((x) => {
      return (
        x.Url.toLocaleLowerCase().includes('jpeg') ||
        x.Url.toLocaleLowerCase().includes('.jpg')
      );
    }).map((dat) => dat.Url);
    let cartItem: ICartItems = {
      ID: data.ID,
      Description: data.Description,
      Images: images,
      Part: data.Part,
      Price: data.Price,
      Qty: 1,
    };
    if (i > -1) {
      this.dataService.cartData.push(cartItem);
      this.dataService.ShareCartItem();
    }
  }
}
