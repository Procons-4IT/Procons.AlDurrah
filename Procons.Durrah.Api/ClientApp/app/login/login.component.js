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
var Subject_1 = require("rxjs/Subject");
require("rxjs/add/operator/takeUntil");
var ContextService_1 = require("../Services/ContextService");
var app_ComponentBase_1 = require("../app.ComponentBase");
var angular2_modal_1 = require("angular2-modal");
var bootstrap_1 = require("angular2-modal/plugins/bootstrap");
var LoginComponent = (function (_super) {
    __extends(LoginComponent, _super);
    function LoginComponent(carService, router, context, modal) {
        var _this = _super.call(this) || this;
        _this.carService = carService;
        _this.router = router;
        _this.context = context;
        _this.modal = modal;
        _this.ngUnsubscribe = new Subject_1.Subject();
        _this.errorMessage = "";
        return _this;
    }
    LoginComponent.prototype.ngOnInit = function () {
    };
    LoginComponent.prototype.Login = function () {
        var _this = this;
        var header = new Array();
        var requestBody = "userName=" + this.userName + "&password=" + this.password + "&grant_type=password";
        var serviceUrl = '/oauth/token';
        header.push({ key: "Content-Type", value: 'application/x-www-form-urlencoded' });
        this.context.httpPost(header, requestBody, serviceUrl)
            .takeUntil(this.ngUnsubscribe).subscribe(function (response) {
            if (response.status == 200) {
                sessionStorage.setItem("SecurityToken", response.json().access_token);
                sessionStorage.setItem("SecurityTokenExpiryDate", response.json()['.expires']);
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
            _this.userName = null;
            _this.password = null;
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
            .body("\n                        <div class=\"modal-header\">\n                            <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>\n                        </div>\n                        <div class=\"modal-body\">\n                            <div class=\"modal-icon\"><img src=\"/eldurra-master/src/app/images/icon_lock.png\" class=\"icon\" /></div>\n                            <p><small>You are logged in</small></p>\n                            <h4>Create your own password</h4>\n                            <a href=\"#tabNewPass\" data-toggle=\"tab\" data-dismiss=\"modal\">Continue</a>\n                        </div>")
            .open();
    };
    return LoginComponent;
}(app_ComponentBase_1.ComponentBase));
LoginComponent = __decorate([
    core_1.Component({
        selector: '[app-login]',
        templateUrl: './login.component.html',
        styleUrls: ['./login.component.css'],
        providers: [bootstrap_1.Modal, angular2_modal_1.Overlay]
    }),
    __metadata("design:paramtypes", [CarService_1.CarService, router_1.Router, ContextService_1.ContextService, bootstrap_1.Modal])
], LoginComponent);
exports.LoginComponent = LoginComponent;
//<div class="modal fade" tabindex= "-1" role= "dialog" id= "modalLogin" #modalLogin >
//    <div class="modal-dialog" role= "document" >
//        <div class="modal-content" >
//            <div class="modal-header" >
//                <button type="button" class="close" data- dismiss="modal" aria- label="Close" > <span aria- hidden="true" >&times; </span></button>
//                    </div>
//                    < div class="modal-body" >
//                        <div class="modal-icon" > <img src="/eldurra-master/src/app/images/icon_lock.png" class="icon" /></div>
//                            < p > <small>You are logged in </small></p>
//                                <h4>Create your own password< /h4>
//                                    < a href= "#tabNewPass" data- toggle="tab" data- dismiss="modal" > Continue < /a>
//                                        < /div>
//                                        < /div>
//                                        < /div>
//                                        < /div> 
//# sourceMappingURL=login.component.js.map