import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../../dto/school/school-dto';
import { SchoolService } from '../../services/school/school.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
@Component({
  selector: 'app-school-form',
  templateUrl: './school-form.component.html',
  styleUrls: ['./school-form.component.css']
})
export class SchoolFormComponent implements OnInit {

  @Input()
  dto: any;
  school: SchoolDto = new SchoolDto();
  @Output() passBackDTO = new EventEmitter<any>();
  schoolForm!: FormGroup;
  title: string = 'Add School';
  constructor(private router: Router,
    private schoolService: SchoolService) { }


  ngOnInit(): void {
    this.school = this.dto as SchoolDto;
    if (this.school?.name) {
      this.title = 'Update School';
    }
    this.schoolForm = new FormGroup({
      id: new FormControl(this.school.id),
      name: new FormControl(this.school.name),
      adress: new FormControl(this.school.adress),
      town: new FormControl(this.school.town),
      description: new FormControl(this.school.description)
    });
  }

  addSchool(): void {
    this.school = this.schoolForm.value;
    this.clearForm();
    this.passBackDTO.emit(this.school);
  }
  clearForm() {
    this.schoolForm.reset();
    Object.keys(this.schoolForm.controls).forEach(controlName => {
      this.schoolForm.get(controlName)?.patchValue('');
    });
  }
}