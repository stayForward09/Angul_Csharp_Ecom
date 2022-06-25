import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
export class AccountComponent implements OnInit {
  displayTab: number = 0;

  constructor() {}

  ngOnInit(): void {}

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
}
