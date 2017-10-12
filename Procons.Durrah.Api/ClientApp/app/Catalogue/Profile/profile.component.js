"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let profileComponent = class profileComponent {
    constructor() {
        this.onBook = new core_1.EventEmitter();
        this.onBack = new core_1.EventEmitter();
    }
    ngOnInit() {
    }
    GoBack() {
        this.onBack.emit();
    }
    Book() {
        this.onBook.emit(true);
    }
};
__decorate([
    core_1.Input()
], profileComponent.prototype, "worker", void 0);
__decorate([
    core_1.Output()
], profileComponent.prototype, "onBook", void 0);
__decorate([
    core_1.Output()
], profileComponent.prototype, "onBack", void 0);
profileComponent = __decorate([
    core_1.Component({
        selector: 'profile',
        templateUrl: './profile.component.html',
        styleUrls: ['./profile.component.css']
    })
], profileComponent);
exports.profileComponent = profileComponent;
//# sourceMappingURL=profile.component.js.map