import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faTimes } from '@fortawesome/free-solid-svg-icons';
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

  faTimes = faTimes;
  constructor() {}

  
  doReturnDTOToDelete(item: object){
    this.returnDTOToDelete.emit(item);
  }
  ngOnInit(): void {}
}