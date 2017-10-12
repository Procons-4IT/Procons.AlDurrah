"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const Observable_1 = require("rxjs/Observable");
const BehaviorSubject_1 = require("rxjs/BehaviorSubject");
const http_1 = require("@angular/http");
require("rxjs/add/observable/of");
require("rxjs/add/observable/throw");
require("rxjs/add/operator/map");
require("rxjs/add/operator/filter");
require("rxjs/add/operator/toPromise");
require("rxjs/add/operator/catch");
const core_1 = require("@angular/core");
let ApiService = class ApiService {
    constructor(http) {
        this.http = http;
        this.KEYS = {
            SecurityToken: "SecurityToken",
            SecurityTokenExpiryDate: "SecurityTokenExpiryDate",
            UserType: "UserType"
        };
        this.config = require('../../config.json');
        this.resetParams = {
            EmailAddress: "",
            Password: "",
            ValidationId: ""
        };
        this.language = navigator.language;
        this.userLoggedIn = new BehaviorSubject_1.BehaviorSubject(this.isLoggedIn());
        this.userTypeLogIn = this.userLoggedIn.filter(x => x).map(x => {
            return this.GetUserType();
        });
    }
    logOut() {
        this.RemoveSecurityToken();
        this.alertListenersUserLoggedIn(this.isLoggedIn());
    }
    login(userName, password) {
        let requestBody = `grant_type=password&username=${userName}&password=${password}`;
        return this.httpPostHelper(this.config.loginUrl, requestBody).map(response => {
            console.log('I was called!  ');
            if (response.status == 200) {
                var token = response.json();
                sessionStorage.setItem(this.KEYS.SecurityToken, token.access_token);
                sessionStorage.setItem(this.KEYS.SecurityTokenExpiryDate, token['expires_in']);
                sessionStorage.setItem(this.KEYS.UserType, token.UserType);
                return true;
            }
            else
                return false;
        })
            .catch(errorResponse => {
            return Observable_1.Observable.throw(this.getErrorMessage(errorResponse));
        })
            .do(x => { this.alertListenersUserLoggedIn(true); });
    }
    getErrorMessage(errorResponse) {
        debugger;
        let errorObject = errorResponse.json && errorResponse.json();
        let errorMessage = errorObject.error_description || errorObject.message || "something went wrong";
        return errorMessage;
    }
    forgotPassword(email) {
        var requestBody = { EmailAddress: email };
        ;
        return this.httpPostHelper(this.config.forgotPasswordUrl, requestBody);
    }
    resetPassword(param) {
        return this.httpPostHelper(this.config.resetPasswordUrl, param);
    }
    confirmEmail(param) {
        return this.httpPostHelper(this.config.emailValidationUrl, param)
            .map(response => {
            return response.status === 200;
        });
    }
    createNewUser(param) {
        return this.httpPostHelper(this.config.createNewUserUrl, param)
            .map(response => {
            return response.json();
        }).catch(errorResponse => {
            return Observable_1.Observable.throw(this.getErrorMessage(errorResponse));
        });
    }
    knetPaymentRedirectUrl(paymentInformation) {
        return this.httpPostHelper(this.config.knetUrl, paymentInformation)
            .map(response => {
            return response.json();
        });
    }
    createIncomingPayment(payment) {
        return this.httpPostHelper(this.config.incomingPaymentUrl, payment)
            .map(response => {
            return response.json();
        });
    }
    getSearchCriteriaParameters() {
        return this.httpGetHelper(this.config.getSearchCriteriaUrl)
            .map(response => { return response.json(); });
    }
    getAllWorkers(optionalFilterCritera) {
        var actualData = this.httpPostHelper(this.config.getWorkersUrl, optionalFilterCritera)
            .map(response => {
            var data = response.json();
            return data;
        });
        return actualData;
    }
    httpPostHelper(url, body) {
        let headers = new http_1.Headers();
        headers.append("accept-language", this.language);
        let token = this.GetSecurityToken();
        if (token) {
            headers.append('Authorization', `bearer ${token}`);
        }
        let options = new http_1.RequestOptions({ headers: headers });
        return this.http.post(this.config.baseUrl + url, body, options);
        // .catch(tempCatch => {
        //     var fakeObject = {
        //         json: () => [1, 2, 3]
        //     };
        //     return Observable.of(<Response>fakeObject);
        // });
    }
    httpGetHelper(url) {
        let headers = new http_1.Headers();
        headers.append("Content-Type", 'application/json');
        headers.append("accept-language", this.language);
        let options = new http_1.RequestOptions({ headers: headers });
        return this.http.get(this.config.baseUrl + url, options);
        // .catch(tempCatch => {
        //     var fakeObject = {
        //         json: () => [1, 2, 3]
        //     };
        //     return Observable.of(<Response>fakeObject);
        // });
    }
    uploadFile(formData) {
        let url = 'http://localhost:59822/api/Workers/AddWorker';
        let headers = new http_1.Headers();
        // headers.append('Content-Type', 'application/x-www-form-urlencoded');
        // headers.append("Content-Type", 'application/json');
        // headers.append("accept-language", this.language);
        // let options: RequestOptions = new RequestOptions({ headers: headers });
        return this.http.post(url, formData);
    }
    isLoggedIn() {
        return !!this.GetSecurityToken();
    }
    GetUserType() {
        return sessionStorage.getItem(this.KEYS.UserType);
    }
    GetSecurityToken() {
        return sessionStorage.getItem(this.KEYS.SecurityToken);
    }
    RemoveSecurityToken() {
        sessionStorage.removeItem(this.KEYS.SecurityToken);
    }
    alertListenersUserLoggedIn(isLoggedIn) {
        this.userLoggedIn.next(isLoggedIn);
    }
    onUserLoggedIn() {
        return this.userLoggedIn;
    }
    onUserTypeLoggedIn() {
        return this.userTypeLogIn;
    }
};
ApiService = __decorate([
    core_1.Injectable()
], ApiService);
exports.ApiService = ApiService;
//# sourceMappingURL=ApiService.js.map