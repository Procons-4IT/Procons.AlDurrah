"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let searchFormComponent = class searchFormComponent {
    constructor() {
        this.onSearchFilterCriteria = new core_1.EventEmitter();
    }
    ngOnInit() {
    }
    GotoResults(workType, age, sex, nationality, maritalStatus, language) {
        let argumentKeys = ["workType", "age", "gender", "nationality", "maritalStatus", "language"];
        let workerFilterParams = {};
        for (var i = 0; i < arguments.length; i++) {
            let argument = arguments[i];
            if (argument.type == 'select-one') {
                let isFirstElement = argument.selectedIndex === 0;
                if (!isFirstElement) {
                    let keyName = argumentKeys[i];
                    workerFilterParams[keyName] = argument.value;
                }
            }
        }
        this.onSearchFilterCriteria.emit(workerFilterParams);
    }
};
__decorate([
    core_1.Output()
], searchFormComponent.prototype, "onSearchFilterCriteria", void 0);
__decorate([
    core_1.Input()
], searchFormComponent.prototype, "searchCriteriaParams", void 0);
searchFormComponent = __decorate([
    core_1.Component({
        selector: 'search-form',
        templateUrl: './searchForm.component.html',
        styleUrls: ['./searchForm.component.css']
    })
], searchFormComponent);
exports.searchFormComponent = searchFormComponent;
// <!-- <div *ngFor "let nameValuePair of searchCriteriaParams.workerTypes;">
// {{nameValuePair.name }}
// </div> -->
//# sourceMappingURL=SearchForm.component.js.map