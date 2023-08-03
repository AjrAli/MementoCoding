import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { SchoolsComponent } from './schools/schools.component';
import { StudentsComponent } from './students/students.component';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { SchoolDetailsComponent } from './schools/school-details/school-details.component';
import { StudentDetailsComponent } from './students/student-details/student-details.component';
import { SearchComponent } from './search/search/search.component';
import { AuthAdminGuard } from './guards/auth.admin.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'home', component: HomeComponent},
  { path: 'login', component: LoginComponent},
  { path: 'schools', component: SchoolsComponent, canActivate: [AuthAdminGuard] },
  { path: 'schools/:id', component: SchoolDetailsComponent, canActivate: [AuthGuard] },
  { path: 'students', component: StudentsComponent, canActivate: [AuthAdminGuard] },
  { path: 'students/:id', component: StudentDetailsComponent, canActivate: [AuthGuard] },
  { path: 'search', component: SearchComponent, canActivate: [AuthGuard] },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
