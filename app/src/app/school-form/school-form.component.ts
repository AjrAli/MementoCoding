import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../dto/school/schooldto';
import { SchoolService } from '../services/school/school.service';
@Component({
  selector: 'app-school-form',
  templateUrl: './school-form.component.html',
  styleUrls: ['./school-form.component.css']
})
export class SchoolFormComponent {

  school: SchoolDto = {
    id: 0,
    name: '',
    adress: '',
    town: '',
    description: ''
  };

  constructor(private router: Router, private schoolService: SchoolService) {}

  addSchool(): void {
    this.schoolService.createSchool(this.school).subscribe(
      () => {
        this.router.navigate(['/schools']);
      },
      (error) => {
        console.error('Add school error:', error);
        alert('Failed to add school. Please try again later.');
      }
    );
  }
}