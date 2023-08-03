import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import jwt_decode from 'jwt-decode';
import { Role } from 'src/app/enum/role';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private apiUrl = environment.apiUrl;
  private userInfo: DecodedToken | undefined;

  constructor(private http: HttpClient) { }

  authenticate(username: string, password: string): Observable<AuthenticationResponse> {
    const body = { username, password };
    return this.http.post<AuthenticationResponse>(`${this.apiUrl}/authenticate`, body);
  }

  setToken(token: string) {
    localStorage.setItem('authToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) {
      return false;
    }
    this.userInfo = jwt_decode<DecodedToken>(token);
    const decodedToken: DecodedToken = jwt_decode(token);
    if (decodedToken.exp * 1000 < Date.now()) {
      // Token has expired
      this.logout();
      return false;
    }

    return true;
  }

  isAdmin(): boolean {
    if (this.isLoggedIn() && this.userInfo) {
      return this.userInfo.role === Role.Administrator;
    }

    return false;
  }

  logout() {
    localStorage.removeItem('authToken');
    this.userInfo = undefined;
  }
}

interface DecodedToken {
  role: Role;
  sub: string;
  jti: string;
  unique_name: string;
  exp: number;
  iss: string;
  aud: string;
}

interface AuthenticationResponse {
  token: string;
  message: string;
}
