import {
  animate,
  keyframes,
  style,
  transition,
  trigger,
} from '@angular/animations';
import {
  Component,
  Input,
  OnInit,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';

@Component({
  selector: 'shop-cart-icon',
  templateUrl: './shop-cart-icon.component.html',
  styleUrls: ['./shop-cart-icon.component.css'],
})
export class ShopCartIconComponent implements OnInit, OnChanges {
  @Input('cartItemCount') cartItemCount: number = 0;
  @ViewChild('cartRef') cartRef!: any;

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    let prev = changes['cartItemCount'].previousValue;
    let curr = changes['cartItemCount'].currentValue;
    const Timing = {
      duration: 1000,
      iterations: 1,
    };
    const pulse = [
      {
        transform: 'scale(0.95)',
        boxShadow: '0 0 0 0 red',
        fill: 'red',
      },
      {
        transform: 'scale(1)',
        boxShadow: '0 0 0 10px red',
        fill: 'red',
      },
      {
        transform: 'scale(0.95)',
        boxShadow: '0 0 0 0 red',
        fill: 'red',
      },
    ];

    if (curr > prev) {
      this.cartRef.nativeElement.animate(pulse, Timing);
    }
  }

  ngOnInit(): void {}
}
