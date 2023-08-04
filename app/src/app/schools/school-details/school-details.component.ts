import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { SchoolDto } from 'src/app/dto/school/school-dto';
import { StudentDto } from 'src/app/dto/student/student-dto';
import { PageDetailsDto } from 'src/app/dto/utilities/page-details-dto';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { SchoolService } from 'src/app/services/school/school.service';

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
  constructor(private route: ActivatedRoute, private schoolService: SchoolService,
    private toastService: ToastService) {
    if (this.route.snapshot.paramMap.get('id') !== null) {
      this.schoolId = Number.parseInt(this.route.snapshot.paramMap.get('id') as string);
    }
  }
  ngOnInit(): void {
    if (this.schoolId) {
      this.schoolService.getSchoolById(this.schoolId).subscribe({
        next: (response: any) => {
          this.school = new SchoolDto();
          Object.assign(this.school, response.schoolDto);
          this.pageDetails.totalItems = response.schoolDto.students.length;
          this.students = response.schoolDto.students.map((studentData: any) => {
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
  }




}
