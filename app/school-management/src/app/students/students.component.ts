import { Component, OnInit } from '@angular/core';
import { StudentDto } from '../dto/student/studentdto';
import { StudentService } from '../services/student/student.service';

@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students: StudentDto[] = [];

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.getStudents();
  }

  getStudents(): void {
    this.studentService.getStudents().subscribe((students: any) => {
      console.debug(students);
      this.students = students.studentsDto;
    });
  }

  createStudent(student: StudentDto): void {
    this.studentService.createStudent(student).subscribe(() => {
      this.getStudents();
    });
  }

  updateStudent(student: StudentDto): void {
    this.studentService.updateStudent(student).subscribe(() => {
      this.getStudents();
    });
  }

  deleteStudent(studentId: number): void {
    this.studentService.deleteStudent(studentId).subscribe(() => {
      this.getStudents();
    });
  }
}