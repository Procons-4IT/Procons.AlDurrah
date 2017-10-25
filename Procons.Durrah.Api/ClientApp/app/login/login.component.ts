import { Component, OnInit, ViewChild, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { ReCaptchaComponent } from 'angular2-recaptcha';
import { ResetPasswordParams, CreateNewUserParams } from '../Models/ApiRequestType';
import { ApiService } from '../Services/ApiService';
import { ProconsModalSerivce } from '../Services/ProconsModalService';
import { UtilityService } from '../Services/UtilityService';


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
        , public utility: UtilityService
    ) { }
    ngOnInit() {
        let isLoggedIn$ = this.myApi.onUserLoggedIn();
        this.isLoggedIn = isLoggedIn$.getValue();
        isLoggedIn$.subscribe(isLoggedIn => {
            this.isLoggedIn = isLoggedIn;
        });

    }

    GoToRegisterEvent() {
        // 
        // grecaptcha.reset();
    }
    passwordMatch(): boolean {
        return this.confirmPassword === this.newUser.Password;
    }




    Login() {

        this.loading = true;

        this.myApi.login(this.newUser.UserName, this.newUser.Password).subscribe(isLoggedIn => {
            this.loading = false;
            this.OpenModal();
            this.isLoggedIn = true;
            this.myApi.onUserLoggedIn().subscribe(x => {
                this.isLoggedIn = x;
            })
        }, (errorMessage) => {
            console.log('login error ', errorMessage);
            this.loading = false;
            this.myModal.showErrorModal(errorMessage, false);
        });

    }
    ForgotPassword(email: string) {

        this.forgotPassModalError = "";
        this.forgotPassModalLoading = true;
        this.myApi.forgotPassword(email).subscribe(isSuccesful => {
            this.forgotPassModalLoading = false;
            $('#myModal').modal('toggle')
            this.showForgotPasswordModal = false;
            this.forgotPassModalText = "error.forgotPassword-200";


        }, onError => {
            this.forgotPassModalLoading = false;

            if (onError.status == '404') {
                this.forgotPassModalError = "error.forgotPassword-404";

            } else {
                let errorObject = onError.json();
                let errorMessage = errorObject.message;
                if (errorMessage) {
                    this.myModal.showErrorModal(errorMessage, false);
                } else {
                    this.myModal.showErrorModal('error.forgotPassword');
                }
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
            this.myModal.showSuccessModal(x);
        }, onError => {
            debugger;
            this.loading = false;
            this.myModal.showErrorModal(onError,false);
            this.recaptchaToken = null;
            this.captcha.reset();

        });
    }
    captchaSubmitted($event) {

        this.recaptchaToken = $event;
    }

    OpenModal() {
        let html = `
            <div class="modal-body">
                <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                <p><small>لقد سجلت الدخول</small></p>
            </div> `

        this.myModal.showHTMLModal(html);
    }
}
