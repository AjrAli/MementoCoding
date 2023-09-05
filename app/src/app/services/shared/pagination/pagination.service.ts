import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaginationService {

  private currentPageSubject = new BehaviorSubject<number>(1);
  currentPage$ : Observable<number> = this.currentPageSubject.asObservable();

  getCurrentPage() : number{
    return this.currentPageSubject.value;
  }
  setCurrentPage(page: number) : void{
    this.currentPageSubject.next(page);
  }
}
