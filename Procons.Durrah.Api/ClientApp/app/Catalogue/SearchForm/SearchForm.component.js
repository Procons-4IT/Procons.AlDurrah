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
var searchFormComponent = (function () {
    function searchFormComponent() {
        this.onSearchFilterCriteria = new core_1.EventEmitter();
    }
    searchFormComponent.prototype.ngOnInit = function () {
        console.log('Init SearchForm Component, ', this.searchCriteriaParams);
    };
    searchFormComponent.prototype.GotoResults = function (workType, age, sex, nationality, maritalStatus, language) {
        var argumentKeys = ["workType", "age", "sex", "nationality", "maritalStatus", "language"];
        var workerFilterParams = {};
        for (var i = 0; i < arguments.length; i++) {
            var argument = arguments[i];
            if (argument.type == 'select-one') {
                var isFirstElement = argument.value === argument.options[0].value;
                if (!isFirstElement) {
                    var keyName = argumentKeys[i];
                    workerFilterParams[keyName] = argument.value;
                }
            }
        }
        console.log('captured searchFilter ', workerFilterParams);
        this.onSearchFilterCriteria.emit(workerFilterParams);
    };
    return searchFormComponent;
}());
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], searchFormComponent.prototype, "onSearchFilterCriteria", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], searchFormComponent.prototype, "searchCriteriaParams", void 0);
searchFormComponent = __decorate([
    core_1.Component({
        selector: 'search-form',
        templateUrl: './searchForm.component.html',
        styleUrls: ['./searchForm.component.css']
    }),
    __metadata("design:paramtypes", [])
], searchFormComponent);
exports.searchFormComponent = searchFormComponent;
// <!-- <div *ngFor "let nameValuePair of searchCriteriaParams.workerTypes;">
// {{nameValuePair.name }}
// </div> -->
//# sourceMappingURL=SearchForm.component.js.map