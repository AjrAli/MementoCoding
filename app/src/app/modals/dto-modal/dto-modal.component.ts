import { Component, EventEmitter, Input, Output, Type, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SchoolDto } from 'src/app/dto/school/school-dto';
import { StudentDto } from 'src/app/dto/student/student-dto';
import { BaseFormComponent } from 'src/app/forms/base-form.component';
import { SchoolFormComponent } from 'src/app/forms/school/school-form.component';
import { StudentFormComponent } from 'src/app/forms/student/student-form.component';
@Component({
  selector: 'app-dto-modal',
  templateUrl: './dto-modal.component.html',
  styleUrls: ['./dto-modal.component.css']
})
export class DtoModalComponent {
  @ViewChild('formComponent') formComponent!: BaseFormComponent;
  @Input()
  title!: string;
  @Input()
  dto!: any;
  @Output() passBackDTOToMainComponent = new EventEmitter<any>();
  constructor(public modal: NgbActiveModal) { }

  handleEvent(dto: any) {
    this.passBackDTOToMainComponent.emit(dto);
  }
  isAccount() {
    if (this.dto) {
      const isAccount = this.dto.typeInstance === 'AccountDto';
      return isAccount;
    }
    return false;
  }
  isSchool() {
    if (this.dto) {
      const isSchool = this.dto.typeInstance === 'SchoolDto';
      return isSchool;
    }
    return false;
  }
  isStudent() {
    if (this.dto) {
      const isStudent = this.dto.typeInstance === 'StudentDto';
      return isStudent;
    }
    return false;
  }
  closeModal() {
    this.modal.dismiss('Cross click')
  }
  doClearForm() {
    this.formComponent.clearForm();
  }
}
