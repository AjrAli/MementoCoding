import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SchoolDto } from '../../dto/school/school-dto' ;

@Injectable({
  providedIn: 'root',
})
export class SchoolService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getSchoolById(schoolId: number): Observable<any> {
    return this.http.get<SchoolDto>(`${this.apiUrl}/SchoolQuery/${schoolId}`);
  }

  getSchools(skip?:number, take?:number): Observable<any> {
      let paginateParams: string = '';
      if(take){
        paginateParams = `/${skip}/${take}`;
      }
    return this.http.get<SchoolDto[]>(`${this.apiUrl}/SchoolQuery${paginateParams}`);
  }

  createSchool(school: SchoolDto): Observable<any> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/SchoolCommand/CreateSchool`, school);
  }

  updateSchool(school: SchoolDto): Observable<any> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/SchoolCommand/UpdateSchool`, school);
  }

  deleteSchool(schoolId: number): Observable<any> {
    return this.http.post<void>(`${this.apiUrl}/SchoolCommand/DeleteSchool`, schoolId);
  }
}