"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let AppComponent = class AppComponent {
    constructor(context, router, userService, myApi, translate) {
        this.context = context;
        this.router = router;
        this.userService = userService;
        this.myApi = myApi;
        this.translate = translate;
        this.isAgent = false;
        this.isLoggedIn = false;
        this.menuNode = {};
        translate.setDefaultLang('ar');
        translate.use('ar');
        this.context.loginPageFollower.subscribe(value => { this.isLoginPage = value; });
    }
    ngOnInit() {
        this.listenToUserLogin();
        this.listenToUserTypeLogin();
    }
    listenToUserLogin() {
        this.myApi.onUserLoggedIn().subscribe(x => {
            this.isLoggedIn = x;
        });
    }
    listenToUserTypeLogin() {
        this.myApi.onUserTypeLoggedIn().subscribe(userType => {
            console.log('adding user type ', userType);
            this.isAgent = userType === "cSupplier";
        });
    }
    logOut() {
        this.myApi.logOut();
    }
};
AppComponent = __decorate([
    core_1.Component({
        selector: 'app-root',
        templateUrl: './app.component.html',
        styleUrls: ['./app.component.css']
    })
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map