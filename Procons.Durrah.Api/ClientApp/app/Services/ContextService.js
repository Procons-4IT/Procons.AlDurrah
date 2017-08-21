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
var Subject_1 = require("rxjs/Subject");
var http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
var core_1 = require("@angular/core");
var ContextService = (function () {
    function ContextService(http) {
        this.http = http;
        this.loginPageFollower = new Subject_1.Subject();
        this.loginPageFollower.next(false);
    }
    ContextService.prototype.setLoginPage = function (islogin) {
        this.loginPageFollower.next(islogin);
    };
    ContextService.prototype.httpPost = function (requestHeaders, requestBody, serviceUrl) {
        var headers = new http_1.Headers();
        requestHeaders.forEach(function (element) {
            debugger;
            headers.append(element.valueOf().key, element.valueOf().value);
        });
        var body = requestBody;
        return this.http.post(serviceUrl, body, headers);
    };
    ContextService.prototype.GetSecurityToken = function () {
        var securityToken = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken;
    };
    return ContextService;
}());
ContextService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], ContextService);
exports.ContextService = ContextService;
//# sourceMappingURL=ContextService.js.map