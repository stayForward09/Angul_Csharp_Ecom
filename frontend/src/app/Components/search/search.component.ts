import { DataService } from './../../Shared/Services/data.service';
import { ServerService } from './../../Shared/Services/server.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Response } from 'src/app/Shared/Models/response';
import { AppComponent } from 'src/app/app.component';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css'],
})
export class SearchComponent implements OnInit {
  rSrch: string = '';
  srchResult: any[] = [];
  LoadingData: boolean = false;
  page: number = 1;
  pagesize: number = 20;
  totalRecords: number = 0;

  constructor(
    private actRoute: ActivatedRoute,
    private server: ServerService,
    private router: Router,
    private dataService: DataService
  ) {}

  ngOnInit(): void {
    this.actRoute.queryParams.subscribe(
      (x: any) => {
        if (!x.q) {
          this.router.navigateByUrl('/');
        }
        this.rSrch = x.q;
        this.dataService.ShareTitle(this.rSrch);
        this.GetSrchResult();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  GetSrchResult() {
    this.LoadingData = true;
    this.server.getPartResult(this.rSrch, this.page, this.pagesize).subscribe(
      (x: Response) => {
        this.srchResult = x.Data.data;
        this.totalRecords = x.Data.total;
        this.LoadingData = false;
      },
      (err) => {
        this.LoadingData = false;
        console.log(err);
      }
    );
  }

  viewPart(data: any) {
    this.router.navigate(['/View'], {
      queryParams: { q: btoa(data.Id) },
    });
  }

  pagingEvent(pageNum: number) {
    this.page = pageNum;
    this.GetSrchResult();
  }
  calculateDiscount(Discount: any, actualPrice: number) {
    if (Discount != null) {
      let { Amount, type, ...rest } = Discount;
      if (type === 1) {
        return actualPrice - Amount;
      } else {
        let per = (Amount / 100) * actualPrice;
        return actualPrice - per;
      }
    }
    return actualPrice;
  }
}
