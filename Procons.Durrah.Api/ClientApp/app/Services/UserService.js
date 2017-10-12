"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const http_1 = require("@angular/http");
const core_1 = require("@angular/core");
require("rxjs/add/operator/toPromise");
let UserService = class UserService {
    constructor(http, cxtService) {
        this.http = http;
        this.cxtService = cxtService;
    }
    AddUser(requestBody, serviceUrl) {
        let header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        let options = new http_1.RequestOptions({ headers: header });
        return this.http.post(serviceUrl, requestBody, options);
    }
    GetUserRole() {
        let header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        let options = new http_1.RequestOptions({ headers: header });
        return this.http.get("http://localhost:59822/api/roles/GetCurrentRole", options);
    }
    DeleteUser(requestHeaders, serviceUrl) {
        return this.http.delete(serviceUrl);
    }
    GetUsers(serviceUrl) {
        let options = new http_1.RequestOptions({ headers: this.GetAuthorizationHeader() });
        return this.http.get(serviceUrl, options);
    }
    GetSecurityToken() {
        return this.cxtService.GetSecurityToken();
    }
    GetAuthorizationHeader() {
        let header = new http_1.Headers();
        header.append("Authorization", this.GetSecurityToken());
        return header;
    }
};
UserService = __decorate([
    core_1.Injectable()
], UserService);
exports.UserService = UserService;
//# sourceMappingURL=UserService.js.map