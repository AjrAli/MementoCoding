import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UrlHistoryService {
  private urlHistory: string[] = [];
  private titleUrlHistory: string[] = [];
  private bufferSize: number = 5;

  constructor(private router: Router) {
    this.init();
  }

  private init() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: any) => {
      if (event instanceof NavigationEnd) {
        this.addToHistory(event.url);
      }
    });
  }

  private addToHistory(url: string) {
    if(!this.urlHistory.find(x => x == url)){
      if (this.urlHistory.length >= this.bufferSize && this.titleUrlHistory.length >= this.bufferSize) {
        this.urlHistory.shift(); // Remove the oldest URL when buffer is full
        this.titleUrlHistory.shift();
      }
      this.urlHistory.push(url);
      this.titleUrlHistory.push(this.getTitleFromUrlRequest(url));
    }
  }
  getTitleFromUrlRequest(url: string): string {
    const [urlWithoutParams, queryParams] = url.split('?');
    const urlWithoutParamsWord = (urlWithoutParams.length > 1) ? urlWithoutParams.charAt(1).toUpperCase() + urlWithoutParams.slice(2) : 'Home';
    if (queryParams) {
      const paramsArray = queryParams.split('&');
      let paramsTitles = paramsArray.join(' and ');
      return `Page: ${urlWithoutParamsWord}, ${paramsTitles}`;
    }
    return `Page: ${urlWithoutParamsWord}`;
  }
  getVisitedUrls(): string[] {
    return this.urlHistory;
  }

  getTitleOfVisitedUrls(): string[] {
    return this.titleUrlHistory;
  }
}
