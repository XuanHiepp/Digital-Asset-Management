import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";
export const UserID = 'UserID';
@Component({
  selector: 'app-user-buyer-item',
  templateUrl: './user-buyer-item.component.html',
  styleUrls: ['./user-buyer-item.component.css']
})
export class UserBuyerItemComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public user: User;
  public transac: Transaction;
  public userList: User[];
  public fromAddress: string;
  public licenceType: string;
  public durationType: string;
  public buyerId: Number;
  public createProgressCheck: boolean;
  public key2: string;
  public ownerId: Number;
  constructor(private http:HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getUserList();
    this.getMusicInfo();
  }


  getUserList(){
    return this.http.get(this.rootUrl + '/User').toPromise().then(res => this.userList = res as User[]);
  }

  getMusicInfo(){
    const musicId = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/Music/GetMusicWithId/'+musicId)
    .subscribe(
      res=>{
        this.key2 = res['key2'];
        this.ownerId = res['ownerId'];
        this.http.get(this.rootUrl + '/User/GetUserInfo/' + this.ownerId)
        .subscribe(
          res=>{
            this.user = res as User;
          });
      });
  }


  buyerChange(value: any)
  {
    this.buyerId = value;
    this.http.get(this.rootUrl + '/User/GetUserInfo/'+value)
    .subscribe(
      res=>{
        this.fromAddress = res['ownerAddress'];
      });
  }

  licenceTypeChange(value: any)
  {
    this.licenceType = value;
  }

  durationTypeChange(value: any)
  {
    this.durationType = value;
  }

  excuteTransact(transactInfo)
  {
    const musicId = this.route.snapshot.paramMap.get('id');
    var licence_type = this.licenceType;
    var duration_type = this.durationType;
    this.createProgressCheck = false;
    if (Number(licence_type) == 0)
    {
      this.transac = transactInfo;
      this.transac.fromUserId = this.fromAddress;
      this.transac.toUserId = this.user.ownerAddress;
      this.transac.buyerId = this.buyerId;
      this.transac.musicId = musicId;
      this.transac.tranType = "ForSale";

      switch(Number(duration_type)) { 
        case 0: { 
           this.transac.duration = 7;
           break; 
        } 
        case 1: { 
          this.transac.duration = 14;
           break; 
        } 
        case 2: { 
          this.transac.duration = 30;
          break; 
        } 
        case 3: { 
          this.transac.duration = 90;
          break; 
        } 
        case 4: { 
          this.transac.duration = 180;
          break; 
        }
        case 5: { 
          this.transac.duration = 365;
          break; 
        } 
      } 

      this.transac.amountValue = 1;
      this.transac.isPermanent = false;
      this.transac.key2 = this.key2;

      this.http.post(this.rootUrl + '/MusicAssetTransfers/CreateTransferAsync', this.transac)
      .subscribe(()=>{
        this.createProgressCheck = true;
        this.toastr.success("Giao dịch thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
        setTimeout(() => 
        {
          this.router.navigate(['/music/user-seller/'+1]);
        },
        3000);
      });
      // console.log(this.transac);
    }
    else
    {
      this.transac = transactInfo;
      this.transac.musicId = musicId;
      this.transac.fromUserId = this.fromAddress;
      this.transac.toUserId = this.user.ownerAddress;
      this.transac.buyerId = this.buyerId;
      this.transac.tranType = "ForOwnerShip";

      this.transac.duration = 0;

      this.transac.amountValue = 1;
      this.transac.isPermanent = true;
      this.transac.key2 = this.key2;

      this.http.post(this.rootUrl + '/MusicAssetTransfers/CreateLicenceTrans', this.transac)
      .subscribe(()=>{
        this.createProgressCheck = true;
        this.toastr.success("Giao dịch thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
        setTimeout(() => 
        {
          this.router.navigate(['/music/user-seller/'+1]);
        },
        3000);
      });
    }
  }
}

export class User {
  userID: String;
  firstName: String;
  lastName: String;
  emailID: String; 
  ownerAddress: String;
}

export class Transaction {
  musicId: String;
  fromUserId: String;
  toUserId: String;
  tranType: String;
  fanType: String;
  duration: Number;
  amountValue: Number;
  isPermanent: boolean;
  buyerId: Number;
  key2: string;
}
