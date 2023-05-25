import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../../dto/school/schooldto';
import { SchoolService } from '../../services/school/school.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-school-form',
  templateUrl: './school-form.component.html',
  styleUrls: ['./school-form.component.css']
})
export class SchoolFormComponent implements OnInit{

  @Input()
  school!: SchoolDto;
  @Output() passBackSchool = new EventEmitter<SchoolDto>();
  schoolForm!: FormGroup;
  constructor(private router: Router, 
    private schoolService: SchoolService) { }


  ngOnInit(): void {
    this.schoolForm = new FormGroup({
      name: new FormControl(this.school.name),
      adress: new FormControl(this.school.adress),
      town: new FormControl(this.school.town),
      description: new FormControl(this.school.description)
    });
    this.schoolForm.valueChanges.subscribe((formValues) => {
      this.school.name = formValues.name;
      this.school.adress = formValues.adress;
      this.school.town = formValues.town;
      this.school.description = formValues.description;
    });
  }

  addSchool(): void {
    this.passBackSchool.emit(this.school);
    this.schoolForm.reset();
  }
}