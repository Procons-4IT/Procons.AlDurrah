"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const platform_browser_1 = require("@angular/platform-browser");
const core_1 = require("@angular/core");
const forms_1 = require("@angular/forms");
const http_1 = require("@angular/common/http");
const http_2 = require("@angular/http");
const animations_1 = require("@angular/platform-browser/animations");
const app_component_1 = require("./app.component");
const catalogue_component_1 = require("./Catalogue/catalogue.component");
const SearchResults_component_1 = require("./Catalogue/SearchResults/SearchResults.component");
const SearchForm_component_1 = require("./Catalogue/SearchForm/SearchForm.component");
const profile_component_1 = require("./Catalogue/Profile/profile.component");
const worker_managment_component_1 = require("./worker-managment/worker-managment.component");
const view_profiles_component_1 = require("./worker-managment/view-profiles/view-profiles.component");
const add_profile_component_1 = require("./worker-managment/add-profile/add-profile.component");
const app_routing_1 = require("./app.routing");
const ActivationGuard_1 = require("./Services/ActivationGuard");
const angular2_busy_1 = require("angular2-busy");
const ngx_loading_1 = require("ngx-loading");
const angular2_recaptcha_1 = require("angular2-recaptcha");
const core_2 = require("@ngx-translate/core");
const http_loader_1 = require("@ngx-translate/http-loader");
const primeng_1 = require("primeng/primeng");
const ApiService_1 = require("./Services/ApiService");
const UtilityService_1 = require("./Services/UtilityService");
const ProconsModalService_1 = require("./Services/ProconsModalService");
const UserService_1 = require("./Services/UserService");
const CarService_1 = require("./Services/CarService");
const AccountService_1 = require("./Services/AccountService");
const ContextService_1 = require("./Services/ContextService");
const home_component_1 = require("./home/home.component");
const login_component_1 = require("./login/login.component");
const primeng_2 = require("primeng/primeng");
const ngx_modialog_1 = require("ngx-modialog");
const bootstrap_1 = require("ngx-modialog/plugins/bootstrap");
function createTranslateLoader(http) {
    return new http_loader_1.TranslateHttpLoader(http, './Assets/i18n/', '.json');
}
exports.createTranslateLoader = createTranslateLoader;
let AppModule = class AppModule {
};
AppModule = __decorate([
    core_1.NgModule({
        declarations: [
            app_component_1.AppComponent,
            catalogue_component_1.CatalogueComponent,
            SearchResults_component_1.SearchResultsComponent,
            SearchForm_component_1.searchFormComponent,
            profile_component_1.profileComponent,
            app_routing_1.routingComponents,
            home_component_1.HomeComponent,
            login_component_1.LoginComponent,
            worker_managment_component_1.WorkerMangmentComponent,
            view_profiles_component_1.ViewProfilesComponent,
            add_profile_component_1.AddProfileComponent
        ],
        imports: [
            bootstrap_1.BootstrapModalModule,
            ngx_modialog_1.ModalModule.forRoot(),
            angular2_busy_1.BusyModule,
            ngx_loading_1.LoadingModule,
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_2.HttpModule,
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
            primeng_2.DataGridModule,
            angular2_recaptcha_1.ReCaptchaModule,
            http_1.HttpClientModule,
            core_2.TranslateModule.forRoot({
                loader: {
                    provide: core_2.TranslateLoader,
                    useFactory: (createTranslateLoader),
                    deps: [http_1.HttpClient]
                }
            })
        ],
        providers: [CarService_1.CarService,
            ActivationGuard_1.CanActivateViaAuthGuard,
            ContextService_1.ContextService,
            ApiService_1.ApiService,
            UtilityService_1.UtilityService,
            AccountService_1.AccountService,
            UserService_1.UserService,
            ProconsModalService_1.ProconsModalSerivce],
        bootstrap: [app_component_1.AppComponent]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map