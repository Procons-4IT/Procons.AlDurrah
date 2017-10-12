"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let SearchResultsComponent = class SearchResultsComponent {
    constructor(sanitizer, myModal) {
        this.sanitizer = sanitizer;
        this.myModal = myModal;
        this.onSelectedWorker = new core_1.EventEmitter();
        this.onBack = new core_1.EventEmitter();
        this.showVideoModal = false;
    }
    ngOnInit() {
    }
    GoBack() {
        this.onBack.emit();
    }
    GoToProfile(selectedWorker) {
        this.onSelectedWorker.emit(selectedWorker);
    }
    GetAvailableCSS(worker) {
        var isAvaible = worker.status == "1" ? true : false;
        return {
            "glyphicon": true,
            "glyphicon-ok": isAvaible,
            "glyphicon-remove": !isAvaible
        };
    }
    openRequestedPopup(url) {
        this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        this.showVideoModal = true;
    }
};
__decorate([
    core_1.Input()
], SearchResultsComponent.prototype, "workers", void 0);
__decorate([
    core_1.Output()
], SearchResultsComponent.prototype, "onSelectedWorker", void 0);
__decorate([
    core_1.Output()
], SearchResultsComponent.prototype, "onBack", void 0);
SearchResultsComponent = __decorate([
    core_1.Component({
        selector: 'search-result',
        templateUrl: './SearchResults.component.html',
        styleUrls: ['./SearchResults.component.css']
    })
], SearchResultsComponent);
exports.SearchResultsComponent = SearchResultsComponent;
//# sourceMappingURL=SearchResults.component.js.map