"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
require("rxjs/add/operator/map");
require("rxjs/add/operator/mergeMap");
require("rxjs/add/operator/filter");
require("rxjs/add/operator/do");
let HomeComponent = class HomeComponent {
    constructor(myApi, utility, activeRouter, router, translate) {
        this.myApi = myApi;
        this.utility = utility;
        this.activeRouter = activeRouter;
        this.router = router;
        this.translate = translate;
        this.isLoggedIn = false;
        this.logInType = '';
        this.paymentModalText = "";
        this.paymentParams = {
            PaymentID: "",
            Postdate: "",
            Result: "",
            TranID: "",
            Auth: "",
            Ref: "",
            TrackID: ""
        };
        this.resetPassModalLoading = false;
        this.resetPassInputError = "";
        this.resetPassModalText = "";
        this.resetParams = {
            EmailAddress: "",
            Password: "",
            ValidationId: ""
        };
        this.loadingPayment = false;
    }
    ngOnInit() {
        this.myApi.onUserLoggedIn()
            .subscribe(isLoggedIn => {
            this.isLoggedIn = isLoggedIn;
        });
        this.myApi.onUserTypeLoggedIn().subscribe(loginType => {
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
            .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter); })
            .mergeMap(params => { return this.myApi.confirmEmail(params); })
            .subscribe(x => {
            this.loadingPayment = false;
            if (x) {
                this.paymentModalText = 'Email Confirmed!';
                this.router.navigate(['/home']);
            }
            else {
                this.paymentModalText = 'Invalid Email!';
            }
        }, onError => {
            this.loadingPayment = false;
            this.paymentModalText = 'Something Went Wrong!';
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
            .mergeMap(x => { return this.utility.getKnetUrlProperties(this.activeRouter); })
            .do(x => { this.paymentParams = x; })
            .mergeMap(params => { return this.myApi.createIncomingPayment(params); })
            .subscribe(x => {
            this.loadingPayment = false;
            this.paymentModalText = x.udF1;
            this.amount = x && x.amount;
        }, onError => {
            this.loadingPayment = false;
            this.paymentModalText = 'Something Went Wrong!';
        });
    }
    handlePasswordResetRoute() {
        this.activeRouter.data
            .filter((data, idx) => { return data.isPasswordReset; })
            .do(x => {
            $('#modalResetPass').modal('toggle');
        })
            .mergeMap(x => { return this.utility.getResetPasswordUrlProperties(this.activeRouter); })
            .subscribe(x => {
            this.resetParams.EmailAddress = x.Email;
            this.resetParams.ValidationId = x.ValidationId;
        }, onError => {
            this.translate.get('error.resetPassword').subscribe(errorMessage => {
                this.resetPassModalText = errorMessage;
            });
        });
    }
    ResetPassword(password, confirmPassword) {
        if (password !== confirmPassword) {
            this.resetPassInputError = "Error Passwords not matching";
        }
        else {
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
                });
            });
        }
    }
};
HomeComponent = __decorate([
    core_1.Component({
        selector: 'app-home',
        templateUrl: './home.component.html',
        styleUrls: ['./home.component.css']
    })
], HomeComponent);
exports.HomeComponent = HomeComponent;
//# sourceMappingURL=home.component.js.map