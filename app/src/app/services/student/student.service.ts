import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StudentDto } from '../../dto/student/student-dto';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getStudents(skip?: number, take?: number): Observable<StudentDto[]> {
    let paginateParams: string = '';
    if (take) {
      paginateParams = `/${skip}/${take}`;
    }
    return this.http.get<StudentDto[]>(`${this.apiUrl}/Student${paginateParams}`);
  }
  createStudent(student: StudentDto): Observable<StudentDto> {
    return this.http.post<StudentDto>(`${this.apiUrl}/Student/CreateStudent`, student);
  }

  updateStudent(student: StudentDto): Observable<StudentDto> {
    return this.http.post<StudentDto>(`${this.apiUrl}/Student/UpdateStudent`, student);
  }

  deleteStudent(studentId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/Student/DeleteStudent`, studentId);
  }
}