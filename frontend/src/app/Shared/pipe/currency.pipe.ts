import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currency',
})
export class CurrencyPipe implements PipeTransform {
  CurrencyFormat = new Intl.NumberFormat(undefined, {
    style: 'currency',
    currency: 'INR',
  });

  transform(value: number, format: string | null = null): string {
    if (format === null) {
      return this.CurrencyFormat.format(value);
    } else {
      return new Intl.NumberFormat(undefined, {
        style: 'currency',
        currency: format,
      }).format(value);
    }
  }
}
