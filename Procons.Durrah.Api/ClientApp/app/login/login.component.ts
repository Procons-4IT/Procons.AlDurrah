import { Component, OnInit, ViewChild, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ReCaptchaComponent } from 'angular2-recaptcha';
import { ResetPasswordParams, CreateNewUserParams } from '../Models/ApiRequestType';

import { ApiService } from '../Services/ApiService';
import { ProconsModalSerivce } from '../Services/ProconsModalService';
import { UtilityService } from '../Services/UtilityService';

var errorMessages = require('../../errorMessages.json');

declare var $;

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']

})
export class LoginComponent implements OnInit {
    @ViewChild(ReCaptchaComponent) captcha: ReCaptchaComponent;

    public loading: boolean = false;
    public isLoggedIn: boolean = false;

    public showForgotPasswordModal = false;
    public forgotPassModalLoading: boolean = false;
    public forgotPassModalError: string = "";
    public forgotPassModalText: string = "";

    public resetPassModalLoading: boolean = false;
    public resetPassInputError: string = "";
    public resetPassModalText: string = "";


    public recaptchaToken = "";
    public confirmPassword = "";
    public newUser: CreateNewUserParams = {
        FirstName: "",
        LastName: "",
        UserName: "",
        CivilId: "",
        Password: "",
        Email: ""
    };

    public resetParams: ResetPasswordParams = {
        EmailAddress: "",
        Password: "",
        ValidationId: ""
    };

    constructor(
        private myApi: ApiService
        , public myModal: ProconsModalSerivce
        , public activeRouter: ActivatedRoute
        , public utility: UtilityService) { }
    ngOnInit() {
        let isLoggedIn$ = this.myApi.onUserLoggedIn();
        this.isLoggedIn = isLoggedIn$.getValue();
        isLoggedIn$.subscribe(isLoggedIn => {
            this.isLoggedIn = isLoggedIn;
        });

    }

    GoToRegisterEvent() {
        // console.log('going to registration form, reseting recaptcha!');
        // grecaptcha.reset();
    }
    passwordMatch(): boolean {
        return this.confirmPassword === this.newUser.Password;
    }




    Login() {
        console.log('Logging in with userName: ', this.newUser.UserName, ' password: ', '####');
        this.loading = true;

        this.myApi.login(this.newUser.UserName, this.newUser.Password).subscribe(isLoggedIn => {
            this.loading = false;
            this.OpenModal();
            this.isLoggedIn = true;
            this.myApi.onUserLoggedIn().subscribe(x => {
                this.isLoggedIn = x;
            })
        }, (error) => {
            this.loading = false;
            this.myModal.showErrorModal(errorMessages.login);
        });

    }
    ForgotPassword(email: string) {
        console.log('### Sending ForgotPassword Request for email ', email);
        this.forgotPassModalError = "";
        this.forgotPassModalLoading = true;
        this.myApi.forgotPassword(email).subscribe(isSuccesful => {
            this.forgotPassModalLoading = false;
            $('#myModal').modal('toggle')
            this.showForgotPasswordModal = false;
            this.forgotPassModalText = "Reset Has Been Sent!";


        }, onError => {
            this.forgotPassModalLoading = false;

            if (onError.status == '404') {
                this.forgotPassModalError = "Invalid Email";

            } else {
                this.myModal.showErrorModal(errorMessages.forgotPassword);
            }
        })

    }
    CreateUser() {
        this.loading = true;
        this.newUser["CaptchaCode"] = this.recaptchaToken;
        this.myApi.createNewUser(this.newUser).subscribe(x => {
            this.loading = false;
            this.recaptchaToken = null;
            this.captcha.reset();
        }, onError => {
            console.log(onError);
            this.loading = false;
            this.myModal.showErrorModal(errorMessages.register);
            this.recaptchaToken = null;
            this.captcha.reset();


        });
    }
    captchaSubmitted($event) {
        console.log('I got the Captcha response!', $event);
        this.recaptchaToken = $event;
    }

    OpenModal() {
        let html = `
            <div class="modal-body">
                <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                <p><small>لقد سجلت الدخول</small></p>
                <h4>إنشاء كلمة المرور</h4>
                <a href="#tabNewPass" data-toggle="tab" data-dismiss="modal">واصل</a>
            </div> `

        this.myModal.showHTMLModal(html);
    }
}
