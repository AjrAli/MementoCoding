import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../../dto/school/school-dto';
import { StudentDto } from '../../dto/student/student-dto';
import { SchoolService } from '../../services/school/school.service';
import { StudentService } from '../../services/student/student.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { GetSchoolDto } from 'src/app/dto/school/getschool-dto';
import { interval, firstValueFrom, lastValueFrom } from 'rxjs';
import { BaseFormComponent } from '../base-form.component';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent extends BaseFormComponent implements OnInit {

  student: StudentDto = new StudentDto();
  schools: GetSchoolDto[] = [];
  title: string = 'Add Student';
  constructor(
    private schoolService: SchoolService,
    private toastService: ToastService
  ) { super(); }
  async fetchSchools(): Promise<void> {
    try {
      const response: any = await lastValueFrom(this.schoolService.getSchools());
      this.schools = response.schoolsDto.map((schoolData: any) => {
        const school = new GetSchoolDto();
        Object.assign(school, schoolData);
        return school;
      });
    } catch (error) {
      this.toastService.showError(error as ErrorResponse);
    }
  }
  async ngOnInit(): Promise<void> {
    await this.fetchSchools();
    this.student = this.dto as StudentDto;
    if (this.student?.firstName) {
      this.title = 'Update Student';
    } else {
      this.student.schoolId = this.schools[0]?.id;
    }
    this.baseForm = new FormGroup({
      id: new FormControl(this.student?.id),
      firstName: new FormControl(this.student?.firstName, [Validators.required, Validators.maxLength(100)]),
      lastName: new FormControl(this.student?.lastName, [Validators.required, Validators.maxLength(100)]),
      age: new FormControl(this.student?.age, [Validators.required, Validators.min(5), Validators.max(20)]),
      adress: new FormControl(this.student?.adress, [Validators.required, Validators.maxLength(100)]),
      schoolId: new FormControl(this.student?.schoolId)
    });
  }
  addStudent(): void {
    this.student = this.baseForm.value;
    this.passBackDTO.emit(this.student);
  }
}
