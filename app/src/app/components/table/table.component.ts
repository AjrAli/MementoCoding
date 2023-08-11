import { Component, EventEmitter, HostListener, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { faTimes, faPencil, faEye, faArrowsUpDown, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { Subject, of, Observable } from 'rxjs';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { EntityUrl } from 'src/app/enum/entity';
import { Command } from 'src/app/enum/command';
import { SearchStateService } from 'src/app/services/search/search-state.service';
import { OrderByChoice } from 'src/app/enum/orderby-choice';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, takeUntil } from 'rxjs/operators';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { ToastService } from 'src/app/services/message-popup/toast.service';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit, OnDestroy, OnChanges {
  @Input() headers: string[] = [];
  @Input() headersTitle: string[] = [];
  @Input() data: any[] | undefined;
  @Input() doAction = true;
  @Input() searchEngine = false;
  @Input() totalItems: number = 0;
  @Output() nextPage = new EventEmitter<any>();
  @Output() actionCommands = new EventEmitter<{ dto: any, command: Command }>();
  @Output() orderQuery = new EventEmitter<{ header: string, order: OrderByChoice }>();
  @Output() filterQuery = new EventEmitter<string>();
  private destroy$: Subject<void> = new Subject<void>();
  searchField!: FormControl;
  changeOrder = false;
  keyword$!: Observable<string> | null;
  myIcons: { [key: string]: IconDefinition } = {};
  page = 1;
  pageSize = 10;
  showItems = 25;
  constructor(private router: Router,
    private searchStateService: SearchStateService,
    private toastService: ToastService,
    private activatedRoute: ActivatedRoute) {
    this.myIcons = {
      "faTimes": faTimes,
      "faPencil": faPencil,
      "faArrowsUpDown": faArrowsUpDown,
      "faEye": faEye
    };
  }
  ngOnChanges(changes: SimpleChanges): void {
    if(!this.searchField){
      if (this.doAction && this.data && this.data.length > 0) {
        this.searchField = new FormControl();
        this.searchField.valueChanges
          .pipe(
            debounceTime(500),
            distinctUntilChanged(),
            takeUntil(this.destroy$) // Unsubscribe from the observable when the component is destroyed
          )
          .subscribe({
            next: (response: any) => {
              this.filterQuery.emit(response);
              console.log(response);
            },
            error: (error: any) => {
              this.toastService.showSimpleError(error?.toString());
            },
            complete: () => console.info('complete')
          });
      }   
    }
  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  getResultLink(result: SearchResultDto): string[] | undefined {
    if (!this.doAction) {
      const routeType = result?.urlEntity?.toLowerCase() as EntityUrl;
      const routeCommands = [`/${routeType}`, String(result.id)];
      return routeCommands;
    }
    return undefined;
  }

  clearSearch(resultLink: string[] | undefined): void {
    if (this.searchEngine && resultLink) {
      const currentUrl = this.router.serializeUrl(this.router.createUrlTree(this.activatedRoute.snapshot.url));
      const resultUrl = this.router.serializeUrl(this.router.createUrlTree(resultLink));

      if (currentUrl === resultUrl) {
        this.searchStateService.setSearchKeyword('');
      }
    }
  }
  orderByField(header: string, changeOrder: boolean) {
    const order: OrderByChoice = changeOrder ? OrderByChoice.Ascending : OrderByChoice.Descending;
    this.changeOrder = changeOrder;
    this.orderQuery.emit({ header, order });
  }
  doNextPage() {
    this.nextPage.emit({ skip: ((this.page - 1) * this.pageSize), take: this.pageSize });
  }
  actionCommandRead(item: any) {
    this.actionCommands.emit({ dto: item, command: Command.Read });
  }
  actionCommandDelete(item: any) {
    this.actionCommands.emit({ dto: item, command: Command.Delete });
  }
  actionCommandUpdate(item: any) {
    this.actionCommands.emit({ dto: item, command: Command.Update });
  }
  ngOnInit(): void {
    if (this.searchEngine) {
      this.keyword$ = this.searchStateService.searchKeyword$;
    }
  }

  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: Event) {
    if (this.searchEngine) {
      if (window.innerHeight + document.documentElement.scrollTop >= document.body.offsetHeight) {
        this.showItems += 10;
      }
    }
  }
}