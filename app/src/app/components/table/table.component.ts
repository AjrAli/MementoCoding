import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faTimes, faPencil } from '@fortawesome/free-solid-svg-icons';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SchoolDto } from 'src/app/dto/school/schooldto';
@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
})
export class TableComponent implements OnInit {
  @Input() headers: string[] = [];
  @Input() data: any[] = [];
  @Output() returnDTOToDelete = new EventEmitter<object>();
  @Output() returnDTOToUpdate = new EventEmitter<object>();
  faTimes = faTimes;
  faPencil = faPencil;
  constructor() {}

  
  doReturnDTOToDelete(item: object){
    this.returnDTOToDelete.emit(item);
  }
  doReturnDTOToUpdate(item: object){
    this.returnDTOToUpdate.emit(item);
  }
  ngOnInit(): void {}
}