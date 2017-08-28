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
var RecipeItemComponent = (function () {
    function RecipeItemComponent() {
    }
    RecipeItemComponent.prototype.CatchEvent = function (houssam) {
        // let $ = require('../ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js');
        // $.alert('ok');
        // let par = this.testReference.nativeElement.value;
    };
    RecipeItemComponent.prototype.ngOnInit = function () {
    };
    return RecipeItemComponent;
}());
__decorate([
    core_1.ViewChild("testReference"),
    __metadata("design:type", core_1.ElementRef)
], RecipeItemComponent.prototype, "testReference", void 0);
RecipeItemComponent = __decorate([
    core_1.Component({
        selector: 'app-recipe-item',
        templateUrl: './recipe-item.component.html',
        styleUrls: ['./recipe-item.component.css']
    }),
    __metadata("design:paramtypes", [])
], RecipeItemComponent);
exports.RecipeItemComponent = RecipeItemComponent;
//# sourceMappingURL=recipe-item.component.js.map