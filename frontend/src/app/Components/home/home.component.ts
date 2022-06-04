import { Router } from '@angular/router';
import { DataService } from './../../Shared/Services/data.service';
import { AppComponent } from './../../app.component';
import { Component, OnInit } from '@angular/core';
import { ServerService } from 'src/app/Shared/Services/server.service';
import { ICartItems, Response } from 'src/app/Shared/Models/response';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  data: any = [];
  latestSrch: any[] = [];
  LoadingData: boolean = false;
  isError = false;
  $login!: Observable<boolean>;

  constructor(
    private server: ServerService,
    private dataService: DataService,
    private route: Router
  ) {
    this.isError = true;
    this.$login = this.dataService.isLogged.asObservable();
    this.$login.subscribe(
      (x: boolean) => {
        if (x) {
          this.GetData();
        }
      },
      (err) => {
        console.log(err);
      }
    );
  }

  ngOnInit(): void {}

  GetData() {
    this.LoadingData = true;
    this.server.getHomeDetails().subscribe(
      (x: Response) => {
        const { lstVistited, srchhis } = x.Data;
        this.data = lstVistited;
        this.latestSrch = srchhis;
        this.isError = false;
        this.LoadingData = false;
      },
      (err) => {
        this.isError = true;
        console.log(err);
      }
    );
  }

  viewPart(data: any) {
    this.route.navigate(['/View'], {
      queryParams: { q: btoa(data.ID) },
    });
  }
}
