import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  authenticate(username: string, password: string): Observable<AuthenticationResponse> {
    return this.http.get<AuthenticationResponse>(`${this.apiUrl}/authenticate?username=${username}&password=${password}`);
  }

  setToken(token: string) {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  logout() {
    localStorage.removeItem('authToken');
  }
}

interface AuthenticationResponse {
  token: string;
}