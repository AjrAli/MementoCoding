import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GetSchoolDto } from 'src/app/dto/school/getschooldto';
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
  isSchool() {
    const isSchool = this.dto.typeInstance === 'SchoolDto' || this.dto.typeInstance === 'GetSchoolDto';
    console.log(isSchool);
    return isSchool;
  }
  isStudent() {
    const isStudent = this.dto.typeInstance === 'StudentDto' || this.dto.typeInstance === 'GetStudentDto';
    console.log(isStudent);
    return isStudent;
  }
  closeModal() {
    console.log(this.dto);
    this.modal.dismiss('Cross click')
  }
}
