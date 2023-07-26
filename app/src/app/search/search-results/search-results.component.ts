// search-results.component.ts
import { Component, Input } from '@angular/core';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent {
  @Input() searchResults: SearchResultDto[] = [];

  constructor() {}

}
