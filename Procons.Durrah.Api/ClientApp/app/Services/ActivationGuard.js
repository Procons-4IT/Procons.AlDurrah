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
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var CarService_1 = require("./CarService");
var CanActivateViaAuthGuard = (function () {
    function CanActivateViaAuthGuard(authService, route, router) {
        this.authService = authService;
        this.route = route;
        this.router = router;
    }
    CanActivateViaAuthGuard.prototype.canActivate = function () {
        if (this.authService.isLogedIn()) {
            return true;
        }
        else {
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.route.snapshot.url } });
            return false;
        }
    };
    return CanActivateViaAuthGuard;
}());
CanActivateViaAuthGuard = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [CarService_1.CarService, router_1.ActivatedRoute, router_1.Router])
], CanActivateViaAuthGuard);
exports.CanActivateViaAuthGuard = CanActivateViaAuthGuard;
//# sourceMappingURL=ActivationGuard.js.map