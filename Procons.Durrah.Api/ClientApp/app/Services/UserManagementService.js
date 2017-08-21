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
var http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
var core_1 = require("@angular/core");
var UserManagementService = (function () {
    function UserManagementService(http) {
        this.http = http;
    }
    UserManagementService.prototype.CreateUser = function (requestHeaders, requestBody, serviceUrl) {
        return this.http.post(serviceUrl, requestBody, requestHeaders);
    };
    UserManagementService.prototype.DeleteUser = function (requestHeaders, serviceUrl) {
        return this.http.delete(serviceUrl);
    };
    UserManagementService.prototype.GetUsers = function (serviceUrl) {
        var headers = new http_1.Headers();
        return this.http.get(serviceUrl);
    };
    return UserManagementService;
}());
UserManagementService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], UserManagementService);
exports.UserManagementService = UserManagementService;
//# sourceMappingURL=UserManagementService.js.map