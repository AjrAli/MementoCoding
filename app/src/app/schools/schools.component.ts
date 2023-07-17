import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school/school.service';
import { SchoolDto } from '../dto/school/school-dto';
import { GetSchoolDto } from '../dto/school/getschool-dto';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { ErrorResponse } from '../dto/error/error-response';
import { DtoModalComponent } from '../modals/dto-modal/dto-modal.component';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
import { Router } from '@angular/router';

@Component({
  selector: 'app-schools',
  templateUrl: './schools.component.html',
  styleUrls: ['./schools.component.css']
})
export class SchoolsComponent implements OnInit {
  schools: GetSchoolDto[] = [];
  newSchool: SchoolDto = new SchoolDto();
  pageDetails: PageDetailsDto = new PageDetailsDto();
  constructor(private schoolService: SchoolService,
    private _modalService: NgbModal,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getSchools(this.pageDetails.skip, this.pageDetails.take);
  }

  getSchools(skip?: number, take?: number): void {
    this.schoolService.getSchools(skip, take).subscribe((response: any) => {
      this.pageDetails.totalItems = response.count;
      this.schools = response.schoolsDto.map((schoolData: any) => {
        const school = new GetSchoolDto();
        Object.assign(school, schoolData);
        return school;
      });
    });
  }

  createSchool(school: SchoolDto): void {
    this.schoolService.createSchool(school).subscribe({
      next: () => {
        this.getSchools(this.pageDetails.skip, this.pageDetails.take);
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
        this.getSchools(this.pageDetails.skip, this.pageDetails.take);
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
        this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e) => {
        console.error('Delete school error:', e);
        alert('Failed to delete school. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  openAddModal() {
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'School Modal';
    modalRef.componentInstance.dto = this.newSchool;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe((receivedSchool: SchoolDto) => {
      this.createSchool(receivedSchool);
      this.changeDetectorRef.detectChanges();
      modalRef.close();
    });
  }
  handleNextPage(result: any) {
    this.pageDetails.skip = result.skip;
    this.pageDetails.take = result.take;
    this.getSchools(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }
  handleReturnSchoolToGet(schoolReturn: any) {
    let school = schoolReturn as GetSchoolDto;
    this.router.navigate(['/schools', school.id]);   
  }
  handleReturnSchoolToDelete(schoolReturn: any) {
    let school = schoolReturn as GetSchoolDto;
    const modalRef = this._modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.name = school.name;
    if (school.haschildren) {
      const errorMessage: ErrorResponse = {
        message: '',
        validationErrors: []
      };
      modalRef.componentInstance.errorMessage = errorMessage;
      modalRef.componentInstance.errorMessage.message = `(Impossible to delete ${school.name}, some students are linked to it!)`;
    }
    modalRef.result.then((result) => {
      if (result === 'yes' && !school.haschildren) {
        this.deleteSchool(school.id);
      } else {
        console.log('Action annulée');
      }
    }).catch((error) => {
      // La boîte de dialogue a été fermée avec une erreur
      console.log('Erreur :', error);
    });
  }
  handleReturnSchoolToUpdate(schoolReturn: any) {
    let school = schoolReturn as SchoolDto;
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'School Modal';
    modalRef.componentInstance.dto = school;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe((receivedSchool: SchoolDto) => {
      this.updateSchool(receivedSchool);
      this.changeDetectorRef.detectChanges();
      modalRef.close();
    });
  }
}