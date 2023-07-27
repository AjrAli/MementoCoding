import { Component, EventEmitter, HostListener, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { faTimes, faPencil, faEye } from '@fortawesome/free-solid-svg-icons';
import { Observable } from 'rxjs';
import { SearchResultDto } from 'src/app/dto/search/searchresult-dto';
import { Entity } from 'src/app/enum/entity';
import { Command } from 'src/app/enum/command';
import { SearchStateService } from 'src/app/services/search/search-state.service';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  @Input() headers: string[] = [];
  @Input() headersTitle: string[] = [];
  @Input() data: any[] = [];
  @Input() noAction = true;
  @Input() searchEngine = false;
  @Input() totalItems: number = 0;
  @Output() nextPage = new EventEmitter<any>();
  @Output() actionCommands = new EventEmitter<{dto : any, command: Command}>();
  keyword$!: Observable<string> | null;
  page = 1;
  pageSize = 10;
  faTimes = faTimes;
  faPencil = faPencil;
  faEye = faEye;
  showItems = 25;
  constructor(private router: Router,
    private searchStateService: SearchStateService,
    private activatedRoute: ActivatedRoute) { }

  getResultLink(result: SearchResultDto): string[] | undefined {
    if (!this.noAction) {
      const routeType = result?.urlEntity?.toLowerCase() as Entity;
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

  doNextPage() {
    this.nextPage.emit({ skip: ((this.page - 1) * this.pageSize), take: this.pageSize });
  }
  actionCommandRead(item: any) {
    this.actionCommands.emit({dto : item, command : Command.Read});
  }
  actionCommandDelete(item: any) {
    this.actionCommands.emit({dto : item, command : Command.Delete});
  }
  actionCommandUpdate(item: any) {
    this.actionCommands.emit({dto : item, command : Command.Update});
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