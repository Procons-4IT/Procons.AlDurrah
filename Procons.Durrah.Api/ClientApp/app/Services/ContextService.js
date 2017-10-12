"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const Subject_1 = require("rxjs/Subject");
const http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
const core_1 = require("@angular/core");
let ContextService = class ContextService {
    constructor(http) {
        this.http = http;
        this.loginPageFollower = new Subject_1.Subject();
        this.loginPageFollower.next(false);
    }
    setLoginPage(islogin) {
        this.loginPageFollower.next(islogin);
    }
    httpPost(requestHeaders, requestBody, serviceUrl) {
        let headers = new http_1.Headers();
        requestHeaders.forEach(element => {
            debugger;
            headers.append(element.valueOf().key, element.valueOf().value);
        });
        let body = requestBody;
        return this.http.post(serviceUrl, body, headers);
    }
    GetSecurityToken() {
        let securityToken = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken;
    }
};
ContextService = __decorate([
    core_1.Injectable()
], ContextService);
exports.ContextService = ContextService;
//# sourceMappingURL=ContextService.js.map