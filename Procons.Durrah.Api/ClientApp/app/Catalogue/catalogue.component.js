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
var WorkersService_1 = require("../Services/WorkersService");
var ApiService_1 = require("../Services/ApiService");
var platform_browser_1 = require("@angular/platform-browser");
var catalogueComponent = (function () {
    function catalogueComponent(myApi, componentFactoryResolver, workersService, sanitizer) {
        this.myApi = myApi;
        this.componentFactoryResolver = componentFactoryResolver;
        this.workersService = workersService;
        this.sanitizer = sanitizer;
        this.showSearchSummary = true;
        this.showSearchForm = false;
        this.showSearchResultTable = false;
        this.showProfile = false;
    }
    catalogueComponent.prototype.GoToSearch = function () {
        var _this = this;
        this.myApi.getSearchCriteriaParameters().subscribe(function (searchCriteria) {
            _this.searchCriteriaParams = searchCriteria;
            _this.showSearchSummary = false;
            _this.showSearchForm = true;
        });
    };
    catalogueComponent.prototype.GoToResults = function (workerFilter) {
        var _this = this;
        console.log('Search-Filter ', workerFilter);
        this.myApi.getAllWorkers(workerFilter).subscribe(function (workers) {
            _this.workers = workers;
            _this.showSearchForm = false;
            _this.showSearchResultTable = true;
        });
    };
    catalogueComponent.prototype.GoToProfile = function (event) {
        console.log('selected Worker: ', event);
        this.selectedWorker = event;
        this.showSearchResultTable = false;
        this.showProfile = true;
    };
    catalogueComponent.prototype.ngOnInit = function () {
        // this.GetLookups();
    };
    catalogueComponent.prototype.GetLookups = function () {
        this.myApi.getSearchCriteriaParameters().subscribe(function (searchCriteriaParams) {
            console.log('look up values ', searchCriteriaParams);
        });
    };
    catalogueComponent.prototype.Book = function (onBook) {
        var selectedWorker = this.selectedWorker;
        console.log('calling knetPayment! for worker ', selectedWorker);
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: "C220Temp", Amount: "100", Code: selectedWorker.code };
        this.myApi.knetPaymentRedirect(paymentInformation).subscribe();
    };
    return catalogueComponent;
}());
catalogueComponent = __decorate([
    core_1.Component({
        selector: '[app-catalogue]',
        templateUrl: './catalogue.component.html',
        styleUrls: ['./catalogue.component.css'],
        providers: [WorkersService_1.WorkersService],
    }),
    __metadata("design:paramtypes", [ApiService_1.ApiService, core_1.ComponentFactoryResolver,
        WorkersService_1.WorkersService, platform_browser_1.DomSanitizer])
], catalogueComponent);
exports.catalogueComponent = catalogueComponent;
//# sourceMappingURL=catalogue.component.js.map