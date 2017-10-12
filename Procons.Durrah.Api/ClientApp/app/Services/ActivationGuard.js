"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let CanActivateViaAuthGuard = class CanActivateViaAuthGuard {
    constructor(authService, route, router) {
        this.authService = authService;
        this.route = route;
        this.router = router;
    }
    canActivate() {
        if (this.authService.isLogedIn()) {
            return true;
        }
        else {
            this.router.navigate(['/login'], { queryParams: { returnUrl: this.route.snapshot.url } });
            return false;
        }
    }
};
CanActivateViaAuthGuard = __decorate([
    core_1.Injectable()
], CanActivateViaAuthGuard);
exports.CanActivateViaAuthGuard = CanActivateViaAuthGuard;
//# sourceMappingURL=ActivationGuard.js.map