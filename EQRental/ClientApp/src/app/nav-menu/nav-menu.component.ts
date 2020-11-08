import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthorizeService } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isAuthenticated = false;
  logoUrl = environment.apiUrl.substring(0, environment.apiUrl.lastIndexOf('api/')) + 'Images/Logo/EQ_Rental_logo.png'

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  constructor(private authorizeService: AuthorizeService) {
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe(
      (isAuthenticated) => {
        this.isAuthenticated = isAuthenticated;
      }
    );
  }
}
