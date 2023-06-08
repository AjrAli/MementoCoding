import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { StudentDto } from '../dto/student/student-dto';
import { GetStudentDto } from '../dto/student/getstudent-dto';
import { StudentService } from '../services/student/student.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { ErrorResponse } from '../dto/error/error-response';
import { DtoModalComponent } from '../modals/dto-modal/dto-modal.component';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students: GetStudentDto[] = [];
  pageDetails: PageDetailsDto = new PageDetailsDto();
  newStudent: StudentDto = new StudentDto();
  constructor(private studentService: StudentService,
    private _modalService: NgbModal,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
  }

  getStudents(skip?: number, take?: number): void {
    this.studentService.getStudents(skip, take).subscribe((students: any) => {
      this.pageDetails.totalItems = students.count;
      this.students = students.studentsDto.map((studentData: any) => {
        const student = new GetStudentDto();
        Object.assign(student, studentData);
        return student;
      });
    });
  }

  createStudent(student: StudentDto): void {
    this.studentService.createStudent(student).subscribe({
      next: () => {
        this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e) => {
        console.error('Add student error:', e);
        alert('Failed to add student. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  updateStudent(student: StudentDto): void {
    this.studentService.updateStudent(student).subscribe({
      next: () => {
        this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e) => {
        console.error('Update student error:', e);
        alert('Failed to update student. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  deleteStudent(studentId: number): void {
    this.studentService.deleteStudent(studentId).subscribe({
      next: () => {
        this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e) => {
        console.error('Delete student error:', e);
        alert('Failed to delete student. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  openAddModal() {
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'Student Modal';
    modalRef.componentInstance.dto = this.newStudent;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe((receivedStudent: StudentDto) => {
      this.createStudent(receivedStudent);
      this.changeDetectorRef.detectChanges();
      modalRef.close();
    });
  }
  handleNextPage(result: any) {
    this.pageDetails.skip = result.skip;
    this.pageDetails.take = result.take;
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }
  handleReturnStudentToDelete(studentReturn: any) {
    let student = studentReturn as GetStudentDto;
    const modalRef = this._modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.name = student.firstName;
    if (student.haschildren) {
      const errorMessage: ErrorResponse = {
        message: '',
        validationErrors: []
      };
      modalRef.componentInstance.errorMessage = errorMessage;
      modalRef.componentInstance.errorMessage.message = `(Impossible to delete ${student.firstName}, sub table linked to it!)`;
    }
    modalRef.result.then((result) => {
      if (result === 'yes' && !student.haschildren) {
        this.deleteStudent(student.id);
      } else {
        console.log('Action annulée');
      }
    }).catch((error) => {
      // La boîte de dialogue a été fermée avec une erreur
      console.log('Erreur :', error);
    });
  }
  handleReturnStudentToUpdate(studentReturn: any) {
    let student = studentReturn as StudentDto;
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'Student Modal';
    modalRef.componentInstance.dto = student;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe((receivedStudent: StudentDto) => {
      this.updateStudent(receivedStudent);
      this.changeDetectorRef.detectChanges();
      modalRef.close();
    });
  }





}