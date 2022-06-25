import { DataService } from './../../Shared/Services/data.service';
import { ServerService } from 'src/app/Shared/Services/server.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { IRazorpayRes, Response } from 'src/app/Shared/Models/response';

@Component({
  selector: 'app-verify-payment',
  templateUrl: './verify-payment.component.html',
  styleUrls: ['./verify-payment.component.css'],
})
export class VerifyPaymentComponent implements OnInit {
  paymentStatus: IRazorpayRes = <IRazorpayRes>{};
  succed: boolean = false;

  constructor(
    private route: Router,
    private actRoute: ActivatedRoute,
    private server: ServerService,
    private dataServer: DataService
  ) {}

  ngOnInit(): void {
    this.actRoute.queryParams.subscribe(
      (x: any) => {
        this.paymentStatus = x as IRazorpayRes;
        console.log(this.paymentStatus);
        this.updatePayment();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  async updatePayment() {
    this.dataServer.setGlobalLoading(true);
    await this.server
      .verifyPayment(
        this.paymentStatus.razorpay_payment_id,
        this.paymentStatus.razorpay_payment_link_id
      )
      .toPromise()
      .then((x: Response | undefined) => {
        this.succed = x?.Data.status === 'captured' ? true : false;
      })
      .catch((err) => {
        var _r = err.error as Response;
        this.succed = _r.Message === 'Order Already Placed' ? true : false;
        console.log(this.succed);
      });
    this.dataServer.setGlobalLoading(false);
  }

  goBack() {
    this.route.navigate(['/']);
  }
}
