import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../../dto/school/schooldto';
import { StudentDto } from '../../dto/student/studentdto';
import { SchoolService } from '../../services/school/school.service';
import { StudentService } from '../../services/student/student.service';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent implements OnInit {
  @Input()
  dto: any;
  student: StudentDto = new StudentDto();
  schools: SchoolDto[] = [];
  @Output() passBackDTO = new EventEmitter<any>();
  studentForm!: FormGroup;
  title: string = 'Add Student';
  constructor(
    private router: Router,
    private studentService: StudentService,
    private schoolService: SchoolService
  ) { }
  fetchSchools(): void {
    this.schoolService.getSchools().subscribe((response: any) => {
      this.schools = response.schoolsDto;
    });
  }
  ngOnInit(): void {
    this.fetchSchools();
    this.student = this.dto as StudentDto;
    if (this.student?.firstName) {
      this.title = 'Update Student';
    }
    this.studentForm = new FormGroup({
      id: new FormControl(this.student.id),
      firstName: new FormControl(this.student.firstName),
      lastName: new FormControl(this.student.lastName),
      age: new FormControl(this.student.age),
      adress: new FormControl(this.student.adress),
      schoolId: new FormControl(this.student.schoolId)
    });
  }

  addStudent(): void {
    this.student = this.studentForm.value;
    this.clearForm();
    this.passBackDTO.emit(this.student);
  }
  clearForm() {
    this.studentForm.reset();
    Object.keys(this.studentForm.controls).forEach(controlName => {
      this.studentForm.get(controlName)?.patchValue('');
    });
  }


}
