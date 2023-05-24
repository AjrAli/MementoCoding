import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../dto/school/schooldto';
import { StudentDto } from '../dto/student/studentdto';
import { SchoolService } from '../services/school/school.service';
import { StudentService } from '../services/student/student.service';

@Component({
  selector: 'app-student-form',
  templateUrl: './student-form.component.html',
  styleUrls: ['./student-form.component.css']
})
export class StudentFormComponent {

  student: StudentDto = {
    id: 0,
    firstName: '',
    lastName: '',
    age: 0,
    adress: '',
    schoolId: 0
  };

  schools: SchoolDto[] = [];

  constructor(
    private router: Router,
    private studentService: StudentService,
    private schoolService: SchoolService
  ) { }

  ngOnInit(): void {
    this.fetchSchools();
  }

  fetchSchools(): void {
    this.schoolService.getSchools().subscribe((response: any) => {
      this.schools = response.SchoolsDto;
    });
  }

  addStudent(): void {
    this.studentService.createStudent(this.student).subscribe({
      next: () => this.router.navigate(['/students']),
      error: (e) => {
        console.error('Add student error:', e);
        alert('Failed to add student. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }
}
