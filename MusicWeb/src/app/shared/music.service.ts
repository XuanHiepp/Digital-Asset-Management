import { Injectable } from '@angular/core';
import { Music } from './music.model';
import { MusicItem } from './music-item.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";
export const UserID = 'UserID';
@Injectable({
  providedIn: 'root'
})
export class MusicService {
  public formData: Music;
  public musicItems: MusicItem[];
  public apiURL = "https://localhost:5001/api";
  public userID = sessionStorage.getItem(UserID);
  constructor(private http:HttpClient,
    private router: Router,
    private toastr: ToastrService) { }

  getMusicList(){
    return this.http.get(this.apiURL + '/Music').toPromise();
  }

  addMusicService(formData){
    this.http.post(this.apiURL + '/Music', formData)
    .subscribe(()=>{
      this.toastr.success("Thêm nhạc số thành công", "Success", {
        positionClass: "toast-top-right",
        timeOut: 4000
      });
        sessionStorage.removeItem('musicLink');
        sessionStorage.removeItem('demoLink');
        sessionStorage.removeItem('licenceLink');
        sessionStorage.removeItem('musicType');
      setTimeout(() => 
      {
        this.router.navigate(['/music/user-seller/'+this.userID]);
      },
      4000);
      // console.warn(formData);
    });
  }
}
