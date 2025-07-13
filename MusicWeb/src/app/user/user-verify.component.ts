import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-user-verify',
  templateUrl: './user-verify.component.html',
  styleUrls: ['./user.component.css']
})
export class UserVerifyComponent implements OnInit {
  public apiURL = "https://localhost:5001/api";
  constructor(private http:HttpClient,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService) {}

  ngOnInit(): void {
  }

  verify ()
  {
    const activationCode = this.route.snapshot.paramMap.get('id');
    this.http.get(this.apiURL + '/UserAuth/VerifyAccount?id='+activationCode)
    .subscribe(()=>{
      // console.warn(userInfo);
      this.toastr.success("You are awesome! I mean it!", "tittle", {
        positionClass: "toast-top-right",
        timeOut: 1000
      });
      setTimeout(() => 
      {
        this.router.navigate(['/music/user-login']);
      },
      1000);
    });
  }

}
