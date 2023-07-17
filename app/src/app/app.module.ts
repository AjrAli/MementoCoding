import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { SchoolsComponent } from './schools/schools.component';
import { StudentsComponent } from './students/students.component';
import { SchoolFormComponent } from './forms/school/school-form.component';
import { StudentFormComponent } from './forms/student/student-form.component';
import { HttpClientModule } from '@angular/common/http';
import { SchoolService } from './services/school/school.service';
import { StudentService } from './services/student/student.service';
import { AuthenticationService } from './services/authentification/authentication.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableComponent } from './components/table/table.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { HomeComponent } from './home/home.component';
import { MenuComponent } from './menu/menu.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ConfirmModalComponent } from './modals/confirm-modal/confirm-modal.component';
import { DtoModalComponent } from './modals/dto-modal/dto-modal.component';
import { SchoolDetailsComponent } from './schools/school-details/school-details.component';
import { StudentDetailsComponent } from './students/student-details/student-details.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SchoolsComponent,
    StudentsComponent,
    SchoolFormComponent,
    StudentFormComponent,
    TableComponent,
    HomeComponent,
    MenuComponent,
    DtoModalComponent,
    ConfirmModalComponent,
    DtoModalComponent,
    SchoolDetailsComponent,
    StudentDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxPaginationModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BrowserAnimationsModule,
    FontAwesomeModule
  ],
  providers: [SchoolService, StudentService, AuthenticationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
