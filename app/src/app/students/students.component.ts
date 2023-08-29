import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { StudentDto } from '../dto/student/student-dto';
import { StudentService } from '../services/student/student.service';
import { ErrorResponse } from '../dto/response/error/error-response';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
import { Router } from '@angular/router';
import { Command } from '../enum/command';
import { BaseResponse } from '../dto/response/base-response';
import { ToastService } from '../services/message-popup/toast.service';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { ModalService } from '../services/modal/modal.service';
import { ODataQueryDto } from '../dto/utilities/odata-query-dto';
import { OrderByChoice } from '../enum/orderby-choice';
import { StudentProperties } from '../enum/student-properties';
import { PaginationService } from '../services/shared/pagination/pagination.service';
@Component({
  selector: 'app-students',
  templateUrl: './students.component.html',
  styleUrls: ['./students.component.css']
})
export class StudentsComponent implements OnInit {
  students: StudentDto[] | undefined = [];
  pageDetails: PageDetailsDto = new PageDetailsDto();
  newStudent: StudentDto = new StudentDto();
  queryOptions: ODataQueryDto = new ODataQueryDto();
  constructor(private studentService: StudentService,
    private modalService: ModalService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private toastService: ToastService,
    private paginationService: PaginationService) { }

  ngOnInit(): void {
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
  }

  getStudents(skip?: number, take?: number): void {
    this.queryOptions.top = take?.toString() || '0';
    this.queryOptions.skip = skip?.toString() || '0';
    this.studentService.getStudents(this.queryOptions).subscribe({
      next: (response: any) => {
        if(!response || response.count === 0){
          this.students = undefined;
          return;
        }
        this.pageDetails.totalItems = response.count;
        this.students = response.studentsDto.map((studentData: any) => {
          const student = new StudentDto();
          Object.assign(student, studentData);
          return student;
        });
      },
      error: (error: ErrorResponse) => {
        this.students = undefined;
        this.toastService.showError(error);
      },
      complete: () => console.info('complete')
    });
  }

  async createStudent(student: StudentDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.studentService.createStudent(student));
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to create student. Please try again later.');
      throw e;
    }
  }

  async updateStudent(student: StudentDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.studentService.updateStudent(student));
      if((this.pageDetails.totalItems - 1) % this.pageDetails.take === 0 && this.queryOptions.keywords.length > 0){
        this.paginationService.setCurrentPage(this.pageDetails.skip / this.pageDetails.take);
        this.pageDetails.skip -= this.pageDetails.take;
      }
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to update student. Please try again later.');
      throw e;
    }
  }

  async deleteStudent(studentId: number): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.studentService.deleteStudent(studentId));
      if((this.pageDetails.totalItems - 1) % this.pageDetails.take === 0){
        this.paginationService.setCurrentPage(this.pageDetails.skip / this.pageDetails.take);
        this.pageDetails.skip -= this.pageDetails.take;
      }
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to delete student. Please try again later.');
      throw e;
    }
  }

  async openAddModal(): Promise<void> {
    await this.modalService.openDtoModal(this.newStudent, 'School Modal', this.createStudent.bind(this));
  }
  async deleteStudentByIdByConfirmModal(studentReturn: any) {
    let student = studentReturn as StudentDto;
    await this.modalService.openConfirmModal(student.id, student.haschildren, student.firstName, this.deleteStudent.bind(this));
  }
  async updateStudentByDtoModal(studentReturn: any) {
    let student = studentReturn as StudentDto;
    await this.modalService.openDtoModal(student, 'School Modal', this.updateStudent.bind(this));
  }


  handleNextPage(result: any) {
    this.pageDetails.skip = result.skip;
    this.pageDetails.take = result.take;
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }
  handleFilterQuery(event: string){
    if(event){
      this.paginationService.setCurrentPage(1);
      this.pageDetails = new PageDetailsDto();
      const studentProps = new StudentProperties();
      for (const prop of Object.keys(studentProps)) {
        const key = prop;
        const value = studentProps[prop];
        this.queryOptions.filter.push({ key, value });
      }
      const stringWithoutExtraSpaces = event.replace(/\s{2,}/g, ' ');
      this.queryOptions.keywords = stringWithoutExtraSpaces.trim().split(' ');
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      this.changeDetectorRef.detectChanges();
    }else{
      this.queryOptions.filter = [];
      this.queryOptions.keywords = [];
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      this.changeDetectorRef.detectChanges();     
    }
  }
  handleOrderQuery(event: {header: string, order: OrderByChoice}){
    if(event.header && event.order){
      this.queryOptions.orderBy = [];
      this.queryOptions.orderBy.push(`${event.header} ${event.order}`);
      this.getStudents(this.pageDetails.skip, this.pageDetails.take);
      this.changeDetectorRef.detectChanges();
    }
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
    let student = studentReturn as StudentDto;
    this.router.navigate(['/students', student.id]);
  }
}