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
let UserManagementService = class UserManagementService {
    constructor(http) {
        this.http = http;
    }
    CreateUser(requestHeaders, requestBody, serviceUrl) {
        return this.http.post(serviceUrl, requestBody, requestHeaders);
    }
    DeleteUser(requestHeaders, serviceUrl) {
        return this.http.delete(serviceUrl);
    }
    GetUsers(serviceUrl) {
        let headers = new http_1.Headers();
        return this.http.get(serviceUrl);
    }
};
UserManagementService = __decorate([
    core_1.Injectable()
], UserManagementService);
exports.UserManagementService = UserManagementService;
//# sourceMappingURL=UserManagementService.js.map