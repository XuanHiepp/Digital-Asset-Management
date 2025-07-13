import { Component, OnInit } from '@angular/core';
import { MusicService } from '../shared/music.service';
import { NgForm } from '@angular/forms';
import { MusicItem } from '../shared/music-item.model';
import { Music } from '../shared/music.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { Router } from '@angular/router';
export const musicType = 'musicType';
export const UserID = 'UserID';

@Component({
  selector: 'app-music',
  templateUrl: './music.component.html',
  styleUrls: [
    './music.component.css']
})
export class MusicComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public musicItemList: MusicItem[];
  public userList: User[];
  public keyInfo: Key1;
  public keyServerInfo: Key2;
  public verifyInfo: VerifySign;
  music: Music = new Music();
  selectedLicence: File = null;
  selectedMusic: File = null;
  selectedMusicEnUp: File = null;
  selectedMusicDemo: File = null;
  public userID = localStorage.getItem(UserID);
  public hashedMessageKey1: string;
  public signatureKey1: string;
  public keyType: Number;
  public checkSign: boolean;
  public checkSignServer: boolean;
  public openFormKey2: boolean;
  public pValue: string;
  public fullKey1X: string;
  public fullKey: string;
  public key2: string;
  public hashedMessageKeyServer: string;
  public signatureKeyServer: string;
  public openFormUploadMusic: boolean;
  public openFormAddMusic: boolean;
  public createProgressCheck: boolean;

  public licenceLink: string;
  public musicLink: string;
  public demoLink: string;
  
  constructor(public service: MusicService,
    private http:HttpClient,
    private toastr: ToastrService,
    private router: Router) { 
      this.openFormKey2 = false;
      this.openFormAddMusic = false;
      this.openFormUploadMusic = false;
    }

  ngOnInit(): void {
    this.resetForm();

    this.service.getMusicList().then(res => this.musicItemList = res as MusicItem[]);
    this.getUserList();
  }

  activeFormKey2()
  {
    this.openFormKey2 = true;
  }

  activeFormUploadMusic()
  {
    this.openFormUploadMusic = true;
  }

  activeFormAddMusic()
  {
    this.openFormAddMusic = true;
  }

  resetForm(form?: NgForm)
  {
    if (form = null)
    {
      form.resetForm();
    }
    this.service.formData = {
      name: '',
      title: '',
      album: '',
      publishingYear: '',
      ownerId: 0,
      licenceLink: '',
      musicLink: '',
      demoLink: '',
      mediaLink: '',
      key1: '',
      key2: '',
      fullKey: '',
      creatureType: '',
      ownerType: '',
  
      transactionHash: '',
      contractAddress: '',
    };
    // this.service.musicItems = [];
  }

  onLicenceSelected(event)
  {
    this.selectedLicence = <File>event.target.files[0];

  }

  musicAssetChange(event)
  {
    this.selectedMusicEnUp = <File>event.target.files[0];
  }

  musicAssetEncryptChange(event)
  {
    this.selectedMusic = <File>event.target.files[0];
  }

  musicAssetDemoChange(event)
  {
    this.selectedMusicDemo = <File>event.target.files[0];
  }

  musicTypeChange(value: any)
  {
    localStorage.setItem(musicType, value);
    // console.log(localStorage.getItem(musicType));
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

  addKey2(keyServerInfoForm)
  {
    this.keyServerInfo = keyServerInfoForm;
    this.keyServerInfo.pValue = this.pValue;
    this.keyServerInfo.fullKey1X = this.fullKey1X;
    this.keyServerInfo.keyType = 1;
    // console.log(this.keyServerInfo);
    this.http.post(this.rootUrl + '/Music/SignDataKey2Server', this.keyServerInfo)
    .subscribe(res => {
      // console.warn(res['sign']);
      this.hashedMessageKeyServer = res["hashMess"];
      this.signatureKeyServer = res["sign"];
      this.fullKey = res["fullKey"];
      this.key2 = res["key2"];
    });
  }

  signKeyServer(signInfoForm)
  {
    this.verifyInfo = signInfoForm;
    this.verifyInfo.keyType = 0;
    this.verifyInfo.userID = Number.parseInt(this.userID);
    // console.log(this.keyInfo);
    this.http.post(this.rootUrl + '/Music/VerifySignature', this.verifyInfo)
    .subscribe(res => {
      // console.warn(res['checkSign']);
      this.checkSignServer = res["checkSign"];
    });
  }

  encryptMusic()
  {
    var music = localStorage.getItem(musicType);
    if (Number(music) == 0)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusic, this.selectedMusic.name);
      musicAsset.append('password', this.fullKey);
      console.log(musicAsset.get('password'));
      this.http.post(this.rootUrl + '/UploadMusicAsset/EncryptFileMusic', musicAsset)
      .subscribe(res=>{
        this.toastr.success("Mã hóa file thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
        setTimeout(() => {
          console.log('hide');
          this.activeFormAddMusic();
        }, 3000);
      });
    }
    else if (Number(music) == 1)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusic, this.selectedMusic.name);
      musicAsset.append('password', this.fullKey);
      this.http.post(this.rootUrl + '/UploadMusicAsset/EncryptFileMusic', musicAsset)
      .subscribe(res=>{
        this.toastr.success("Mã hóa file thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
        setTimeout(() => {
          console.log('hide');
          this.activeFormAddMusic();
        }, 3000);
      });
    }
    else if (Number(music) == 2)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusic, this.selectedMusic.name);
      musicAsset.append('password', this.fullKey);
      this.http.post(this.rootUrl + '/UploadMusicAsset/EncryptFileMusic', musicAsset)
      .subscribe(res=>{
        this.toastr.success("Mã hóa file thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
        setTimeout(() => {
          console.log('hide');
          this.activeFormAddMusic();
        }, 3000);
      });
    }
  }

  addMusic(musicInfo)
  {
    // let musicInfo = this.service.formData;
    // this.service.addMusicService(musicInfo);
    // console.warn(musicInfo);

    this.service.formData = musicInfo;
    const licence = new FormData();
    licence.append('myFile', this.selectedLicence, this.selectedLicence.name);
    this.http.post(this.rootUrl + '/UploadMusicAsset/licence', licence)
    .subscribe(res=>{
      this.licenceLink = res['licenceLink'];
      this.service.formData.licenceLink = this.licenceLink;
    });

    var music = localStorage.getItem(musicType);

    if (Number(music) == 0)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusicEnUp, this.selectedMusicEnUp.name);
      this.http.post(this.rootUrl + '/UploadMusicAsset/lyrics', musicAsset)
      .subscribe(res=>{
        this.musicLink = res['lyricsLink'];
        this.service.formData.musicLink = this.musicLink;
      });
    }
    else if (Number(music) == 1)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusicEnUp, this.selectedMusicEnUp.name);
      this.http.post(this.rootUrl + '/UploadMusicAsset/audio', musicAsset)
      .subscribe(res=>{
        this.musicLink = res['audioLink'];
        this.service.formData.musicLink = this.musicLink;
      });
    }
    else if (Number(music) == 2)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusicEnUp, this.selectedMusicEnUp.name);
      this.http.post(this.rootUrl + '/UploadMusicAsset/video', musicAsset)
      .subscribe(res=>{
        this.musicLink = res['videoLink'];
        this.service.formData.musicLink = this.musicLink;
      });
    }

    if (Number(music) == 1)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusicDemo, this.selectedMusicDemo.name);
      this.http.post(this.rootUrl + '/UploadMusicAsset/audio', musicAsset)
      .subscribe(res=>{
        this.demoLink = res['audioLink'];
        this.service.formData.demoLink = this.demoLink;
      });
    }
    else if (Number(music) == 2)
    {
      const musicAsset = new FormData();
      musicAsset.append('myFile', this.selectedMusicDemo, this.selectedMusicDemo.name);
      this.http.post(this.rootUrl + '/UploadMusicAsset/video', musicAsset)
      .subscribe(res=>{
        this.demoLink = res['videoLink'];
        this.service.formData.demoLink = this.demoLink;
      });
    }
   
    this.service.formData.key1 = this.fullKey1X;
    this.service.formData.key2 = this.key2;
    this.service.formData.fullKey = this.fullKey;
    this.createProgressCheck = false;
    
    setTimeout(() => 
    {
      this.http.post(this.rootUrl + '/Music', this.service.formData)
      .subscribe(()=>{
        this.toastr.success("Thêm nhạc số thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 4000
        });
          this.createProgressCheck = true;
        setTimeout(() => 
        {
          this.router.navigate(['/music/user-seller/'+this.userID]);
        },
        4000);
      });
    },
    40000);
  }

  getUserList(){
    return this.http.get(this.rootUrl + '/User').toPromise().then(res => this.userList = res as User[]);
  }

  onSubmit(form: NgForm){
    
  }

  getMusicList()
  {
    this.service.getMusicList().then(res => this.musicItemList = res as MusicItem[]);
  }

}


export class User {
  userID: string;
  firstName: string;
  lastName: string;
}

export class Key1 {
  key1X: Number;
  keyType: Number;
  userID: Number;
}

export class Key2 {
  pValue: string;
  fullKey1X: string;
  key2X: Number;
  keyType: Number;
}

export class VerifySign {
  hashedMessage: string;
  signature: string;
  keyType: Number;
  userID: Number;
}