import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  template: ''
})
export class BaseFormComponent  {
  
  @Input()
  dto: any;
  @Output() passBackDTO = new EventEmitter<any>();
  baseForm!: FormGroup;
  constructor() { }

  clearForm() {
    this.baseForm.reset();
    Object.keys(this.baseForm.controls).forEach(controlName => {
      this.baseForm.get(controlName)?.patchValue('');
    });
  }


}
