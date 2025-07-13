import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MusicItem } from '../shared/music-item.model';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-user-seller-item',
  templateUrl: './user-seller-item.component.html',
  styleUrls: ['./user-seller-item.component.css']
})
export class UserSellerItemComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public musicTransact: MusicTransaction[];
  public licenceMusicTransact: MusicTransaction[];
  public approveLicenceTransact: ApproveLicenceTransact;
  public dateStart: Number;
  public dateEnd: Number;
  public amountValue: String;
  public approveType: String;

  constructor(private http:HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getTransacMusic();
    this.getLicenceTransacMusic();
  }

  getTransacMusic(){
    const musicId = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/MusicAssetTransfers/GetMusicTransfers/'+musicId)
    .subscribe(
      res=>{
        this.musicTransact = res as MusicTransaction[];
        for(let i = 0; i < this.musicTransact.length; i++)
        {
          this.musicTransact[i].transactionHashLink = this.musicTransact[i].transactionHash;
          if (this.musicTransact[i].transactionHash.length > 20)
          {
            this.musicTransact[i].transactionHash = this.musicTransact[i].transactionHash.substring(0, 25)+'...';
          }
          this.http.get(this.rootUrl + '/User/GetUserInfo/'+ this.musicTransact[i].buyerId)
          .subscribe(
            res=>{
              this.musicTransact[i].buyerName = res["firstName"] + "_" + res["lastName"];
            });
        }
        
      });
  }

  getLicenceTransacMusic(){
    const musicId = this.route.snapshot.paramMap.get('id');
    this.http.get(this.rootUrl + '/MusicAssetTransfers/GetLicenceMusicTransfers/'+musicId)
    .subscribe(
      res=>{
        this.licenceMusicTransact = res as MusicTransaction[];
        for(let i = 0; i < this.licenceMusicTransact.length; i++)
        {
          this.licenceMusicTransact[i].transactionHashLink = this.licenceMusicTransact[i].transactionHash;
          if (this.licenceMusicTransact[i].transactionHash.length > 20)
          {
            this.licenceMusicTransact[i].transactionHash = this.licenceMusicTransact[i].transactionHash.substring(0, 25)+'...';
          }
          this.http.get(this.rootUrl + '/User/GetUserInfo/'+ this.licenceMusicTransact[i].buyerId)
          .subscribe(
            res=>{
              this.licenceMusicTransact[i].buyerName = res["firstName"] + "_" + res["lastName"];
            });
        }
      });
  }

  aprroveTypeChange(value: any)
  {
    this.approveType = value;
    this.http.get(this.rootUrl + '/MusicAssetTransfers/GetTransfer/'+value)
    .subscribe(
      res=>{
        this.dateStart = res["dateStart"] * 1000;
        this.dateEnd = res["dateEnd"] * 1000;
        this.amountValue = res["amountValue"];
      });
  }

  approveTransaction()
  {
    const musicAssetTranferId = this.approveType;
    this.http.get(this.rootUrl + '/MusicAssetTransfers/GetTransfer/'+musicAssetTranferId)
    .subscribe(
      res=>{
        this.approveLicenceTransact = new ApproveLicenceTransact();
        this.approveLicenceTransact.id = musicAssetTranferId;
        this.approveLicenceTransact.musicId = res["musicId"];
        this.http.post(this.rootUrl + '/MusicAssetTransfers/UpdateLicenceTransAsync',  this.approveLicenceTransact)
        .subscribe(()=>{
          this.toastr.success("Giao dịch bản quyền thành công", "Success", {
            positionClass: "toast-top-right",
            timeOut: 3000
          });
          setTimeout(() => 
          {
            this.router.navigate(['/music/user-seller/'+1]);
          },
          3000);
        });
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

export class ApproveLicenceTransact {
  id: String;
  musicId: String;
}