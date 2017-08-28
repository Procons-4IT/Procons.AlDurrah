"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var login_component_1 = require("./login/login.component");
var home_component_1 = require("./home/home.component");
var paymentConfirmation_component_1 = require("./paymentConfirmation/paymentConfirmation.component");
var router_1 = require("@angular/router");
var routes = [
    { path: 'Home', component: home_component_1.HomeComponent },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: 'PaymentConfirmation', component: paymentConfirmation_component_1.PaymentConfirmationComponent },
    { path: '', pathMatch: 'full', redirectTo: '/Home' },
    { path: '**', pathMatch: 'full', redirectTo: '/Home' }
];
var RoutingModule = (function () {
    function RoutingModule() {
    }
    return RoutingModule;
}());
RoutingModule = __decorate([
    core_1.NgModule({
        imports: [router_1.RouterModule.forRoot(routes)],
        exports: [router_1.RouterModule],
    })
], RoutingModule);
exports.RoutingModule = RoutingModule;
exports.routingComponents = [home_component_1.HomeComponent];
//# sourceMappingURL=app.routing.js.map