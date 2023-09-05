import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './services/authentification/authentication.service';
import { UrlHistoryService } from './services/shared/url-history.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'schools-management';
  constructor(private router: Router,
    private authenticationService: AuthenticationService,
    private urlHistoryService: UrlHistoryService) {
  }

}
