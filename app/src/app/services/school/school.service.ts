import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SchoolDto } from '../../dto/school/school-dto';
import { ODataQueryDto } from 'src/app/dto/utilities/odata-query-dto';

@Injectable({
  providedIn: 'root',
})
export class SchoolService {
  private readonly apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getSchoolById(schoolId: number): Observable<any> {
    return this.http.get<SchoolDto>(`${this.apiUrl}/SchoolQuery/${schoolId}`);
  }

  getSchools(query?: ODataQueryDto): Observable<any> {

    const queryString = query?.toString();

    // Create HttpParams object and set the queryString as the 'params' option
    let params = new HttpParams();
    if (queryString) {
      queryString.split('&').forEach(param => {
        const [key, value] = param.split('=');
        params = params.set(key, value);
      });
    }
    return this.http.get<SchoolDto[]>(`${this.apiUrl}/SchoolQuery`, { params });
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