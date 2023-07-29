import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ErrorResponse } from 'src/app/dto/response/error/error-response';
import { GetStudentDto } from 'src/app/dto/student/getstudent-dto';
import { ToastService } from 'src/app/services/message-popup/toast.service';
import { StudentService } from 'src/app/services/student/student.service';

@Component({
  selector: 'app-student-details',
  templateUrl: './student-details.component.html',
  styleUrls: ['./student-details.component.css']
})
export class StudentDetailsComponent {
  
  student: GetStudentDto = new GetStudentDto();
  studentId: number = 0;
  constructor(private route: ActivatedRoute, private studentService: StudentService,
    private _modalService: NgbModal,
    private changeDetectorRef: ChangeDetectorRef,
    private toastService: ToastService) {
    if (this.route.snapshot.paramMap.get('id') !== null) {
      this.studentId = Number.parseInt(this.route.snapshot.paramMap.get('id') as string);
    }
  }
  ngOnInit(): void {
    if (this.studentId) {
      this.studentService.getStudentById(this.studentId).subscribe((response: any) => {
        this.student = new GetStudentDto();
        Object.assign(this.student, response.studentDto);
      },
      (error : ErrorResponse) => {
        this.toastService.showError(error);
      });
    }
  }

}
