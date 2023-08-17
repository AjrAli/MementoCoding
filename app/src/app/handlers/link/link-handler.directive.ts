import { Directive, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';

@Directive({
  selector: '[linkHandler]'
})
export class LinkHandlerDirective {
  @Input() linkHandler: string = '';

  constructor(private router: Router) {}

  @HostListener('click', ['$event'])
  onClick(event: Event): void {
    event.preventDefault();

    const [urlWithoutParams, queryParams] = this.linkHandler.split('?');

    if (queryParams) {
      const paramsObj: { [key: string]: string } = {};
      const paramsArray = queryParams.split('&');

      paramsArray.forEach(param => {
        const [key, value] = param.split('=');
        paramsObj[key] = value;
      });

      this.router.navigate([urlWithoutParams], { queryParams: paramsObj });
    } else {
      this.router.navigate([this.linkHandler]);
    }
  }
}
