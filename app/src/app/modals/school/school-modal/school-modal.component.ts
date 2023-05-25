import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SchoolDto } from 'src/app/dto/school/schooldto';
@Component({
  selector: 'app-school-modal',
  templateUrl: './school-modal.component.html',
  styleUrls: ['./school-modal.component.css']
})
export class SchoolModalComponent {
  @Input()
  school!: SchoolDto;
  @Output() passBackSchoolToMainSchool = new EventEmitter<SchoolDto>();
  constructor(public modal: NgbActiveModal) {}

  handleEvent(school: SchoolDto) {
    this.passBackSchoolToMainSchool.emit(school);
  }
}
