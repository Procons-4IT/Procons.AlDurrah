"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const ProconsModalService_1 = require("../Services/ProconsModalService");
let CatalogueComponent = class CatalogueComponent {
    constructor(myApi, utility, sanitizer, myModal) {
        this.myApi = myApi;
        this.utility = utility;
        this.sanitizer = sanitizer;
        this.myModal = myModal;
        this.loading = false;
        this.showSearchSummary = true;
        this.showSearchForm = false;
        this.showSearchResultTable = false;
        this.showProfile = false;
    }
    GoToSearch() {
        var securityToken = this.myApi.GetSecurityToken();
        if (securityToken) {
            this.loading = true;
            this.myApi.getSearchCriteriaParameters().subscribe(searchCriteria => {
                this.loading = false;
                this.searchCriteriaParams = searchCriteria;
                this.showSearchSummary = false;
                this.showSearchForm = true;
            }, onError => {
                this.loading = false;
                console.error('Error: ', onError);
                this.myModal.showErrorModal();
            }, () => {
                this.loading = false;
            });
        }
        else {
            this.myModal.showErrorModal('error.notLoggedIn');
        }
    }
    GoToResults(workerFilter) {
        this.loading = true;
        this.myApi.getAllWorkers(workerFilter).subscribe(workers => {
            this.loading = false;
            this.workers = workers;
            this.showSearchForm = false;
            this.showSearchResultTable = true;
        }, onError => {
            this.loading = false;
            this.myModal.showErrorModal();
        });
    }
    GoToProfile(event) {
        this.selectedWorker = event;
        this.showSearchResultTable = false;
        this.showProfile = true;
    }
    ngOnInit() {
        // this.GetLookups();
    }
    GetLookups() {
        this.myApi.getSearchCriteriaParameters().subscribe(searchCriteriaParams => {
        });
    }
    //TO-DO: REMOVE AMOUNT HERE!
    Book(onBook) {
        var selectedWorker = this.selectedWorker;
        var paymentInformation = { SerialNumber: selectedWorker.serialNumber, CardCode: selectedWorker.agent, Amount: "100", Code: selectedWorker.code };
        this.loading = true;
        this.myApi.knetPaymentRedirectUrl(paymentInformation)
            .map(url => this.utility.redirectToUrl(url))
            .subscribe(onSuccess => { }, onError => {
            this.loading = false;
            this.myModal.showErrorModal();
        });
    }
    ShowSearchResult() {
        this.showProfile = false;
        this.showSearchForm = false;
        this.showSearchSummary = false;
        this.showSearchResultTable = true;
    }
    ShowSearchFilter() {
        this.showProfile = false;
        this.showSearchForm = true;
        this.showSearchSummary = false;
        this.showSearchResultTable = false;
    }
};
CatalogueComponent = __decorate([
    core_1.Component({
        selector: 'app-catalogue',
        templateUrl: './catalogue.component.html',
        styleUrls: ['./catalogue.component.css'],
        providers: [ProconsModalService_1.ProconsModalSerivce],
    })
], CatalogueComponent);
exports.CatalogueComponent = CatalogueComponent;
//# sourceMappingURL=catalogue.component.js.map