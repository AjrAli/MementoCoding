import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../services/authentification/authentication.service';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { FormControl, FormGroup } from '@angular/forms';
import { SearchStateService } from '../services/search/search-state.service';
import { SearchResultDto } from '../dto/search/searchresult-dto';
import { SearchService } from '../services/search/search.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  constructor(private router: Router,
    private authService: AuthenticationService,
    private searchStateService: SearchStateService,
    private searchService: SearchService) { }
  searchForm!: FormGroup;
  searchField: string = '';
  faSearch = faSearch;
  ngOnInit(): void {
    this.searchForm = new FormGroup({
      searchField: new FormControl(this.searchField)
    });
  }

  submitSearch(value?: string) {

    if(value){
      this.searchField = value
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchField } });
      this.searchStateService.setSearchKeyword(this.searchField);
    }else{
      this.router.navigate(['/search'], { queryParams: { keyword: this.searchForm.value.searchField } });
      this.searchStateService.setSearchKeyword(this.searchForm.value.searchField);
    }
  }
  isConnected() {
    return this.authService.isLoggedIn();
  }
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/home']);
  }

}
