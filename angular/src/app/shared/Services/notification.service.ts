import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private hubConnection: signalR.HubConnection;
  BaseApiUrl = environment.apis.default.url;
  constructor(private toastr: ToastrService) {
    this.startConnection();
  }

  private async startConnection(): Promise<void> {
    // Initialize SignalR connection
    if (this.hubConnection) {
      return; // Connection already established
    }
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.BaseApiUrl+'/signalr-notifications') // backend URL
      .withAutomaticReconnect()
      .build();

    // Receive messages
    this.hubConnection.on('ReceiveMessage', (user: string, message: string) => {
      // You can use a Subject/Toastr here
      this.toastr.success(message, 'New Notification');
    });
    if (this.hubConnection.state === signalR.HubConnectionState.Disconnected)
      await this.hubConnection.start();
  }

  // Optional: send a message
  public async sendMessage(user: string, message: string): Promise<void> {
    if (this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      await this.hubConnection.start(); // Attempt reconnect
    }
    return this.hubConnection.invoke('SendMessage', user, message)
      .catch(err => console.error(err));
  }
}