import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school/school.service';
import { SchoolDto } from '../dto/school/school-dto';
import { ErrorResponse } from '../dto/response/error/error-response';
import { PageDetailsDto } from '../dto/utilities/page-details-dto';
import { Router } from '@angular/router';
import { Command } from '../enum/command';
import { ToastService } from '../services/message-popup/toast.service';
import { BaseResponse } from '../dto/response/base-response';
import { firstValueFrom, lastValueFrom } from 'rxjs';
import { ModalService } from '../services/modal/modal.service';

@Component({
  selector: 'app-schools',
  templateUrl: './schools.component.html',
  styleUrls: ['./schools.component.css']
})
export class SchoolsComponent implements OnInit {
  schools: SchoolDto[] = [];
  newSchool: SchoolDto = new SchoolDto();
  pageDetails: PageDetailsDto = new PageDetailsDto();
  constructor(private schoolService: SchoolService,
    private modalService: ModalService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.getSchools(this.pageDetails.skip, this.pageDetails.take);
  }

  getSchools(skip?: number, take?: number): void {
    this.schoolService.getSchools(skip, take).subscribe({
      next: (response: any) => {
        this.pageDetails.totalItems = response.count;
        this.schools = response.schoolsDto.map((schoolData: any) => {
          const school = new SchoolDto();
          Object.assign(school, schoolData);
          return school;
        });
      },
      error: (error: ErrorResponse) => {
        this.toastService.showError(error);
      },
      complete: () => console.info('complete')
    });
  }

  async createSchool(school: SchoolDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.schoolService.createSchool(school));
      this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to create school. Please try again later.');
      throw e;
    }
  }

  async updateSchool(school: SchoolDto): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.schoolService.updateSchool(school));
      this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to update school. Please try again later.');
      throw e;
    }
  }

  async deleteSchool(schoolId: number): Promise<BaseResponse> {
    try {
      const response: BaseResponse = await firstValueFrom(this.schoolService.deleteSchool(schoolId));
      this.getSchools(this.pageDetails.skip, this.pageDetails.take);
      return response;
    } catch (e) {
      this.toastService.showError(e as ErrorResponse);
      this.toastService.showSimpleError('Failed to delete school. Please try again later.');
      throw e;
    }
  }

  async openAddModal(): Promise<void> {
    await this.modalService.openDtoModal(this.newSchool, 'School Modal', this.createSchool.bind(this));
  }
  async deleteSchoolByIdByConfirmModal(schoolReturn: any) {
    let school = schoolReturn as SchoolDto;
    await this.modalService.openConfirmModal(school.id, school.haschildren, school.name, this.deleteSchool.bind(this));
  }
  async updateSchoolByDtoModal(schoolReturn: any) {
    let school = schoolReturn as SchoolDto;
    await this.modalService.openDtoModal(school, 'School Modal', this.updateSchool.bind(this));
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
    let school = schoolReturn as SchoolDto;
    this.router.navigate(['/schools', school.id]);
  }

}