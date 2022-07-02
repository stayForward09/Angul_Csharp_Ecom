import { ServerService } from 'src/app/Shared/Services/server.service';
import { Component, OnInit } from '@angular/core';
import { IOrderdetails, Response } from 'src/app/Shared/Models/response';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
export class AccountComponent implements OnInit {
  displayTab: number = 0;
  orderDetails: Array<IOrderdetails> = [];

  constructor(private service: ServerService) {}

  ngOnInit(): void {
    this.getOrders();
  }

  SwitchTabs(e: MouseEvent, id: string) {
    let eve = e.target as HTMLButtonElement;
    var ele = document.getElementsByClassName('btnnav');

    for (var i = 0; i < ele.length; i++) {
      ele[i].classList.remove('btnnav-active');
    }

    ele = document.getElementsByClassName('tabContent-item');

    for (var i = 0; i < ele.length; i++) {
      let d = ele[i] as HTMLElement;
      d.style.display = 'none';
    }
    var cD = document.getElementById(id);
    if (cD != null) {
      cD.style.display = 'flex';
    }
    eve.classList.add('btnnav-active');
  }

  tabClick(indx: number) {
    this.displayTab = indx;
  }

  getOrders() {
    this.service.getOrders().subscribe(
      (x: Response) => {
        this.orderDetails = x.Data as Array<IOrderdetails>;
        console.log(this.orderDetails);
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
