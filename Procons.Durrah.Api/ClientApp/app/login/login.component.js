"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var CarService_1 = require("../Services/CarService");
var router_1 = require("@angular/router");
require("rxjs/add/operator/takeUntil");
var ContextService_1 = require("../Services/ContextService");
var app_ComponentBase_1 = require("../app.ComponentBase");
var ngx_modialog_1 = require("ngx-modialog");
var bootstrap_1 = require("ngx-modialog/plugins/bootstrap");
var ApiService_1 = require("../Services/ApiService");
var LoginComponent = (function (_super) {
    __extends(LoginComponent, _super);
    function LoginComponent(myApiService, carService, router, context, modal) {
        var _this = _super.call(this) || this;
        _this.myApiService = myApiService;
        _this.carService = carService;
        _this.router = router;
        _this.context = context;
        _this.modal = modal;
        _this.errorMessage = "";
        return _this;
    }
    LoginComponent.prototype.ngOnInit = function () {
    };
    LoginComponent.prototype.Login = function (userName, password) {
        var _this = this;
        console.log('Logging in with userName: ', userName, ' password: ', '####');
        this.myApiService.login(userName, password).subscribe(function (isLoggedIn) {
            if (isLoggedIn) {
                _this.OpenModal();
                _this.router.navigateByUrl("/Home");
            }
            else {
            }
        }, function (error) {
            _this.errorMessage = "";
            _this.errorMessage = error.json().error_description;
            _this.displayError = true;
        }, function () {
            // this.userName = null;
            // this.password = null;
        });
    };
    LoginComponent.prototype.Redirect = function () {
        window.location.href = "http://knet.testyourprojects.co.in/";
    };
    LoginComponent.prototype.OnDestroy = function () {
    };
    LoginComponent.prototype.OpenModal = function () {
        this.modal.alert()
            .showClose(true)
            .body("\n                        <div class=\"modal-body\">\n                            <div class=\"modal-icon\"><img src=\"/Assets/src/app/images/icon_lock.png\" class=\"icon\" /></div>\n                            <p><small>You are logged in</small></p>\n                            <h4>Create your own password</h4>\n                            <a href=\"#tabNewPass\" data-toggle=\"tab\" data-dismiss=\"modal\">Continue</a>\n                        </div>")
            .open();
    };
    return LoginComponent;
}(app_ComponentBase_1.ComponentBase));
LoginComponent = __decorate([
    core_1.Component({
        selector: '[app-login]',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css'],
        providers: [bootstrap_1.Modal, ngx_modialog_1.Overlay]
    }),
    __metadata("design:paramtypes", [ApiService_1.ApiService, CarService_1.CarService, router_1.Router, ContextService_1.ContextService, bootstrap_1.Modal])
], LoginComponent);
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map