import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { SchoolsComponent } from './schools/schools.component';
import { StudentsComponent } from './students/students.component';
import { SchoolFormComponent } from './school-form/school-form.component';
import { StudentFormComponent } from './student-form/student-form.component';
import { HttpClientModule } from '@angular/common/http';
import { SchoolService } from './services/school/school.service';
import { StudentService } from './services/student/student.service';
import { AuthenticationService } from './services/authentification/authentication.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableComponent } from './components/table/table.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SchoolsComponent,
    StudentsComponent,
    SchoolFormComponent,
    StudentFormComponent,
    TableComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgxPaginationModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [SchoolService, StudentService, AuthenticationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
