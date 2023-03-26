import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SchoolDto } from '../../dto/school/schooldto' ;

@Injectable({
  providedIn: 'root',
})
export class SchoolService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getSchools(): Observable<SchoolDto[]> {
    return this.http.get<SchoolDto[]>(`${this.apiUrl}/School`);
  }

  createSchool(school: SchoolDto): Observable<SchoolDto> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/School/CreateSchool`, school);
  }

  updateSchool(school: SchoolDto): Observable<SchoolDto> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/School/UpdateSchool`, school);
  }

  deleteSchool(schoolId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/School/DeleteSchool`, schoolId);
  }
}