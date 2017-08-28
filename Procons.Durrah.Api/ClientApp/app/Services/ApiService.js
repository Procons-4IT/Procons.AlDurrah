"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var router_1 = require("@angular/router");
var http_1 = require("@angular/http");
require("rxjs/add/observable/of");
require("rxjs/add/operator/map");
require("rxjs/add/operator/toPromise");
var core_1 = require("@angular/core");
var ApiService = (function () {
    function ApiService(http, activeRoute) {
        this.http = http;
        this.activeRoute = activeRoute;
        this.config = require('../../config.json');
    }
    ApiService.prototype.login = function (userName, password) {
        var requestBody = "userName=" + userName + "&password=" + password + "&grant_type=password";
        return this.httpPostHelper(this.config.loginUrl, requestBody).map(function (response) {
            if (response.status == 200) {
                var token = response.json();
                console.log(token);
                sessionStorage.setItem("SecurityToken", token.access_token);
                sessionStorage.setItem("SecurityTokenExpiryDate", token['expires_in']);
                return true;
            }
            else
                return false;
        });
    };
    ApiService.prototype.knetPaymentRedirect = function (paymentInformation) {
        return this.httpPostHelper(this.config.knetUrl, paymentInformation).map(function (response) {
            if (response.status = 200) {
                var knetPortalUrl = response.json();
                if (knetPortalUrl) {
                    window.location.href = knetPortalUrl;
                }
            }
            console.error('knetPayment failed to get valid response ', response);
            return '';
        });
    };
    ApiService.prototype.createIncomingPayment = function (payment) {
        return this.httpPostHelper(this.config.incomingPaymentUrl, payment)
            .map(function (response) {
            return response.json();
        });
    };
    ApiService.prototype.getAllWorkers = function () {
        console.log('Getting All the Workers');
        var actualData = this.httpGetHelper(this.config.getWorkersUrl)
            .map(function (response) {
            var data = response.json();
            console.log('[server-worker data] ', data);
            data.map(function (x) { x.image = x.photo; x.video = "https://www.youtube.com/embed/o_XCxBbuaJE?rel=0&amp;showinfo=0"; });
            return data;
        });
        return actualData;
    };
    ApiService.prototype.httpPostHelper = function (url, body) {
        // let headers = new Headers();
        // headers.append("Content-Type", 'application/x-www-form-urlencoded');
        return this.http.post(this.config.baseUrl + url, body);
    };
    ApiService.prototype.httpGetHelper = function (url) {
        var headers = new http_1.Headers();
        headers.append("Content-Type", 'application/json');
        var options = new http_1.RequestOptions({ headers: headers });
        return this.http.get(this.config.baseUrl + url, options);
    };
    ApiService.prototype.GetSecurityToken = function () {
        var securityToken = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken;
    };
    ApiService.prototype.getKnetUrlProperties = function () {
        return this.activeRoute.queryParams.map(function (x) {
            return {
                PaymentID: x.PaymentID,
                Postdate: x.Postdate,
                Result: x.Result,
                TranID: x.TranID,
                Auth: x.Auth
            };
        });
    };
    return ApiService;
}());
ApiService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http, router_1.ActivatedRoute])
], ApiService);
exports.ApiService = ApiService;
//# sourceMappingURL=ApiService.js.map