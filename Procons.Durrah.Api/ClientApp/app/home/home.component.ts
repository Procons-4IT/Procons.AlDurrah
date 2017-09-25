import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router'
import { Observable, ObservableInput } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/do';
import { TranslateService } from '@ngx-translate/core'

import { ResetPasswordParams } from '../Models/ApiRequestType';

import { ApiService } from '../Services/ApiService';
import { UtilityService } from '../Services/UtilityService';

declare var $;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public paymentModalText: string = "";

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
    let userLoggedIn$ = this.myApi.onUserLoggedIn();
    console.log('user init state ', userLoggedIn$.getValue());
    userLoggedIn$.subscribe(isLoggedIn => {
      console.log('user state', isLoggedIn);
    });
    this.handlePaymentRoute();
    this.handleConfirmEmailRoute();
    this.handlePasswordResetRoute();
  }


  handleConfirmEmailRoute() {
    console.log('## Checking if ConfirmEmailRoute');

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
        console.log('Recieved X', x);
        this.loadingPayment = false;
        if (x) {
          this.paymentModalText = 'Email Confirmed!';
          this.router.navigate(['/home']);
        } else {
          this.paymentModalText = 'Invalid Email!';
        }
      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }
  handlePaymentRoute() {
    console.log('## Checking if PaymentRoute');

    this.activeRouter.data
      .filter((data, idx) => { return data.isPayment; })
      .do(x => {
        $('#modalIncomingPayment').modal('toggle');
        this.loadingPayment = true;
        this.paymentModalText = "Proccesing Payment Please Wait!";

      })
      .mergeMap(x => { return this.utility.getKnetUrlProperties(this.activeRouter) })
      .mergeMap(params => { return this.myApi.createIncomingPayment(params) })
      .subscribe(x => {
        console.log('Recieved X', x);
        this.loadingPayment = false;
        this.paymentModalText = x;
      }, onError => {
        this.loadingPayment = false;
        this.paymentModalText = 'Something Went Wrong!'
      });

  }

  handlePasswordResetRoute() {
    console.log('## Checking if PasswordResetRoute');

    this.activeRouter.data
      .filter((data, idx) => { return data.isPasswordReset; })
      .do(x => {
        $('#modalResetPass').modal('toggle');
      })
      .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter) })
      .subscribe(x => {
        console.log('Recieved ResetParams! ', x);
        this.resetParams.EmailAddress = x.Email;
        this.resetParams.ValidationId = x.ValidationId;
      }, onError => {
        this.translate.get('error.resetPassword').subscribe(errorMessage => {
          this.resetPassModalText = errorMessage;
        })
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
            console.log('## Password was Reset! ');
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

  @ViewChild('files') el: ElementRef;

  uploadFile() {
    console.log('click!');
    let files = this.el.nativeElement.files;
    if (files && files[0]) {
      const formData = new FormData();
      for (var i = 0; i < files.length; i++) {
          formData.append("Photo", files[i], files[i].name);
          formData.append("Passport", files[i], files[i].name);
      }
      formData.append('Age', '43');
      formData.append('BirthDate', '01-01-2000');
      formData.append('CivilId', '124542154215');
      formData.append('Code', 'code');
      formData.append('Education', '1');
      formData.append('Gender', '1');
      formData.append('Height', '180');
      formData.append('Language', '1');
      formData.append('MaritalStatus', '1');
      formData.append('Nationality', '1');
      formData.append('PassportExpDate', '01-01-2000');
      formData.append('PassportIssDate', '01-01-2000');
      formData.append('PassportNumber', '01-01-2000');
      formData.append('Religion', '1');
      formData.append('Video', '01-01-2000');
      formData.append('BirthDate', '01-01-2000');
      formData.append('Weight', '80');

      console.log('sending FormData', formData);
      this.myApi.uploadFile(formData).subscribe(x => {
        console.log('Somethign Happend ! ', formData)
      }, onError => {
        console.log('oopsss! ', onError);
      })
    }
  }


}
