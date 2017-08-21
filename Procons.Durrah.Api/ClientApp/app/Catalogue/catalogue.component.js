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
var Worker_1 = require("../Models/Worker");
var SearchContent_1 = require("../Models/SearchContent");
var WorkersService_1 = require("../Services/WorkersService");
var catalogueComponent = (function () {
    function catalogueComponent(componentFactoryResolver, workersService) {
        this.componentFactoryResolver = componentFactoryResolver;
        this.workersService = workersService;
        this.isVisible = true;
        this.visiblePart = 0;
        this.searchContent = new SearchContent_1.SearchContent();
    }
    catalogueComponent.prototype.GotoSearch = function () {
        debugger;
        this.selectedStatus = "";
        this.selectedLanguage = "";
        this.selectedAge = "";
        this.selectedGender = "";
        this.selectedType = "";
        this.selectedStatus = "";
        this.selectedCountry = "";
        //let compFactory: ComponentFactory<any>;
        //compFactory = this.componentFactoryResolver.resolveComponentFactory(searchFormComponent);
        //this.catalogue.createComponent(compFactory);
        this.tabcatalogue.nativeElement.classList.remove('active', 'in');
        //this.catalogue.nativeElement.querySelector('.tab-pane .active').classList.remove('active', 'in');
        this.tabSearchForm.nativeElement.classList.add('active', 'in');
    };
    catalogueComponent.prototype.GotoResults = function () {
        debugger;
        this.workers = [
            new Worker_1.Worker(1, "https://www.jagonews24.com/media/imgAll/2016October/SM/shahed2017061312381720170613162337.jpg", 87, 160, 'available'),
            new Worker_1.Worker(2, "https://s-media-cache-ak0.pinimg.com/736x/09/4b/2a/094b2a3d1526178188f39d10eef9fd88--maids.jpg", 50, 160, 'available'),
            new Worker_1.Worker(3, "https://media.licdn.com/mpr/mpr/shrinknp_400_400/AAEAAQAAAAAAAANYAAAAJDEzMDZmNmI4LWJkOTgtNGFiZC1hOGZmLTljNzMxODE2MjdkMw.jpg", 67, 164, 'available'),
            new Worker_1.Worker(4, "http://static.clickbd.com/global/classified/item_img/1680377_3_original.jpg", 58, 167, 'available')
        ];
        this.tabSearchForm.nativeElement.classList.remove('active', 'in');
        this.tabSearchResults.nativeElement.classList.add('active', 'in');
    };
    catalogueComponent.prototype.GotoProfile = function (event) {
        debugger;
        this.selectedWorker = event;
        this.tabSearchResults.nativeElement.classList.remove('active', 'in');
        this.tabprofile.nativeElement.classList.add('active', 'in');
    };
    catalogueComponent.prototype.ngOnInit = function () {
        this.GetLookups();
    };
    catalogueComponent.prototype.onRowSelect = function (event) {
    };
    catalogueComponent.prototype.GetLookups = function () {
        var _this = this;
        this.workersService.GetLookups("/api/Workers/GetSearchLookups").subscribe(function (response) {
            _this.languages = response.json().languages;
            _this.countries = response.json().nationality;
            _this.maritalStatus = response.json().maritalStatus;
            _this.gender = response.json().gender;
            _this.workerTypes = response.json().workerTypes;
        }, function (error) {
        });
    };
    return catalogueComponent;
}());
__decorate([
    core_1.ViewChild('catalogue'),
    __metadata("design:type", core_1.ElementRef)
], catalogueComponent.prototype, "catalogue", void 0);
__decorate([
    core_1.ViewChild('tabcatalogue'),
    __metadata("design:type", core_1.ElementRef)
], catalogueComponent.prototype, "tabcatalogue", void 0);
__decorate([
    core_1.ViewChild('tabSearchForm'),
    __metadata("design:type", core_1.ElementRef)
], catalogueComponent.prototype, "tabSearchForm", void 0);
__decorate([
    core_1.ViewChild('tabSearchResults'),
    __metadata("design:type", core_1.ElementRef)
], catalogueComponent.prototype, "tabSearchResults", void 0);
__decorate([
    core_1.ViewChild('tabprofile'),
    __metadata("design:type", core_1.ElementRef)
], catalogueComponent.prototype, "tabprofile", void 0);
catalogueComponent = __decorate([
    core_1.Component({
        selector: '[app-catalogue]',
        templateUrl: './catalogue.component.html',
        styleUrls: ['./catalogue.component.css'],
        providers: [WorkersService_1.WorkersService]
    }),
    __metadata("design:paramtypes", [core_1.ComponentFactoryResolver,
        WorkersService_1.WorkersService])
], catalogueComponent);
exports.catalogueComponent = catalogueComponent;
//# sourceMappingURL=catalogue.component.js.map