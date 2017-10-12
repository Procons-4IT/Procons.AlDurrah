"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let ViewProfilesComponent = class ViewProfilesComponent {
    constructor() {
        this.onBack = new core_1.EventEmitter();
    }
    back() {
        this.onBack.emit();
    }
};
__decorate([
    core_1.Input()
], ViewProfilesComponent.prototype, "workers", void 0);
__decorate([
    core_1.Output()
], ViewProfilesComponent.prototype, "onBack", void 0);
ViewProfilesComponent = __decorate([
    core_1.Component({
        selector: "view-profiles",
        templateUrl: "./view-profiles.component.html",
        styles: ["./view-profiles.component.css"]
    })
], ViewProfilesComponent);
exports.ViewProfilesComponent = ViewProfilesComponent;
//# sourceMappingURL=view-profiles.component.js.map