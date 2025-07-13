import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { MusicItem } from '../shared/music-item.model';
import {formatDate} from '@angular/common';

@Component({
  selector: 'app-origin-music',
  templateUrl: './origin-music.component.html',
  styleUrls: ['./origin-music.component.css']
})
export class OriginMusicComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public musicItem: MusicItem;
  public firstName: String;
  public lastName: String;
  public musicTransact: MusicTransaction;
  public fromAddr: String;
  public fromAddrFull: String;
  public toAddr: String;
  public toAddrFull: String;
  public idTransfer: String;
  public currentDate: Date;
  public dateToNow: number;

  constructor(private http:HttpClient,
    private toastr: ToastrService,
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getMusicInfo();
    this.getMusicTFInfo();
  }

  getMusicInfo()
  {
    const transactionHash = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/Music/GetMusicForOrigin/'+transactionHash)
    .subscribe(
      res=>{
        this.musicItem = res as MusicItem;
        // this.musicItem.id = this.musicItem.id.split("-")[0];
        this.http.get(this.rootUrl + '/User/GetUserInfo/'+ res["ownerId"])
      .subscribe(
        resNew=>{
          this.firstName = resNew["firstName"];
          this.lastName = resNew["lastName"];
        });
      });
  }

  getdateDistanceCalculate(year: number, month: number, day: number) {
    if (month < 3) {
        year--;
        month += 12;
    }
    return 365*year + year/4 - year/100 + year/400 + (153*month - 457)/5 + day - 306;
  }

  getMusicTFInfo()
  {
    this.currentDate = new Date();
    const transactionHash = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/Music/GetMusicTFWithTransactionHash/'+transactionHash)
    .subscribe(
      res=>{
        this.musicTransact = res as MusicTransaction;
        if (this.musicTransact.transactionHash.length > 20)
        {
          this.musicTransact.transactionHashLink = this.musicTransact.transactionHash.substring(0, 15)+'...';
        }
        const currentDateChange = Number.parseInt(formatDate(this.currentDate, 'dd', 'en-US'));
        const currentMonthChange = Number.parseInt(formatDate(this.currentDate, 'MM', 'en-US'));
        const currentYearChange = Number.parseInt(formatDate(this.currentDate, 'yyyy', 'en-US'));
        

        const dateCreatedChange = Number.parseInt(formatDate(this.musicTransact.dateCreated.toString(), 'dd', 'en-US'));
        const monthCreatedChange = Number.parseInt(formatDate(this.musicTransact.dateCreated.toString(), 'MM', 'en-US'));
        const yearCreatedChange = Number.parseInt(formatDate(this.musicTransact.dateCreated.toString(), 'yyyy', 'en-US'));

        this.dateToNow = this.getdateDistanceCalculate(currentYearChange, currentMonthChange, currentDateChange) - this.getdateDistanceCalculate(yearCreatedChange, monthCreatedChange, dateCreatedChange);      
        this.dateToNow = Number.parseInt(this.dateToNow.toFixed(0))
      });

    this.http.get(this.rootUrl + '/Music/GetMusicTransferForOrigin/'+transactionHash)
    .subscribe(
      res=>{
        this.fromAddr = res["fromAddress"].substring(0, 20)+'...';
        this.toAddr = res["toAddress"].substring(0, 20)+'...';
        this.fromAddrFull = res["fromAddress"];
        this.toAddrFull = res["toAddress"];
        this.idTransfer = res["id"].split("-")[0];
      });

      
  }
}

export class MusicTransaction {
  id: String;
  musicId: String;
  buyerId: String;
  fromId: String;
  toId: String;
  tranType: String;
  fanType: String;
  dateStart: Number;
  dateEnd: Number;
  transactionHash: String;
  transactionHashLink: String;
  dateCreated: String;
  amountValue: Number;
  isConfirmed: boolean;
  
  buyerName: String;
}
