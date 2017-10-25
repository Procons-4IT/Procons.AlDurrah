"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const angular2_recaptcha_1 = require("angular2-recaptcha");
let LoginComponent = class LoginComponent {
    constructor(myApi, myModal, activeRouter, utility) {
        this.myApi = myApi;
        this.myModal = myModal;
        this.activeRouter = activeRouter;
        this.utility = utility;
        this.loading = false;
        this.isLoggedIn = false;
        this.showForgotPasswordModal = false;
        this.forgotPassModalLoading = false;
        this.forgotPassModalError = "";
        this.forgotPassModalText = "";
        this.resetPassModalLoading = false;
        this.resetPassInputError = "";
        this.resetPassModalText = "";
        this.recaptchaToken = "";
        this.confirmPassword = "";
        this.newUser = {
            FirstName: "",
            LastName: "",
            UserName: "",
            CivilId: "",
            Password: "",
            Email: ""
        };
        this.resetParams = {
            EmailAddress: "",
            Password: "",
            ValidationId: ""
        };
    }
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
    passwordMatch() {
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
            });
        }, (errorMessage) => {
            console.log('login error ', errorMessage);
            this.loading = false;
            this.myModal.showErrorModal(errorMessage, false);
        });
    }
    ForgotPassword(email) {
        this.forgotPassModalError = "";
        this.forgotPassModalLoading = true;
        this.myApi.forgotPassword(email).subscribe(isSuccesful => {
            this.forgotPassModalLoading = false;
            $('#myModal').modal('toggle');
            this.showForgotPasswordModal = false;
            this.forgotPassModalText = "error.forgotPassword-200";
        }, onError => {
            this.forgotPassModalLoading = false;
            if (onError.status == '404') {
                this.forgotPassModalError = "error.forgotPassword-404";
            }
            else {
                let errorObject = onError.json();
                let errorMessage = errorObject.message;
                if (errorMessage) {
                    this.myModal.showErrorModal(errorMessage, false);
                }
                else {
                    this.myModal.showErrorModal('error.forgotPassword');
                }
            }
        });
    }
    CreateUser() {
        this.loading = true;
        this.newUser["CaptchaCode"] = this.recaptchaToken;
        this.myApi.createNewUser(this.newUser).subscribe(x => {
            this.loading = false;
            this.recaptchaToken = null;
            this.captcha.reset();
        }, onError => {
            debugger;
            this.loading = false;
            this.myModal.showErrorModal(onError, false);
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
            </div> `;
        this.myModal.showHTMLModal(html);
    }
};
__decorate([
    core_1.ViewChild(angular2_recaptcha_1.ReCaptchaComponent)
], LoginComponent.prototype, "captcha", void 0);
LoginComponent = __decorate([
    core_1.Component({
        selector: 'login',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css']
    })
], LoginComponent);
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map