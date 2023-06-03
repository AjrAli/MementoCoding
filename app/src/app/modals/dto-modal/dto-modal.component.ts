import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SchoolDto } from 'src/app/dto/school/schooldto';
import { GetStudentDto } from 'src/app/dto/student/getstudentdto';
import { StudentDto } from 'src/app/dto/student/studentdto';
@Component({
  selector: 'app-dto-modal',
  templateUrl: './dto-modal.component.html',
  styleUrls: ['./dto-modal.component.css']
})
export class DtoModalComponent {
  @Input()
  title!: string;
  @Input()
  dto!: any;
  @Output() passBackDTOToMainComponent = new EventEmitter<any>();
  constructor(public modal: NgbActiveModal) { }

  handleEvent(dto: any) {
    this.passBackDTOToMainComponent.emit(dto);
  }
  isSchoolDTO() {
    return this.dto instanceof SchoolDto || GetStudentDto;
  }
  closeModal() {
    console.log(this.dto);
    this.modal.dismiss('Cross click')
  }
}
