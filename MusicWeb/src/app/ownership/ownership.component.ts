import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { ActivatedRoute } from '@angular/router';
export const UserID = 'UserID';
@Component({
  selector: 'app-ownership',
  templateUrl: './ownership.component.html',
  styleUrls: ['./ownership.component.css']
})
export class OwnershipComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public keyInfo: Key1;
  public userID = localStorage.getItem(UserID);
  public hashedMessageKey1: String;
  public signatureKey1: String;
  public pValue: String;
  public fullKey1X: string;
  public checkSign: boolean;
  public verifyInfo: VerifySign;
  public musicId: String;
  public key2: string;
  public createKey: CreateKey;
  public blobName: string;
  public keyCreateCheck: boolean;
  public fullKeyOld: string;
  public fullKeyNew: string;
  public musicNewLink : string;
  public musicInfoUpdatefullKeyNew: MusicInfoUpdate;
  public musicChaneOwnerShip: MusicChaneOwnerShip;
  public updateCheck: boolean;
  public updateProgressCheck: boolean;
  public updateMusicInfoCheck: boolean;
  public inheritUserId: Number;
  public ownerId: Number;
  constructor(private http:HttpClient,
    private toastr: ToastrService,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.getMusicInfo();
    this.keyCreateCheck = false;
    this.updateCheck = false;
    this.updateMusicInfoCheck = false;
    const infoMusic = this.route.snapshot.paramMap.get('id');
    if (Number.parseInt(infoMusic.split("_")[1]) != 0)
    {
      this.inheritUserId = Number.parseInt(infoMusic.split("_")[1]);
    }
    else this.inheritUserId = 0;
  }

  getMusicInfo()
  {
    const infoMusic = this.route.snapshot.paramMap.get('id');
    if (Number.parseInt(infoMusic.split("_")[1]) == 0)
    {
      this.musicId = infoMusic.split("_")[0];
      this.http.get(this.rootUrl + '/Music/GetMusicWithId/'+this.musicId)
        .subscribe(res =>{
            this.fullKeyOld = res["fullKey"];
            this.blobName = res["musicLink"].split("//")[1].split("/")[2];
            this.ownerId = res["ownerId"];
            this.http.get(this.rootUrl + '/Music/'+this.musicId+'_'+this.ownerId+'/contract-address')
              .subscribe(res =>{
                  this.key2 = res["key2"];
              });
        });
    }
    else
    {
      this.http.get(this.rootUrl + '/MusicAssetTransfers/GetTransfer/'+infoMusic.split("_")[0])
      .subscribe(res =>{
          this.musicId = res["musicId"];
          this.http.get(this.rootUrl + '/Music/GetMusicWithId/'+this.musicId)
          .subscribe(res =>{
              this.fullKeyOld = res["fullKey"];
              this.blobName = res["musicLink"].split("//")[1].split("/")[2];  
              this.ownerId = res["ownerId"];          
          });
      });

      this.http.get(this.rootUrl + '/MusicAssetTransfers/'+infoMusic.split("_")[0]+'/contract-address')
        .subscribe(res =>{
            this.key2 = res["key2"];
        });
    }
    
    if (this.key2 == null)
    {
      this.http.get(this.rootUrl + '/MusicAssetTransfers/GetTransfer/'+infoMusic.split("_")[0])
      .subscribe(res =>{
        this.http.get(this.rootUrl + '/Music/GetMusicWithId/'+res['musicId'])
        .subscribe(resNew =>{
            this.key2 = resNew["key2"];
        });
      });
    }
  }

  changeOwnerShip()
  {
    const assetUpdate = new FormData();
    assetUpdate.append('blobName', this.blobName);
    assetUpdate.append('old_password', this.fullKeyOld.toString());
    assetUpdate.append('new_password', this.fullKeyNew.toString());

    this.updateProgressCheck = false;
    this.http.post(this.rootUrl + '/UploadMusicAsset/CopyFileEncryptAndUploadAudioOwnerShip', assetUpdate)
    .subscribe(
      res=>{
        this.updateCheck = true;
        this.updateProgressCheck = true;
        this.musicNewLink = res["musicLink"];
        this.toastr.success("Mã hóa nhạc số thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 2000
        });
      });
  }

  updateInfoForMusic(ownerInfo)
  {
    const infoMusic = this.route.snapshot.paramMap.get('id');
    
    this.musicInfoUpdatefullKeyNew = ownerInfo;
    if (this.inheritUserId != 0)
    {
      this.musicInfoUpdatefullKeyNew.ownerId = this.inheritUserId;
    }
    this.musicInfoUpdatefullKeyNew.musicId = this.musicId.toString();
    this.musicInfoUpdatefullKeyNew.key1 = this.fullKey1X;
    this.musicInfoUpdatefullKeyNew.fullKey = this.fullKeyNew;
    this.musicInfoUpdatefullKeyNew.musicLink = this.musicNewLink;

    this.http.post(this.rootUrl + '/Music/UpdateKey', this.musicInfoUpdatefullKeyNew)
    .subscribe(
    res=>{

    });

    if (Number.parseInt(infoMusic.split("_")[1]) == 0)
    {
      this.musicChaneOwnerShip = new MusicChaneOwnerShip();
      this.musicChaneOwnerShip.id = this.musicId;
      this.musicChaneOwnerShip.ownerId = this.musicInfoUpdatefullKeyNew.ownerId;
      this.musicChaneOwnerShip.musicLink = this.musicInfoUpdatefullKeyNew.musicLink;
      this.http.post(this.rootUrl + '/Music/UpdateChangeOwnerShipAsync', this.musicChaneOwnerShip)
      .subscribe(
        res=>{
          this.updateMusicInfoCheck = true;
          this.toastr.success("Cập nhật nhạc số thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
        });
    }
    else
    {
      this.musicChaneOwnerShip = new MusicChaneOwnerShip();
      this.musicChaneOwnerShip.id = this.musicId;
      this.musicChaneOwnerShip.ownerId = Number.parseInt(infoMusic.split("_")[1]);
      this.musicChaneOwnerShip.musicLink = this.musicInfoUpdatefullKeyNew.musicLink;
      this.http.post(this.rootUrl + '/Music/UpdateChangeOwnerShipAsync', this.musicChaneOwnerShip)
      .subscribe(
        res=>{
          this.updateMusicInfoCheck = true;
          this.toastr.success("Cập nhật nhạc số thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 2000
          });
        });
    }
  }

  createNewKey()
  {
    this.createKey = new CreateKey();
    this.createKey.key1X = Number.parseFloat(this.fullKey1X);
    this.createKey.key2X = Number.parseFloat(this.key2);
    this.http.post(this.rootUrl + '/Music/CreateFullKey', this.createKey)
    .subscribe(
      res=>{
        this.keyCreateCheck = true;
        // console.log(res['fullKey']);
        this.fullKeyNew = res['fullKey'];
        this.toastr.success("Tạo khóa mới thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 2000
        });
      });
  }

  addKey1(keyInfoForm)
  {
    this.keyInfo = keyInfoForm;
    this.keyInfo.keyType = 1;
    this.keyInfo.userID = Number.parseInt(this.userID);
    // console.log(this.keyInfo);
    this.http.post(this.rootUrl + '/Music/SignDataKey1', this.keyInfo)
    .subscribe(res => {
      // console.warn(res['sign']);
      this.hashedMessageKey1 = res["hashMess"];
      this.signatureKey1 = res["sign"];
      this.pValue = res["pValue"];
      this.fullKey1X = res["key1"];
    });
  }

  signKey1(signInfoForm)
  {
    this.verifyInfo = signInfoForm;
    this.verifyInfo.keyType = 1;
    this.verifyInfo.userID = Number.parseInt(this.userID);
    // console.log(this.keyInfo);
    this.http.post(this.rootUrl + '/Music/VerifySignature', this.verifyInfo)
    .subscribe(res => {
      // console.warn(res['checkSign']);
      this.checkSign = res["checkSign"];
    });
  }

}

export class Key1 {
  key1X: Number;
  keyType: Number;
  userID: Number;
}

export class VerifySign {
  hashedMessage: String;
  signature: String;
  keyType: Number;
  userID: Number;
}

export class CreateKey {
  key1X: Number;
  key2X: Number;
}

export class MusicInfoUpdate {
  musicId: string;
  key1: string;
  fullKey: string;
  ownerId: Number;
  musicLink: string;
}

export class AssetUpdate {
  blobName: string;
  old_password: string;
  new_password: string;
}

export class MusicChaneOwnerShip {
  id: String;
  ownerId: Number;
  musicLink: String;
}