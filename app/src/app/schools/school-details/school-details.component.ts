import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { GetSchoolDto } from 'src/app/dto/school/getschool-dto';
import { GetStudentDto } from 'src/app/dto/student/getstudent-dto';
import { PageDetailsDto } from 'src/app/dto/utilities/page-details-dto';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { SchoolService } from 'src/app/services/school/school.service';

@Component({
  selector: 'app-school-details',
  templateUrl: './school-details.component.html',
  styleUrls: ['./school-details.component.css']
})
export class SchoolDetailsComponent implements OnInit {

  students: GetStudentDto[] = [];
  pageDetails: PageDetailsDto = new PageDetailsDto();
  school: GetSchoolDto = new GetSchoolDto();
  schoolId: number = 0;
  constructor(private route: ActivatedRoute, private schoolService: SchoolService,
    private _modalService: NgbModal,
    private changeDetectorRef: ChangeDetectorRef,
    private toastService:ToastService) {
    if (this.route.snapshot.paramMap.get('id') !== null) {
      this.schoolId = Number.parseInt(this.route.snapshot.paramMap.get('id') as string);
    }
  }
  ngOnInit(): void {
    if (this.schoolId) {
      this.schoolService.getSchoolById(this.schoolId).subscribe((response: any) => {
        this.school = new GetSchoolDto();
        Object.assign(this.school, response.schoolDto);
        this.pageDetails.totalItems = response.schoolDto.students.length;
        this.students = response.schoolDto.students.map((studentData: any) => {
          const student = new GetStudentDto();
          Object.assign(student, studentData);
          return student;
        });
      },
      (error : ErrorResponse) => {
        this.toastService.showError(error);
      });
    }
  }




}
