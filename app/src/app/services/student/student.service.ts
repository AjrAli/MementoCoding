import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { StudentDto } from '../../dto/student/student-dto';
import { ODataOptions } from 'src/app/enum/odata-options';
import { OrderByChoice } from 'src/app/enum/orderby-choice';
import { StudentProperties } from 'src/app/enum/student-properties';
import { ODataQueryDto } from 'src/app/dto/utilities/odata-query-dto';

@Injectable({
  providedIn: 'root',
})
export class StudentService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  
  getStudentById(studentId: number): Observable<any> {
    return this.http.get<StudentDto>(`${this.apiUrl}/StudentQuery/${studentId}`);
  }
  
  getStudents(query?:ODataQueryDto): Observable<any> {

    const queryString = query?.toString();

    // Create HttpParams object and set the queryString as the 'params' option
    let params = new HttpParams();
    if (queryString) {
      queryString.split('&').forEach(param => {
        const [key, value] = param.split('=');
        params = params.set(key, value);
      });
    } 
    return this.http.get<StudentDto[]>(`${this.apiUrl}/StudentQuery`, {params});
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