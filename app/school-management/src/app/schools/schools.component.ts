import { Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school/school.service';
import { SchoolDto } from '../dto/school/schooldto' ;

@Component({
  selector: 'app-schools',
  templateUrl: './schools.component.html',
  styleUrls: ['./schools.component.css']
})
export class SchoolsComponent implements OnInit {
  schools: SchoolDto[] = [];

  constructor(private schoolService: SchoolService) { }

  ngOnInit(): void {
    this.getSchools();
  }

  getSchools(): void {
    this.schoolService.getSchools().subscribe((schools: any) => {
      this.schools = schools.schoolsDto;
    });
  }

  createSchool(school: SchoolDto): void {
    this.schoolService.createSchool(school).subscribe(() => {
      this.getSchools();
    });
  }

  updateSchool(school: SchoolDto): void {
    this.schoolService.updateSchool(school).subscribe(() => {
      this.getSchools();
    });
  }

  deleteSchool(schoolId: number): void {
    this.schoolService.deleteSchool(schoolId).subscribe(() => {
      this.getSchools();
    });
  }
}