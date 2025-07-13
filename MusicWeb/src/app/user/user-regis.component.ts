import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-user-regis',
  templateUrl: './user-regis.component.html',
  styleUrls: ['./user.component.css']
})
export class UserRegisComponent implements OnInit {
  public userInfo: User;
  public apiURL = "https://localhost:5001/api";
  constructor(private http:HttpClient,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(userInfo)
  {
    this.http.post(this.apiURL + '/UserAuth/Registration', userInfo)
    .subscribe(()=>{
      // console.warn(userInfo);
      this.toastr.success("Link xác nhận đã được gửi vào Email", "Success", {
        positionClass: "toast-top-right",
        timeOut: 1000
      });
      setTimeout(() => 
      {
        this.router.navigate(['/music/statistic']);
      },
      1000);
      
    });
  }

}

export class User {
  firstName: String;
  lastName: String;
  emailID: String;
  dateOfBirth: Date;
  userType: Number;
  password: String;
  confirmPassword: String;
  resetPasswordCode: String;
}
