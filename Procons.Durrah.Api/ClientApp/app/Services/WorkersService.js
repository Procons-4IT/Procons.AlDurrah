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
let WorkersService = class WorkersService {
    constructor(http, cxtService) {
        this.http = http;
        this.cxtService = cxtService;
    }
    GetLookups(serviceUrl) {
        return this.http.get(serviceUrl);
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
WorkersService = __decorate([
    core_1.Injectable()
], WorkersService);
exports.WorkersService = WorkersService;
//# sourceMappingURL=WorkersService.js.map