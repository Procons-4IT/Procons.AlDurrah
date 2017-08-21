"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var material_1 = require("@angular/material");
var platform_browser_1 = require("@angular/platform-browser");
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var animations_1 = require("@angular/platform-browser/animations");
var app_component_1 = require("./app.component");
var recipes_component_1 = require("./recipes/recipes.component");
var recipe_list_component_1 = require("./recipes/recipe-list/recipe-list.component");
var catalogue_component_1 = require("./catalogue/catalogue.component");
var SearchResults_component_1 = require("./catalogue/SearchResults/SearchResults.component");
var SearchForm_component_1 = require("./catalogue/SearchForm/SearchForm.component");
var profile_component_1 = require("./catalogue/profile/profile.component");
var recipe_detail_component_1 = require("./recipes/recipe-detail/recipe-detail.component");
var recipe_item_component_1 = require("./recipes/recipe-list/recipe-item/recipe-item.component");
var app_routing_1 = require("./app.routing");
var ActivationGuard_1 = require("./Services/ActivationGuard");
var angular2_busy_1 = require("angular2-busy");
var primeng_1 = require("primeng/primeng");
var UserService_1 = require("./Services/UserService");
var CarService_1 = require("./Services/CarService");
var AccountService_1 = require("./Services/AccountService");
var ContextService_1 = require("./Services/ContextService");
var home_component_1 = require("./home/home.component");
var login_component_1 = require("./login/login.component");
var gv_control_component_1 = require("./SharedComponents/gv-control/gv-control.component");
var primeng_2 = require("primeng/primeng");
var angular2_modal_1 = require("angular2-modal");
var bootstrap_1 = require("angular2-modal/plugins/bootstrap");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        declarations: [
            app_component_1.AppComponent,
            catalogue_component_1.catalogueComponent,
            recipes_component_1.RecipesComponent,
            recipe_list_component_1.RecipeListComponent,
            recipe_detail_component_1.RecipeDetailComponent,
            recipe_item_component_1.RecipeItemComponent,
            app_routing_1.routingComponents,
            home_component_1.HomeComponent,
            login_component_1.LoginComponent,
            gv_control_component_1.GvControlComponent,
            SearchResults_component_1.SearchResultsComponent,
            SearchForm_component_1.searchFormComponent,
            profile_component_1.profileComponent
        ],
        imports: [
            bootstrap_1.BootstrapModalModule,
            angular2_modal_1.ModalModule.forRoot(),
            angular2_busy_1.BusyModule,
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_1.HttpModule,
            primeng_1.PanelMenuModule,
            animations_1.BrowserAnimationsModule,
            app_routing_1.RoutingModule,
            primeng_1.DataTableModule,
            primeng_1.SharedModule,
            primeng_1.SliderModule,
            primeng_1.DropdownModule,
            primeng_1.MultiSelectModule,
            primeng_1.ButtonModule,
            primeng_1.DialogModule,
            primeng_2.DataGridModule
        ],
        providers: [CarService_1.CarService,
            ActivationGuard_1.CanActivateViaAuthGuard,
            ContextService_1.ContextService,
            AccountService_1.AccountService,
            UserService_1.UserService,
            material_1.OVERLAY_PROVIDERS],
        bootstrap: [app_component_1.AppComponent],
        entryComponents: [SearchResults_component_1.SearchResultsComponent, SearchForm_component_1.searchFormComponent, profile_component_1.profileComponent]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map