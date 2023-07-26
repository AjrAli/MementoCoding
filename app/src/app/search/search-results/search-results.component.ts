// search-results.component.ts
import { Component, ElementRef, HostListener, Input, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Entity, SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { SearchStateService } from 'src/app/services/search/search-state.service';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent {
  @Input() searchResults: SearchResultDto[] = [];
  keyword$: Observable<string>;
  showItems = 25;
  
  constructor(
    private router: Router,
    private searchStateService: SearchStateService,
    private activatedRoute: ActivatedRoute
  ) {
    this.keyword$ = this.searchStateService.searchKeyword$;
  }

  getResultLink(result: SearchResultDto): string[] {
    const routeType = result?.type?.toLowerCase() as Entity;
    const routeCommands = [`/${routeType}`, String(result.id)];
    return routeCommands;
  }

  clearSearch(resultLink: string[]): void {
    const currentUrl = this.router.serializeUrl(this.router.createUrlTree(this.activatedRoute.snapshot.url));
    const resultUrl = this.router.serializeUrl(this.router.createUrlTree(resultLink));

    if (currentUrl === resultUrl) {
      this.searchStateService.setSearchKeyword('');
    }
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: Event) {
    if (window.innerHeight + document.documentElement.scrollTop >= document.body.offsetHeight) {
      this.showItems += 10;
    }
  }
}
