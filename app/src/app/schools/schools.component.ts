import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school/school.service';
import { SchoolDto } from '../dto/school/school-dto';
import { GetSchoolDto } from '../dto/school/getschool-dto';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from '../modals/confirm-modal/confirm-modal.component';
import { ErrorResponse } from '../dto/response/error/error-response';
import { DtoModalComponent } from '../modals/dto-modal/dto-modal.component';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
import { Router } from '@angular/router';
import { Command } from '../enum/command';
import { ToastService } from '../services/message-popup/toast.service';
import { BaseResponse } from '../dto/response/base-response';
import { firstValueFrom, lastValueFrom } from 'rxjs';

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
    private changeDetectorRef: ChangeDetectorRef,
    private toastService: ToastService) { }

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
    },
    (error : ErrorResponse) => {
      this.toastService.showError(error);
    });
  }

  async createSchool(school: SchoolDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom (this.schoolService.createSchool(school));
      this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to create school. Please try again later.');
      throw e; 
    }
  }

  async updateSchool(school: SchoolDto): Promise<BaseResponse>  {
    try {
      const response: BaseResponse = await firstValueFrom (this.schoolService.updateSchool(school));
      this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to update school. Please try again later.');
      throw e; 
    }
  }

  deleteSchool(schoolId: number): void {
    this.schoolService.deleteSchool(schoolId).subscribe({
      next: (response: BaseResponse) => {
        this.toastService.showSuccess(response.message);
        this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      },
      error: (e: ErrorResponse) => {
        this.toastService.showError(e);
        this.toastService.showSimpleError('Failed to delete school. Please try again later.');
      },
      complete: () => console.info('complete')
    });
  }

  async openAddModal(): Promise<void>  {
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'School Modal';
    modalRef.componentInstance.dto = this.newSchool;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe(async (receivedSchool: SchoolDto) => {
      const result = await this.createSchool(receivedSchool);
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
    this.getSchools(this.pageDetails.skip, this.pageDetails.take);
    this.changeDetectorRef.detectChanges();
  }
  handleActionCommands(event: { dto: any, command: Command }) {
    switch (event.command) {
      case Command.Read:
        this.navigateToSchoolById(event.dto);
        break;
      case Command.Update:
        this.updateSchoolByDtoModal(event.dto);
        break;
      case Command.Delete:
        this.deleteSchoolByIdByConfirmModal(event.dto);
        break;
      default:
        console.log(`Error : ${event.command}`);
        break;
    }
  }
  navigateToSchoolById(schoolReturn: any) {
    let school = schoolReturn as GetSchoolDto;
    this.router.navigate(['/schools', school.id]);
  }
  deleteSchoolByIdByConfirmModal(schoolReturn: any) {
    let school = schoolReturn as GetSchoolDto;
    const modalRef = this._modalService.open(ConfirmModalComponent);
    modalRef.componentInstance.name = school.name;
    if (school.haschildren) {
      const errorMessage: ErrorResponse = {
        message: '',
        success: false,
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
      this.toastService.showError(error as ErrorResponse);
    });
  }
  async updateSchoolByDtoModal(schoolReturn: any) {
    let school = schoolReturn as SchoolDto;
    const modalRef = this._modalService.open(DtoModalComponent);
    modalRef.componentInstance.title = 'School Modal';
    modalRef.componentInstance.dto = school;
    modalRef.componentInstance.passBackDTOToMainComponent.subscribe(async (receivedSchool: SchoolDto) => {
      const result = await this.updateSchool(receivedSchool);
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