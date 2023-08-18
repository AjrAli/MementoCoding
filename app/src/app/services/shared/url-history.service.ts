import { Injectable } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UrlHistoryService {
  private urlHistory: { url: string; title: string }[] = [];
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
    if (!this.getTitleOfVisitedUrls()?.find(x => x == this.getTitleFromUrlRequest(url))) {
      const title = this.getTitleFromUrlRequest(url);
      const duplicateIndex = this.findDuplicateUrlIndex(url);

      if (duplicateIndex !== -1) {
        this.urlHistory.splice(duplicateIndex, 1);
      } else if (this.urlHistory.length >= this.bufferSize) {
        this.urlHistory.shift();
      }

      this.urlHistory.push({ url, title });
    }
  }

  getTitleFromUrlRequest(url: string): string {
    const [urlWithoutParams] = url.split('?');
    const urlWithoutParamsWord = urlWithoutParams.length > 1 ? urlWithoutParams.charAt(1).toUpperCase() + urlWithoutParams.slice(2) : 'Home';
    const queryParams = url.includes('?') ? url.split('?')[1] : '';

    return queryParams ? `Page: ${urlWithoutParamsWord}, ${queryParams.replace(/&/g, ' and ')}` : `Page: ${urlWithoutParamsWord}`;
  }

  findDuplicateUrlIndex(url: string): number {
    const [, queryParams] = url.split('?');

    if (queryParams && this.urlHistory.length > 0) {
      const paramsArray = queryParams.split('&');

      for (const [index, entry] of this.urlHistory.entries()) {
        const [, queryParamsFromRecordUrls] = entry.url.split('?');

        if (queryParamsFromRecordUrls) {
          const paramsArrayFromRecordUrls = queryParamsFromRecordUrls.split('&');

          const allParamKeysIncluded = paramsArrayFromRecordUrls.every(paramRecord => {
            const [keyRecord] = paramRecord.split('=');
            return paramsArray.some(param => {
              const [key] = param.split('=');
              return key === keyRecord;
            });
          });

          if (allParamKeysIncluded) {
            return index;
          }
        }
      }
    }

    return -1;
  }


  getVisitedUrls(): string[] {
    return this.urlHistory.map(entry => entry.url);
  }

  getTitleOfVisitedUrls(): string[] {
    return this.urlHistory.map(entry => entry.title);
  }
}
