import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MusicItem } from '../shared/music-item.model';
declare const loadVideo: any;
declare const loadAudioPlayer: any;

@Component({
  selector: 'app-user-buyer',
  templateUrl: './user-buyer.component.html',
  styleUrls: ['./user-buyer.component.css']
})
export class UserBuyerComponent implements OnInit {
  public apiURL = "https://localhost:5001/api";
  public musicItemList: MusicItem[];
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    loadVideo();
    loadAudioPlayer();
    this.getMusicList();
  }

  getMusicList(){
    return this.http.get(this.apiURL + '/Music').toPromise()
    .then(res => this.musicItemList = res as MusicItem[]);;
  }
}
