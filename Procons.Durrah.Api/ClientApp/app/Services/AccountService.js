"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
const core_1 = require("@angular/core");
let AccountService = class AccountService {
    constructor(http) {
        this.http = http;
    }
    httpPost(requestHeaders, requestBody, serviceUrl) {
        return this.http.post(serviceUrl, requestBody, requestHeaders);
    }
    DeleteAccount(requestHeaders, serviceUrl) {
        return this.http.delete(serviceUrl);
    }
    GetAccounts(serviceUrl) {
        let headers = new http_1.Headers();
        return this.http.get(serviceUrl);
    }
};
AccountService = __decorate([
    core_1.Injectable()
], AccountService);
exports.AccountService = AccountService;
//# sourceMappingURL=AccountService.js.map