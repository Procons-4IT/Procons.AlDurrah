"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const login_component_1 = require("./login/login.component");
const home_component_1 = require("./home/home.component");
const router_1 = require("@angular/router");
class LowerCaseUrlSerializer extends router_1.DefaultUrlSerializer {
    parse(url) {
        return super.parse(url.toLocaleLowerCase());
    }
}
exports.LowerCaseUrlSerializer = LowerCaseUrlSerializer;
const routes = [
    { path: 'home', component: home_component_1.HomeComponent },
    { path: 'paymentconfirmation', data: { isPayment: true }, component: home_component_1.HomeComponent },
    { path: 'confirmemail', data: { isConfirmEmail: true }, component: home_component_1.HomeComponent },
    { path: 'resetpassword', data: { isPasswordReset: true }, component: home_component_1.HomeComponent },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: '', pathMatch: 'full', redirectTo: '/home' },
    { path: '**', pathMatch: 'full', redirectTo: '/home' }
];
let RoutingModule = class RoutingModule {
};
RoutingModule = __decorate([
    core_1.NgModule({
        imports: [router_1.RouterModule.forRoot(routes)],
        exports: [router_1.RouterModule],
        providers: [{ provide: router_1.UrlSerializer, useClass: LowerCaseUrlSerializer }]
    })
], RoutingModule);
exports.RoutingModule = RoutingModule;
exports.routingComponents = [home_component_1.HomeComponent];
//# sourceMappingURL=app.routing.js.map