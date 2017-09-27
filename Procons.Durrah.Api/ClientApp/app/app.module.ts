import { OVERLAY_PROVIDERS } from "@angular/material";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { RecipesComponent } from './recipes/recipes.component';
import { RecipeListComponent } from './recipes/recipe-list/recipe-list.component';
import { CatalogueComponent } from './Catalogue/catalogue.component';
import { SearchResultsComponent } from './Catalogue/SearchResults/SearchResults.component';
import { searchFormComponent } from './Catalogue/SearchForm/SearchForm.component';
import { profileComponent } from './Catalogue/Profile/profile.component';
import { WorkerMangmentComponent } from './worker-managment/worker-managment.component'

import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeItemComponent } from './recipes/recipe-list/recipe-item/recipe-item.component';
import { RoutingModule, routingComponents, } from './app.routing';
import { CanActivateViaAuthGuard } from "./Services/ActivationGuard";
import { RouterStateSnapshot } from '@angular/router';
import { BusyModule } from 'angular2-busy';
import { LoadingModule } from 'ngx-loading';
import { ReCaptchaModule } from 'angular2-recaptcha';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import {
    DataTableModule,
    SharedModule,
    SliderModule,
    DropdownModule,
    MultiSelectModule,
    PanelMenuModule,
    MenuItem,
    ButtonModule,
    DialogModule
} from 'primeng/primeng';

import { ApiService } from './Services/ApiService';
import { UtilityService } from './Services/UtilityService';

import { ProconsModalSerivce } from './Services/ProconsModalService';
import { UserService } from './Services/UserService';
import { CarService } from './Services/CarService';
import { AccountService } from './Services/AccountService';
import { ContextService } from './Services/ContextService';

import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { GvControlComponent } from './SharedComponents/gv-control/gv-control.component';
import { DataGridModule } from 'primeng/primeng';
import { ModalModule } from 'ngx-modialog';
import { BootstrapModalModule } from 'ngx-modialog/plugins/bootstrap';
export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './Assets/i18n/', '.json');
}
@NgModule({
    declarations: [
        AppComponent,
        CatalogueComponent,
        SearchResultsComponent,
        searchFormComponent,
        profileComponent,
        RecipesComponent,
        RecipeListComponent,
        RecipeDetailComponent,
        RecipeItemComponent,
        routingComponents,
        HomeComponent,
        LoginComponent,
        GvControlComponent,
        WorkerMangmentComponent
    ],
    imports: [
        BootstrapModalModule,
        ModalModule.forRoot(),
        BusyModule,
        LoadingModule,
        BrowserModule,
        FormsModule,
        HttpModule,
        PanelMenuModule,
        BrowserAnimationsModule,
        RoutingModule,
        DataTableModule,
        SharedModule,
        SliderModule,
        DropdownModule,
        MultiSelectModule,
        ButtonModule,
        DialogModule,
        DataGridModule,
        ReCaptchaModule,
        HttpClientModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [HttpClient]
            }
        })
    ],
    providers: [CarService,
        CanActivateViaAuthGuard,
        ContextService,
        ApiService,
        UtilityService,
        AccountService,
        UserService,
        OVERLAY_PROVIDERS,
        ProconsModalSerivce],

    bootstrap: [AppComponent]
})
export class AppModule { }
