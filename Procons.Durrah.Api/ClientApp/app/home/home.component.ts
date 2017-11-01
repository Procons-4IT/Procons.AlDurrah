import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { Observable, ObservableInput } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/do';
import { TranslateService } from '@ngx-translate/core'

import { ResetPasswordParams, KnetPayment } from '../Models/ApiRequestType';

import { ApiService } from '../Services/ApiService';
import { UtilityService } from '../Services/UtilityService';

declare var $;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  isLoggedIn = false;
  logInType = '';

  public paymentModalText: string = "";
  public paymentParams: KnetPayment = {
    PaymentID: "",
    Postdate: "",
    Result: "",
    TranID: "",
    Auth: "",
    Ref: "",
    TrackID: ""
  };
  public amount: string;

  public resetPassModalLoading: boolean = false;
  public resetPassInputError: string = "";
  public resetPassModalText: string = "";

  public resetParams: ResetPasswordParams = {
    EmailAddress: "",
    Password: "",
    ValidationId: ""
  };

  public loadingPayment: boolean = false;
  constructor(public myApi: ApiService,
    public utility: UtilityService,
    public activeRouter: ActivatedRoute,
    public router: Router,
    public translate: TranslateService
  ) { }

  ngOnInit() {
    this.myApi.onUserLoggedIn()
      .subscribe(isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
      });
    this.myApi.onUserTypeLoggedIn().subscribe(loginType=>{
      this.logInType = loginType;
    });
    this.handlePaymentRoute();
    this.handleConfirmEmailRoute();
    this.handlePasswordResetRoute();
  }


  handleConfirmEmailRoute() {
    

    this.activeRouter.data
      .filter((data, idx) => { return data.isConfirmEmail; })
      .do(x => {
        $('#modalIncomingPayment').modal('toggle');
        this.loadingPayment = true;
        this.paymentModalText = "Confirming Email Please Wait!";

      })
      .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter) })
      .mergeMap(params => { return this.myApi.confirmEmail(params) })
      .subscribe(x => {

        this.loadingPayment = false;
        if (x) {
          this.paymentModalText = 'Email Confirmed!';
          var stateObj = { foo: "bar" };
          history.replaceState(stateObj, "page 3", "home");


        } else {
          this.paymentModalText = 'Invalid Email!';
        }
      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }
  // Ex.http://localhost:4200/paymentid=1394338331172790&result=not%20captured&postdate=1006&tranid=7630570331172790&auth=&ref=727911110230&trackid=9670186
  //Ex. http://localhost:4200/paymentconfirmation?paymentid=5904845091172790&result=not%20captured&postdate=1006&tranid=2624949101172790&auth=&ref=727911110228&trackid=6358289

  handlePaymentRoute() {
    

    this.activeRouter.data
      .filter((data, idx) => { return data.isPayment; })
      .do(x => {
        $('#modalIncomingPayment').modal('toggle');
        this.loadingPayment = true;
        this.paymentModalText = "Proccesing Payment Please Wait!";

      })
      .mergeMap(x => { return this.utility.getKnetUrlProperties(this.activeRouter) })
      .do(x => { this.paymentParams = x; })
      .mergeMap(params => { return this.myApi.createIncomingPayment(params) })
      .subscribe(x => {
        
        this.loadingPayment = false;
        this.paymentModalText = x.udF1;
        this.amount = x && x.amount;
        var stateObj = { foo: "bar" };
        history.replaceState(stateObj, "page 3", "home");

      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }

  handlePasswordResetRoute() {
    

    this.activeRouter.data
      .filter((data, idx) => { return data.isPasswordReset; })
      .do(x => {
        $('#modalResetPass').modal('toggle');
      })
      .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter) })
      .subscribe(x => {
        
        this.resetParams.EmailAddress = x.Email;
        this.resetParams.ValidationId = x.ValidationId;
      }, onError => {
        this.translate.get('error.resetPassword').subscribe(errorMessage => {
          this.resetPassModalText = errorMessage;
        });
      });
  }

  ResetPassword(password: string, confirmPassword: string) {
    if (password !== confirmPassword) {
      this.resetPassInputError = "Error Passwords not matching";
    } else {
      this.resetPassModalLoading = true;
      this.resetParams.Password = password;

      this.myApi.resetPassword(this.resetParams)
        .subscribe(isReset => {
          this.resetPassModalLoading = false;
          if (isReset) {
            
            this.resetPassModalText = 'Password was Reset!';
            setTimeout(x => {
              this.router.navigate(['/home']);
            }, 3000);

          }
        }, onError => {
          this.resetPassModalLoading = false;
          this.translate.get('error.resetPassword').subscribe(errorMessage => {
            this.resetPassModalText = errorMessage;
          })

        });
    }
  }




}
