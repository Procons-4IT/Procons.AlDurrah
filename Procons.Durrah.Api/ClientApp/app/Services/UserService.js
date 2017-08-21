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
var ContextService_1 = require("./ContextService");
var http_1 = require("@angular/http");
var core_1 = require("@angular/core");
require("rxjs/add/operator/toPromise");
var UserService = (function () {
    function UserService(http, cxtService) {
        this.http = http;
        this.cxtService = cxtService;
    }
    UserService.prototype.AddUser = function (requestBody, serviceUrl) {
        var header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        var options = new http_1.RequestOptions({ headers: header });
        return this.http.post(serviceUrl, requestBody, options);
    };
    UserService.prototype.GetUserRole = function () {
        var header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        var options = new http_1.RequestOptions({ headers: header });
        return this.http.get("http://localhost:59822/api/roles/GetCurrentRole", options);
    };
    UserService.prototype.DeleteUser = function (requestHeaders, serviceUrl) {
        return this.http.delete(serviceUrl);
    };
    UserService.prototype.GetUsers = function (serviceUrl) {
        var options = new http_1.RequestOptions({ headers: this.GetAuthorizationHeader() });
        return this.http.get(serviceUrl, options);
    };
    UserService.prototype.GetSecurityToken = function () {
        return this.cxtService.GetSecurityToken();
    };
    UserService.prototype.GetAuthorizationHeader = function () {
        var header = new http_1.Headers();
        header.append("Authorization", this.GetSecurityToken());
        return header;
    };
    return UserService;
}());
UserService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http, ContextService_1.ContextService])
], UserService);
exports.UserService = UserService;
//# sourceMappingURL=UserService.js.map