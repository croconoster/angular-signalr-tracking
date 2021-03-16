import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
//import { SignalRService } from './services/signal-r.service';

import { AuthenticationService } from './_services/authentication.service';
import { User } from './_models/user';
import { Role } from './_models/role';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  user: User

  // , public signalRService: SignalRService
  constructor(private authenticationService: AuthenticationService) {
    this.authenticationService.user.subscribe((x: any) => this.user = x);
  }

  get isAdmin() {
    return this.user && this.user.role === Role.Admin;
  }

  logout() {
    this.authenticationService.logout();
    window.location.reload();
  }

  ngOnInit(): void {

    //this.signalRService.startConnection();
  }
}
