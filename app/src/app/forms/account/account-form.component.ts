import { Component, OnInit } from '@angular/core';
import { BaseFormComponent } from '../base-form.component';
import { AccountDto } from 'src/app/dto/account/account-dto';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-account-form',
  templateUrl: './account-form.component.html',
  styleUrls: ['./account-form.component.css']
})
export class AccountFormComponent extends BaseFormComponent implements OnInit {
  account: AccountDto = new AccountDto();
  title: string = 'Create an Account';
  constructor() {
    super();
  }


  ngOnInit(): void {
    this.baseForm = new FormGroup({
      username: new FormControl(this.account?.username, Validators.required),
      password: new FormControl(this.account?.password, [Validators.required, Validators.minLength(4)]),
      firstName: new FormControl(this.account?.firstName, Validators.required),
      lastName: new FormControl(this.account?.lastName, Validators.required),
      email: new FormControl(this.account?.email, [Validators.required, Validators.email]),
      emailConfirmed: new FormControl(true)
    });
  }

  createAccount(): void {
    this.account = this.baseForm.value;
    this.passBackDTO.emit(this.account);
  }

}
