import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SchoolDto } from '../../dto/school/school-dto' ;
import { GetSchoolDto } from 'src/app/dto/school/getschool-dto';

@Injectable({
  providedIn: 'root',
})
export class SchoolService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getSchoolById(schoolId: number): Observable<GetSchoolDto> {
    return this.http.get<GetSchoolDto>(`${this.apiUrl}/SchoolQuery/${schoolId}`);
  }

  getSchools(skip?:number, take?:number): Observable<GetSchoolDto[]> {
      let paginateParams: string = '';
      if(take){
        paginateParams = `/${skip}/${take}`;
      }
    return this.http.get<GetSchoolDto[]>(`${this.apiUrl}/SchoolQuery${paginateParams}`);
  }

  createSchool(school: SchoolDto): Observable<SchoolDto> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/SchoolCommand/CreateSchool`, school);
  }

  updateSchool(school: SchoolDto): Observable<SchoolDto> {
    return this.http.post<SchoolDto>(`${this.apiUrl}/SchoolCommand/UpdateSchool`, school);
  }

  deleteSchool(schoolId: number): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/SchoolCommand/DeleteSchool`, schoolId);
  }
}