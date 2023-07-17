import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { faTimes, faPencil, faEye } from '@fortawesome/free-solid-svg-icons';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit{
  @Input() headers: string[] = [];
  @Input() headersTitle: string[] = [];
  @Input() data: any[] = [];
  @Input() noAction = true;
  @Input() totalItems: number = 0;
  @Output() nextPage = new EventEmitter<any>();
  @Output() returnDTOToGet = new EventEmitter<any>();
  @Output() returnDTOToDelete = new EventEmitter<any>();
  @Output() returnDTOToUpdate = new EventEmitter<any>();
  page = 1;
  pageSize = 10;
  faTimes = faTimes;
  faPencil = faPencil;
  faEye = faEye;
  constructor() {}
  doNextPage(){
    this.nextPage.emit({skip: ((this.page - 1) * this.pageSize), take: this.pageSize});
  }
  doReturnDTOToGet(item: any){
    this.returnDTOToGet.emit(item);
  }
  doReturnDTOToDelete(item: any){
    this.returnDTOToDelete.emit(item);
  }
  doReturnDTOToUpdate(item: any){
    this.returnDTOToUpdate.emit(item);
  }
  ngOnInit(): void {}

}