import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup } from '@angular/forms';
import { SearchStateService } from '../services/search/search-state.service';
import { SearchResultDto } from '../dto/search/searchresult-dto';
import { SearchService } from '../services/search/search.service';
import { UrlHistoryService } from '../services/shared/url-history.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  searchForm!: FormGroup;
  searchField: string = '';
  faSearch = faSearch;
  constructor(private router: Router,
    private authService: AuthenticationService,
    private searchStateService: SearchStateService,
    private searchService: SearchService,
    private urlHistoryService: UrlHistoryService) {
  }
  ngOnInit(): void {
    this.searchForm = new FormGroup({
      searchField: new FormControl(this.searchField)
    });
  }
  getVisitedUrls() {
    return this.urlHistoryService.getVisitedUrls();
  }
  getTitleOfVisitedUrls() {
    return this.urlHistoryService.getTitleOfVisitedUrls();
  }
  submitSearch(value?: string) {

    if (value) {
      this.searchField = value
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchField } });
      this.searchStateService.setSearchKeyword(this.searchField);
    } else {
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchForm.value.searchField } });
      this.searchStateService.setSearchKeyword(this.searchForm.value.searchField);
    }
  }
  isAdminConnected(): boolean {
    return this.authService.isAdmin();
  }
  isConnected() {
    return this.authService.isLoggedIn();
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/home']);
  }

}
