import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchStateService {
  private searchKeywordSource = new BehaviorSubject<string>('');
  public searchKeyword$: Observable<string> = this.searchKeywordSource.asObservable();

  private searchResultsSource = new BehaviorSubject<any[]>([]);
  public searchResults$: Observable<any[]> = this.searchResultsSource.asObservable();

  constructor() {}

  setSearchKeyword(keyword: string): void {
    this.searchKeywordSource.next(keyword);
  }

  setSearchResults(searchResults: any[]): void {
    this.searchResultsSource.next(searchResults);
  }
}