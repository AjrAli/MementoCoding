import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { environment } from 'src/app/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  getSearchResults(keyword: string): Observable<SearchResultDto[]> {
    return this.http.get<SearchResultDto[]>(`${this.apiUrl}/SearchQuery/${keyword}`);
  }
}