"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const http_1 = require("@angular/http");
require("rxjs/add/operator/toPromise");
let CarService = class CarService {
    constructor(http) {
        this.http = http;
    }
    httpPost(requestHeaders, requestBody, serviceUrl) {
        let headers = new http_1.Headers();
        requestHeaders.forEach(element => {
            headers.append(element.key, element.value);
        });
        let body = requestBody;
        return this.http.post(serviceUrl, body, headers);
    }
    getCarsMedium() {
    }
    isLogedIn() {
        if (!sessionStorage)
            return false;
        else {
            let expirationDate = new Date(sessionStorage.getItem("SecurityTokenExpiryDate"));
            let currentDate = new Date();
            if (currentDate >= expirationDate) {
                return false;
            }
            else {
                return true;
            }
        }
    }
    ngOnDestroy() {
    }
};
CarService = __decorate([
    core_1.Injectable()
], CarService);
exports.CarService = CarService;
//# sourceMappingURL=CarService.js.map