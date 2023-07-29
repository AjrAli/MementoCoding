import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StudentDto } from '../../dto/student/student-dto';
import { GetStudentDto } from 'src/app/dto/student/getstudent-dto';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  getStudentById(studentId: number): Observable<any> {
    return this.http.get<GetStudentDto>(`${this.apiUrl}/StudentQuery/${studentId}`);
  }
  
  getStudents(skip?: number, take?: number): Observable<any> {
    let paginateParams: string = '';
    if (take) {
      paginateParams = `/${skip}/${take}`;
    }
    return this.http.get<GetStudentDto[]>(`${this.apiUrl}/StudentQuery${paginateParams}`);
  }
  createStudent(student: StudentDto): Observable<any> {
    return this.http.post<StudentDto>(`${this.apiUrl}/StudentCommand/CreateStudent`, student);
  }

  updateStudent(student: StudentDto): Observable<any> {
    return this.http.post<StudentDto>(`${this.apiUrl}/StudentCommand/UpdateStudent`, student);
  }

  deleteStudent(studentId: number): Observable<any> {
    return this.http.post<void>(`${this.apiUrl}/StudentCommand/DeleteStudent`, studentId);
  }
}