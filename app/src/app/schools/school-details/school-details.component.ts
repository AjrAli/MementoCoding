import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { SchoolDto } from 'src/app/dto/school/school-dto';
import { StudentDto } from 'src/app/dto/student/student-dto';
import { ODataQueryDto } from 'src/app/dto/utilities/odata-query-dto';
import { PageDetailsDto } from 'src/app/dto/utilities/page-details-dto';
import { OrderByChoice } from 'src/app/enum/orderby-choice';
import { StudentProperties } from 'src/app/enum/student-properties';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { SchoolService } from 'src/app/services/school/school.service';
import { StudentService } from 'src/app/services/student/student.service';

@Component({
  selector: 'app-school-details',
  templateUrl: './school-details.component.html',
  styleUrls: ['./school-details.component.css']
})
export class SchoolDetailsComponent implements OnInit {

  students: StudentDto[] = [];
  pageDetails: PageDetailsDto = new PageDetailsDto();
  school: SchoolDto = new SchoolDto();
  schoolId: number = 0;
  constructor(private route: ActivatedRoute,
    private schoolService: SchoolService,
    private toastService: ToastService,
    private studentService: StudentService,
    private changeDetectorRef: ChangeDetectorRef) {
    if (this.route.snapshot.paramMap.get('id') !== null) {
      this.schoolId = Number.parseInt(this.route.snapshot.paramMap.get('id') as string);
    }
  }
  getStudents(skip?: number, take?: number): void {
    let query: ODataQueryDto = new ODataQueryDto();
    query.top = take?.toString() || '0';
    query.skip = skip?.toString() || '0';
    query.orderBy.push(`${StudentProperties.LastName} ${OrderByChoice.Ascending}`);
    query.filter.push(`${StudentProperties.SchoolId} eq ${this.school.id}`);
    this.studentService.getStudents(query).subscribe({
      next: (response: any) => {
        this.pageDetails.totalItems = response.count;
        this.students = response.studentsDto.map((studentData: any) => {
          const student = new StudentDto();
          Object.assign(student, studentData);
          return student;
        });
      },
      error: (error: ErrorResponse) => {
        this.toastService.showError(error);
      },
      complete: () => console.info('complete')
    });
  }
  handleNextPage(result: any) {
    this.pageDetails.skip = result.skip;
    this.pageDetails.take = result.take;
    this.getStudents(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }
  ngOnInit(): void {
    if (this.schoolId) {
      this.schoolService.getSchoolById(this.schoolId).subscribe({
        next: (response: any) => {
          this.school = new SchoolDto();
          Object.assign(this.school, response.schoolDto);
          this.getStudents(0, 10);
        },
        error: (error: ErrorResponse) => {
          this.toastService.showError(error);
        },
        complete: () => console.info('complete')
      });
    }
  }




}
