import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: ['./music.component.css']
})
export class CreateOrEditMusicModalComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public name = '';

  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.refreshList();
  }

  refreshList(){
    this.http.get(this.rootUrl + '/Music')
    .toPromise()
    .then(res => this.name = res as string)
  }

}
