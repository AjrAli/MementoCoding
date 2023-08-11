import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject, of } from 'rxjs';
import { takeUntil, catchError, debounceTime, distinctUntilChanged, startWith } from 'rxjs/operators';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { SearchService } from 'src/app/services/search/search.service';
import { SearchStateService } from 'src/app/services/search/search-state.service';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnDestroy {
  searchResults: SearchResultDto[] | undefined = [];
  keyword: string = '';
  private destroy$: Subject<void> = new Subject<void>();
  private searchKeyword$ = new Subject<string>();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private searchService: SearchService,
    private searchStateService: SearchStateService,
    private toastService: ToastService
  ) {
    this.initializeSearchStateService();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeSearchStateService(): void {
    if (this.route.snapshot.queryParams['keyword']) {
      this.keyword = decodeURIComponent(this.route.snapshot.queryParams['keyword']);
      this.searchStateService.setSearchKeyword(this.keyword);
    }
    // Subscribe to changes in the search keyword in the SearchStateService
    this.searchStateService.searchKeyword$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (keyword: string) => {
          if (!keyword)
            this.router.navigate(['/home']);
          // Emit the new search keyword to the searchKeyword$ Subject
          // whenever it changes in the SearchStateService
          this.searchKeyword$.next(keyword);
        },
        error: (error: any) => {
          this.toastService.showSimpleError(error.toString());
        },
        complete: () => console.info('complete')
      });

    // Use the startWith operator to trigger the initial search
    // when the component is initialized with a search keyword
    this.searchKeyword$
      .pipe(
        startWith(this.keyword), // Use an initial empty string to trigger the search on component load
        debounceTime(500), // Wait for 500ms between consecutive requests
        distinctUntilChanged(), // Ignore consecutive identical requests
        takeUntil(this.destroy$) // Unsubscribe from the observable when the component is destroyed
      )
      .subscribe({
        next: (keyword: string) => {
          this.onSearch(keyword);
        },
        error: (error: any) => {
          this.toastService.showSimpleError(error.toString());
        },
        complete: () => console.info('complete')
      });
  }

  onSearch(keyword: string): void {
    this.searchService
      .getSearchResults(keyword)
      ?.pipe(
        takeUntil(this.destroy$),
        catchError((error) => {
          this.toastService.showError(error as ErrorResponse);
          return of(null); // Return a new observable with null value to continue the observable chain
        })
      )
      .subscribe({
        next: (response: any) => {
          if (!response || response?.searchResultsDto?.length === 0) {
            this.searchResults = undefined;
            return;
          }
          this.searchResults = response?.searchResultsDto?.map(
            (searchData: any) => {
              const searchResult = new SearchResultDto(
                searchData.type?.toLowerCase()
              );
              Object.assign(searchResult, searchData);
              return searchResult;
            }
          );
        },
        error: (error: any) => {
          this.toastService.showSimpleError(error.toString());
        },
        complete: () => console.info('complete')
      });
  }
}
