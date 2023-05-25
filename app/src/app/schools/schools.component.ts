import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school/school.service';
import { SchoolDto } from '../dto/school/schooldto' ;
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SchoolModalComponent } from '../modals/school/school-modal/school-modal.component';

@Component({
  selector: 'app-schools',
  templateUrl: './schools.component.html',
  styleUrls: ['./schools.component.css']
})
export class SchoolsComponent implements OnInit {
  schools: SchoolDto[] = [];
  newSchool: SchoolDto = {
    id: 0,
    name: '',
    adress: '',
    town: '',
    description: ''
  };
  constructor(private schoolService: SchoolService, 
    private _modalService: NgbModal,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getSchools();
  }

  getSchools(): void {
    this.schoolService.getSchools().subscribe((schools: any) => {
      this.schools = schools.schoolsDto;
    });
  }

  createSchool(school: SchoolDto): void {
    this.schoolService.createSchool(school).subscribe({
      next: () => {
        this.getSchools();
      },
      error: (e) => {
        console.error('Add school error:', e);
        alert('Failed to add school. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  updateSchool(school: SchoolDto): void {
    this.schoolService.updateSchool(school).subscribe({
      next: () => {
        this.getSchools();
      },
      error: (e) => {
        console.error('Update school error:', e);
        alert('Failed to update school. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  deleteSchool(schoolId: number): void {
    this.schoolService.deleteSchool(schoolId).subscribe({
      next: () => {
        this.getSchools();
      },
      error: (e) => {
        console.error('Delete school error:', e);
        alert('Failed to delete school. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  open() {
    const modalRef = this._modalService.open(SchoolModalComponent);
    modalRef.componentInstance.school = this.newSchool;
    modalRef.componentInstance.passBackSchoolToMainSchool.subscribe((receivedSchool: SchoolDto) => {
      this.createSchool(receivedSchool);
      this.changeDetectorRef.detectChanges();
      modalRef.close();
    });
  }
}