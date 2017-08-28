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
var router_1 = require("@angular/router");
require("rxjs/add/operator/takeUntil");
require("rxjs/add/operator/mergeMap");
var app_ComponentBase_1 = require("../app.ComponentBase");
var ngx_modialog_1 = require("ngx-modialog");
var bootstrap_1 = require("ngx-modialog/plugins/bootstrap");
var ApiService_1 = require("../Services/ApiService");
var PaymentConfirmationComponent = (function (_super) {
    __extends(PaymentConfirmationComponent, _super);
    function PaymentConfirmationComponent(myApiService, modal, route) {
        var _this = _super.call(this) || this;
        _this.myApiService = myApiService;
        _this.modal = modal;
        _this.route = route;
        return _this;
    }
    PaymentConfirmationComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.myApiService.getKnetUrlProperties()
            .map(function (x) { return _this.myApiService.createIncomingPayment(x); })
            .mergeMap(function (x) { return x; })
            .subscribe(function (onSuccess) {
            var response = 'Payment Attempted to Post: [server-response] ' + onSuccess;
            console.log(response);
            _this.OpenModal(response);
        });
    };
    PaymentConfirmationComponent.prototype.OpenModal = function (message) {
        this.modal.alert()
            .showClose(true)
            .title(message)
            .body("\n                        <div class=\"modal-header\">\n                            <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button>\n                        </div>\n                        <div class=\"modal-body\">\n                            <div class=\"modal-icon\"><img src=\"/Assets/src/app/images/icon_lock.png\" class=\"icon\" /></div>\n                            <p><small>Payment Confirmed! </small></p>\n                        </div>")
            .open();
    };
    return PaymentConfirmationComponent;
}(app_ComponentBase_1.ComponentBase));
PaymentConfirmationComponent = __decorate([
    core_1.Component({
        selector: '[app-paymentConfirmation]',
        templateUrl: './paymentConfirmation.component.html',
        styleUrls: ['./paymentConfirmation.component.css'],
        providers: [bootstrap_1.Modal, ngx_modialog_1.Overlay]
    }),
    __metadata("design:paramtypes", [ApiService_1.ApiService, bootstrap_1.Modal, router_1.ActivatedRoute])
], PaymentConfirmationComponent);
exports.PaymentConfirmationComponent = PaymentConfirmationComponent;
//# sourceMappingURL=paymentConfirmation.component.js.map