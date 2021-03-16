import { Injectable } from '@angular/core';
import * as signalR from "@aspnet/signalr";
import { environment } from '@environments/environment';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  public usersData: any;

  constructor(private authenticationService : AuthenticationService) { }

  private hubConnection : signalR.HubConnection;

  public startConnection = () => {
      this.hubConnection = new signalR.HubConnectionBuilder()
          .configureLogging(signalR.LogLevel.Debug)
          .withUrl(`${environment.apiUrl}/pagevisit`, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            accessTokenFactory: () => this.authenticationService.userValue.token ?? ''
          })
          .build();

      this.hubConnection
          .start()
          .then(() => console.log('Connection started'))
          //.then(() => this.broadcastData())
          .then(() => this.addBroadcastDataListener())
          .catch(err => console.log('Error while starting connection: ' + err))
  }

  public stopConnection = () => {
    this.hubConnection.stop();
  }

  public broadcastData = () => {
    this.hubConnection.invoke('GetUser', "Some Data I Sent")
    .catch(err => console.error(err));
  }

  public addBroadcastDataListener = () => {
    this.hubConnection.on('broadcastUsers', (data) => {
      this.usersData = data;
      console.log(data);
    })
  }
}
