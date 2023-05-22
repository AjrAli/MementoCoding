import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent {
  constructor(private router: Router) { }

  isConnected() {
    return localStorage.getItem("authToken");
  }
  logout(): void {
    localStorage.removeItem('authToken');
    this.router.navigate(['/home']);
  }

}
