import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
  Output,
  EventEmitter,
} from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css'],
})
export class PaginationComponent implements OnInit, OnChanges {
  @Input('pageSize') pageSize!: number;
  @Input('totalRecords') totalRecords!: number;
  @Input('pageNumber') pageNumber!: number;
  @Output('changePage') changePage: EventEmitter<number> = new EventEmitter<number>();
  pages: Array<any> = [];

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    this.calculatePagesize();
  }

  ngOnInit(): void {
    this.calculatePagesize();
  }

  calculatePagesize() {
    if (this.totalRecords > 0 && this.pageSize > 0) {
      this.pages = new Array(Math.ceil(this.totalRecords / this.pageSize));
    }
  }

  clickEvent(pagenum:number) {
    this.changePage.emit(pagenum);
  }
}
