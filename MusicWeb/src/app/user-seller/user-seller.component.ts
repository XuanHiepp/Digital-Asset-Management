import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MusicItem } from '../shared/music-item.model';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";
export const UserTypeCheck = 'UserTypeCheck';

@Component({
  selector: 'app-user-seller',
  templateUrl: './user-seller.component.html',
  styleUrls: ['./user-seller.component.css']
})
export class UserSellerComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public user: User;
  public ownerInfo: OwnerInfo;
  public ownerShipInfo: OwnerShipInfo;
  public musicItemList: MusicItem[];
  public musicShareOwnerShipList: MusicItem[];
  public ownerAddr: String;
  public ownerPrKey: String;
  public userTypeCheck : String;
  public totalRecords: number;
  public page: number = 1;
  public userList: User[];
  public musicId: String;
  constructor(private http:HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    const userID = this.route.snapshot.paramMap.get('id');
    const userIDLogin = localStorage.getItem("UserID");
    if (Number.parseInt(userIDLogin) == Number.parseInt(userID))
    {
      this.getUserInfo();
      this.getMusicListDefault();
      this.loadOwnerInfo();
      this.getUserList();
    }
  }

  logOut()
  {
    localStorage.removeItem("UserID");
    setTimeout(() => 
          {
            this.router.navigate(['/music/statistic']);
          },
          2000);
  }

  getUserInfo(){
    const userID = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/User/GetUserInfo/'+userID).toPromise().then(res => this.user = res as User);
  }

  loadOwnerInfo(){
    const userID = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/UserAuth/GetOwnerInfo/'+userID)
    .subscribe(res =>{
      this.ownerAddr = res['ownerAddress'];
      this.ownerPrKey = res['ownerPrivateKey'];
    });    
  }

  loadMusicId(value)
  {
    this.musicId = value;
  }

  shareOwnerShip(ownerShipInfo)
  {
    const userID = this.route.snapshot.paramMap.get('id');
    this.ownerShipInfo = ownerShipInfo;
    this.ownerShipInfo.musicId = this.musicId;
    this.http.post(this.rootUrl + '/Music/CreateMusicOwnerShip', this.ownerShipInfo)
      .subscribe(res=>{
        if (res["checkExist"] == true)
        {
          this.toastr.error("Bản quyền đã được chia sẻ cho account chỉ định", "Error", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
        }
        else
        {
          this.toastr.success("Chia sẻ bản quyền thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
        }
      });
  }

  updateOwnerInfo(ownerInfo)
  {
    this.ownerInfo = ownerInfo;

    if (this.ownerInfo.ownerAddress != "" && this.ownerInfo.ownerPrivateKey == "")
    {
      const userID = this.route.snapshot.paramMap.get('id');
      this.ownerInfo.userID = Number(userID);
      this.http.post(this.rootUrl + '/UserAuth/UpdateOwnerAddress', this.ownerInfo)
      .subscribe(res=>{
          this.toastr.success("Cập nhật Address thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
          setTimeout(() => 
          {
            this.router.navigate(['/music/user-seller/'+userID]);
          },
          2000);
      });
    }
    else if (this.ownerInfo.ownerAddress == "" && this.ownerInfo.ownerPrivateKey != "")
    {
      const userID = this.route.snapshot.paramMap.get('id');
      this.ownerInfo.userID = Number(userID);
      this.http.post(this.rootUrl + '/UserAuth/UpdateOwnerPrivateKey', this.ownerInfo)
      .subscribe(res=>{
          this.toastr.success("Cập nhật Private Key thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
          setTimeout(() => 
          {
            this.router.navigate(['/music/user-seller/'+userID]);
          },
          2000);
      });
    }
    else if (this.ownerInfo.ownerAddress != "" && this.ownerInfo.ownerPrivateKey != "")
    {
      const userID = this.route.snapshot.paramMap.get('id');
      this.ownerInfo.userID = Number(userID);
      this.http.post(this.rootUrl + '/UserAuth/UpdateOwnerAddress', this.ownerInfo)
      .subscribe(res=>{
         
      });

      this.ownerInfo.userID = Number(userID);
      this.http.post(this.rootUrl + '/UserAuth/UpdateOwnerPrivateKey', this.ownerInfo)
      .subscribe(res=>{
          this.toastr.success("Cập nhật thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
          setTimeout(() => 
          {
            this.router.navigate(['/music/user-seller/'+userID]);
          },
          2000);
      });
    }
    else
    {
      this.toastr.error("Thông tin bị thiếu", "Error", {
        positionClass: "toast-top-right",
        timeOut: 2000
      });
    }
    // console.log(this.ownerInfo);
  }

  SetUserType()
  {
    sessionStorage.setItem(UserTypeCheck, "seller");
    this.userTypeCheck = sessionStorage.getItem(UserTypeCheck);
    this.getMusicList();
  }

  SetUserTypeBuyer()
  {
    sessionStorage.setItem(UserTypeCheck, "buyer");
    this.userTypeCheck = sessionStorage.getItem(UserTypeCheck);
    this.getMusicList();
  }

  getMusicListDefault()
  {
    sessionStorage.setItem(UserTypeCheck, "seller");
    this.userTypeCheck = sessionStorage.getItem(UserTypeCheck);
    this.getMusicList();
  }

  getUserList(){
    return this.http.get(this.rootUrl + '/User').toPromise().then(res => this.userList = res as User[]);
  }

  getMusicList()
  {
    // console.log(this.userType);
    if (this.userTypeCheck == "seller")
    {
      const userID = this.route.snapshot.paramMap.get('id'); 
    
      this.http.get(this.rootUrl + '/User/GetMusicAssetWithUser/'+userID)
      .subscribe(res =>{
        this.musicItemList = res as MusicItem[];
        let count = 0;
        var index;
        for(let i = 0; i < this.musicItemList.length; i++)
        {
          if (this.musicItemList[i].creatureType == "Lyrics")
          {
            this.musicItemList[i].lyricsCheck = true;
          }
          else if (this.musicItemList[i].creatureType == "Audio")
          {
            this.musicItemList[i].audioCheck = true;
          }
          else if (this.musicItemList[i].creatureType == "MV")
          {
            this.musicItemList[i].mvCheck = true;
          }
        }

        this.http.get(this.rootUrl + '/Music/GetMusicShareOwnerShip/'+userID)
        .subscribe(res =>{
          this.musicShareOwnerShipList = res as MusicItem[];
          for(let i = 0; i < this.musicShareOwnerShipList.length; i++)
          {
            if (this.musicShareOwnerShipList[i].creatureType == "Lyrics")
            {
              this.musicShareOwnerShipList[i].lyricsCheck = true;
            }
            else if (this.musicShareOwnerShipList[i].creatureType == "Audio")
            {
              this.musicShareOwnerShipList[i].audioCheck = true;
            }
            else if (this.musicShareOwnerShipList[i].creatureType == "MV")
            {
              this.musicShareOwnerShipList[i].mvCheck = true;
            }
            
            if(this.musicItemList.find(item => item.id == this.musicShareOwnerShipList[i].id) === undefined)
            {
              this.musicItemList.push(this.musicShareOwnerShipList[i]);
            }
          }

          this.totalRecords = this.musicItemList.length;
          // console.log(this.musicItemList)
        });
      }); 
    }
    else if (this.userTypeCheck == "buyer")
    {
      const userID = this.route.snapshot.paramMap.get('id');
      this.http.get(this.rootUrl + '/User/GetMusicAssetWithUserBuyer/'+userID)
      .subscribe(res =>{
        this.musicItemList = res as MusicItem[];
        for(let i = 0; i < this.musicItemList.length; i++)
          {
            if (this.musicItemList[i].creatureType == "Lyrics")
            {
              this.musicItemList[i].lyricsCheck = true;
            }
            else if (this.musicItemList[i].creatureType == "Audio")
            {
              this.musicItemList[i].audioCheck = true;
            }
            else if (this.musicItemList[i].creatureType == "MV")
            {
              this.musicItemList[i].mvCheck = true;
            }
            this.musicItemList[i].musicLink = this.musicItemList[i].musicLink.split("//")[1].split("/")[2];
            if (this.musicItemList[i].mediaLink != "")
            {
              this.musicItemList[i].mediaLink = btoa(this.musicItemList[i].mediaLink.toString());
            }
          }
          this.totalRecords = this.musicItemList.length;
          // console.log(this.musicItemList);
      });
    }
  }
}

export class User {
  userID: String;
  firstName: String;
  lastName: String;
  emailID: String; 
}

export class OwnerInfo {
  userID: Number;
  ownerAddress: String;
  ownerPrivateKey: String;
}

export class OwnerShipInfo {
  musicId: String;
  userId: Number;
}