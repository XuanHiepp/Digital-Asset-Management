import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";

export const UserID = 'UserID';
export const UserType = 'UserType';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user.component.css']
})
export class UserLoginComponent implements OnInit {
  readonly rootUrl = "https://localhost:5001/api";
  public loginData: UserLogin;
  userID: string;
  userType: Number;
  constructor(private http:HttpClient,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    
  }
  
  login (loginData)
  {
    loginData.rememberMe = true;
    this.http.post(this.rootUrl + '/UserAuth/Login', loginData)
    .subscribe(res =>{
      this.userID = res['userID'] as string;

      localStorage.setItem(UserID, this.userID);
      
      this.userType = res['userType'] as Number;
      localStorage.setItem(UserType, this.userType.toString());
      console.log(localStorage['UserID']);
      if (localStorage['UserID'] != 0)
      {
        this.toastr.success("Đăng nhập thành công", "Success", {
          positionClass: "toast-top-right",
          timeOut: 1000
        });
        setTimeout(() => 
        {
          this.router.navigate(['/music/statistic']);
        },
        1000);
      }
      else
      {
        this.toastr.error("Tài khoản hoặc mật khẩu không đúng!", "Error", {
          positionClass: "toast-top-right",
          timeOut: 1000
        });
      }
    });
  }

}

export class UserLogin {
  emailID: String;
  password: String;
  rememberMe: boolean;
}



