import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { GetSchoolDto } from 'src/app/dto/school/getschool-dto';
import { SchoolDto } from 'src/app/dto/school/school-dto';
import { GetStudentDto } from 'src/app/dto/student/getstudent-dto';
import { StudentDto } from 'src/app/dto/student/student-dto';
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
    if (this.dto) {
      const isSchool = this.dto.typeInstance === 'SchoolDto' || this.dto.typeInstance === 'GetSchoolDto';
      console.log(isSchool);
      return isSchool;
    }
    return false;
  }
  isStudent() {
    if (this.dto) {
      const isStudent = this.dto.typeInstance === 'StudentDto' || this.dto.typeInstance === 'GetStudentDto';
      console.log(isStudent);
      return isStudent;
    }
    return false;
  }
  closeModal() {
    console.log(this.dto);
    this.modal.dismiss('Cross click')
  }
}
