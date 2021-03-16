import { Component, OnInit } from '@angular/core';
import { SignalRService } from '@app/_services/signal-r.service';
import { first } from 'rxjs/operators';
import { User } from '../_models/user';
import { AuthenticationService } from '../_services/authentication.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  loading = false;
  user: User;
  userFromApi: User;

  constructor(
    private userService: UserService,
    private authenticationService: AuthenticationService,
    public signalRService: SignalRService
) {
    this.user = this.authenticationService.userValue;
}

  ngOnInit(): void {
    this.loading = true;
        this.userService.getById(this.user.id).pipe(first()).subscribe(user => {
            this.loading = false;
            this.userFromApi = user;
        });

      this.signalRService.startConnection();
  }

  ngOnDestroy() : void {
    this.signalRService.stopConnection();
  }
}
