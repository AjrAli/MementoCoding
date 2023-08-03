import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { StudentDto } from '../dto/student/student-dto';
import { GetStudentDto } from '../dto/student/getstudent-dto';
import { StudentService } from '../services/student/student.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { ErrorResponse } from '../dto/response/error/error-response';
import { DtoModalComponent } from '../modals/dto-modal/dto-modal.component';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
import { Router } from '@angular/router';
import { Command } from '../enum/command';
import { BaseResponse } from '../dto/response/base-response';
import { ToastService } from '../services/message-popup/toast.service';
import { firstValueFrom, lastValueFrom } from 'rxjs';
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
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
  }

  getStudents(skip?: number, take?: number): void {
    this.studentService.getStudents(skip, take).subscribe((response: any) => {
      this.pageDetails.totalItems = response.count;
      this.students = response.studentsDto.map((studentData: any) => {
        const student = new GetStudentDto();
        Object.assign(student, studentData);
        return student;
      });
    },
    (error : ErrorResponse) => {
      this.toastService.showError(error);
    });
  }

  async createStudent(student: StudentDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom (this.studentService.createStudent(student));
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to create student. Please try again later.');
      throw e; 
    }
  }

  async updateStudent(student: StudentDto): Promise<BaseResponse>  {
    try {
      const response: BaseResponse = await firstValueFrom (this.studentService.updateStudent(student));
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to update student. Please try again later.');
      throw e; 
    }
  }

  deleteStudent(studentId: number): void {
    this.studentService.deleteStudent(studentId).subscribe({
      next: (response: BaseResponse) => {
        this.toastService.showSuccess(response.message);
        this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e: ErrorResponse) => {
        this.toastService.showError(e);
        this.toastService.showSimpleError('Failed to delete student. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  async openAddModal() {
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'Student Modal';
    modalRef.componentInstance.dto = this.newStudent;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe(async (receivedStudent: StudentDto) => {
      const result = await this.createStudent(receivedStudent);
      if (result?.success) {
        this.toastService.showSuccess(result.message);
        this.changeDetectorRef.detectChanges();
        modalRef.componentInstance.doClearForm();
        modalRef.close();
      } else {
        let responseError = result as ErrorResponse;
        this.toastService.showError(responseError);
      }
    });
  }
  handleNextPage(result: any) {
    this.pageDetails.skip = result.skip;
    this.pageDetails.take = result.take;
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }

  handleActionCommands(event: { dto: any, command: Command }) {
    switch (event.command) {
      case Command.Read:
        this.navigateToStudentById(event.dto);
        break;
      case Command.Update:
        this.updateStudentByDtoModal(event.dto);
        break;
      case Command.Delete:
        this.deleteStudentByIdByConfirmModal(event.dto);
        break;
      default:
        console.log(`Error : ${event.command}`);
        break;
    }
  }

  navigateToStudentById(studentReturn: any) {
    let student = studentReturn as GetStudentDto;
    this.router.navigate(['/students', student.id]);
  }
  deleteStudentByIdByConfirmModal(studentReturn: any) {
    let student = studentReturn as GetStudentDto;
    const modalRef = this._modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.name = student.firstName;
    if (student.haschildren) {
      const errorMessage: ErrorResponse = {
        success: false,
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
      this.toastService.showError(error as ErrorResponse);  
    });
  }
  async updateStudentByDtoModal(studentReturn: any) {
    let student = studentReturn as StudentDto;
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'Student Modal';
    modalRef.componentInstance.dto = student;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe(async (receivedStudent: StudentDto) => {
      const result = await this.updateStudent(receivedStudent);
      if (result?.success) {
        this.toastService.showSuccess(result.message);
        this.changeDetectorRef.detectChanges();
        modalRef.componentInstance.doClearForm();
        modalRef.close();
      } else {
        let responseError = result as ErrorResponse;
        this.toastService.showError(responseError);
      }
    });
  }
}