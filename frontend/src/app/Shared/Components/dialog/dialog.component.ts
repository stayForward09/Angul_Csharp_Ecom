import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  HostListener,
  Input,
} from '@angular/core';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css'],
})
export class DialogComponent implements OnInit {
  @ViewChild('dialog', { static: false }) dialogRef!: ElementRef;
  @Input('modelBackdrop') modelBackdrop: boolean = false;

  constructor() {}

  Open() {
    this.dialogRef?.nativeElement?.showModal();
  }

  Close() {
    this.dialogRef.nativeElement?.close();
  }

  @HostListener('document:keydown', ['$event'])
  onKeyDownHandler(event: KeyboardEvent) {
    if (event.key === 'Escape' && this.modelBackdrop) {
      event.preventDefault();
    }
  }

  @HostListener('document:click', ['$event'])
  clickOutside(event: any) {
    if (event.target.nodeName === 'DIALOG' && !this.modelBackdrop) {
      this.Close();
    }
  }

  ngOnInit(): void {}
}
