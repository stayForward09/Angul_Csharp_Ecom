import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[appRipple]',
})
export class RippleDirective {
  constructor(private element: ElementRef) {}

  @HostListener('click', ['$event']) 
  onClick(e: PointerEvent) {
    this.makeRipple(e);
  }

  makeRipple(event: PointerEvent) {
    this.element.nativeElement.style.position = 'relative';
    const x = event.pageX - this.element.nativeElement.offsetLeft;
    const y = event.pageY - this.element.nativeElement.offsetTop;
    const w = this.element.nativeElement.offsetWidth;
    let span = document.createElement('span');
    span.className = 'ripple';
    span.style.left = x + 'px';
    span.style.top = y + 'px';
    span.style.setProperty('--scale',w);
    this.element.nativeElement.appendChild(span);
    setTimeout(() => {
      span.parentNode?.removeChild(span);
      this.element.nativeElement.style.position = '';
    },500);
  }
}
