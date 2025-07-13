import { Component } from '@angular/core';
export const UserID = 'UserID';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public userID = localStorage.getItem(UserID);
  //Clear storage after specific time
  title = 'MusicWeb';
}
