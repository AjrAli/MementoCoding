import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GetSchoolDto } from 'src/app/dto/school/getschool-dto';
import { SchoolService } from 'src/app/services/school/school.service';

@Component({
  selector: 'app-school-details',
  templateUrl: './school-details.component.html',
  styleUrls: ['./school-details.component.css']
})
export class SchoolDetailsComponent implements OnInit {

  school: GetSchoolDto = new GetSchoolDto();
  schoolId: number = 0;
  constructor(private route: ActivatedRoute, private schoolService: SchoolService,
    private _modalService: NgbModal,
    private changeDetectorRef: ChangeDetectorRef) {
    if (this.route.snapshot.paramMap.get('id') !== null) {
      this.schoolId = Number.parseInt(this.route.snapshot.paramMap.get('id') as string);
    }
  }
  ngOnInit(): void {
    if (this.schoolId) {
      this.schoolService.getSchoolById(this.schoolId).subscribe((response: any) => {
        this.school = new GetSchoolDto();
        Object.assign(this.school, response.schoolDto);
      });
    }
  }




}
