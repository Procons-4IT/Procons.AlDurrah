import {OVERLAY_PROVIDERS} from "@angular/material";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { RecipesComponent } from './recipes/recipes.component';
import { RecipeListComponent } from './recipes/recipe-list/recipe-list.component';
import { CatalogueComponent } from './catalogue/catalogue.component';
import { SearchResultsComponent } from './catalogue/SearchResults/SearchResults.component';
import { searchFormComponent } from './catalogue/SearchForm/SearchForm.component';
import { profileComponent } from './catalogue/profile/profile.component';

import { RecipeDetailComponent } from './recipes/recipe-detail/recipe-detail.component';
import { RecipeItemComponent } from './recipes/recipe-list/recipe-item/recipe-item.component';
import { RoutingModule, routingComponents, } from './app.routing';
import { CanActivateViaAuthGuard } from "./Services/ActivationGuard";
import { RouterStateSnapshot } from '@angular/router';
import { BusyModule } from 'angular2-busy';
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

import {ApiService} from './Services/ApiService';
import {UserService} from './Services/UserService';
import { CarService } from './Services/CarService';
import { AccountService } from './Services/AccountService';
import { ContextService } from './Services/ContextService';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { PaymentConfirmationComponent} from './paymentConfirmation/paymentConfirmation.component';
import { GvControlComponent } from './SharedComponents/gv-control/gv-control.component';
import { DataGridModule } from 'primeng/primeng';
import { ModalModule } from 'ngx-modialog';
import { BootstrapModalModule } from 'ngx-modialog/plugins/bootstrap';
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
        PaymentConfirmationComponent,
        GvControlComponent
    ],
    imports: [
        BootstrapModalModule,
        ModalModule.forRoot(),
        BusyModule,
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
        DataGridModule
    ],
    providers: [CarService,
        CanActivateViaAuthGuard,
        ContextService,
        ApiService,
        AccountService,
        UserService,
        OVERLAY_PROVIDERS],

    bootstrap: [AppComponent] 
})
export class AppModule { }
