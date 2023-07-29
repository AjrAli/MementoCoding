import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SchoolDto } from '../../dto/school/school-dto';
import { SchoolService } from '../../services/school/school.service';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, FormGroup } from '@angular/forms';
import { BaseFormComponent } from '../base-form.component';
@Component({
  selector: 'app-school-form',
  templateUrl: './school-form.component.html',
  styleUrls: ['./school-form.component.css']
})
export class SchoolFormComponent extends BaseFormComponent implements OnInit {

  school: SchoolDto = new SchoolDto();
  title: string = 'Add School';
  constructor(private router: Router,
    private schoolService: SchoolService) {
    super();
  }


  ngOnInit(): void {
    this.school = this.dto as SchoolDto;
    if (this.school?.name) {
      this.title = 'Update School';
    }
    this.baseForm = new FormGroup({
      id: new FormControl(this.school?.id),
      name: new FormControl(this.school?.name),
      adress: new FormControl(this.school?.adress),
      town: new FormControl(this.school?.town),
      description: new FormControl(this.school?.description)
    });
  }

  addSchool(): void {
    this.school = this.baseForm.value;
    this.passBackDTO.emit(this.school);
  }
}