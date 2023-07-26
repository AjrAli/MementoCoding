import { Component, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged, startWith } from 'rxjs/operators';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { SearchService } from 'src/app/services/search/search.service';
import { SearchStateService } from 'src/app/services/search/search-state.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnDestroy {
  searchResults: SearchResultDto[] = [];
  keyword: string = '';
  private destroy$: Subject<void> = new Subject<void>();
  private searchKeyword$ = new Subject<string>();

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private searchService: SearchService,
    private searchStateService: SearchStateService
  ) {
    this.initializeSearchStateService();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initializeSearchStateService(): void {
    if(this.route.snapshot.queryParams['keyword']){
      this.keyword = decodeURIComponent(this.route.snapshot.queryParams['keyword']);
      this.searchStateService.setSearchKeyword(this.keyword);
    }
    // Subscribe to changes in the search keyword in the SearchStateService
    this.searchStateService.searchKeyword$
      .pipe(takeUntil(this.destroy$))
      .subscribe((keyword: string) => {
        this.keyword = keyword;
        // Emit the new search keyword to the searchKeyword$ Subject
        // whenever it changes in the SearchStateService
        this.searchKeyword$.next(this.keyword);
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
      .subscribe((keyword: string) => {
        this.keyword = keyword;
        if(!this.keyword)
        this.router.navigate(['/home']);
        this.onSearch(this.keyword);
      });
  }

  onSearch(keyword: string): void {
    this.searchService
      .getSearchResults(keyword)
      .pipe(takeUntil(this.destroy$))
      .subscribe((response: any) => {
        this.searchResults = response.searchResultsDto.map(
          (searchData: any) => {
            const searchResult = new SearchResultDto(
              searchData.type?.toLowerCase()
            );
            Object.assign(searchResult, searchData);
            return searchResult;
          }
        );
      });
  }
}
