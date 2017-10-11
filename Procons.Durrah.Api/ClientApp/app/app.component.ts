import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { MenuItem } from 'primeng/primeng';
import { ContextService } from './Services/ContextService'
import { UserService } from './Services/UserService'
import { Router } from '@angular/router'
import { TranslateService } from '@ngx-translate/core';

import { ApiService } from './Services/ApiService';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    public items: MenuItem[];
    public isLoginPage: boolean;
    public isAgent: boolean = false;
    public isLoggedIn: boolean = false;
    public menuNode = {};

    constructor(private context: ContextService
        , private router: Router
        , private userService: UserService
        , private myApi: ApiService
        , private translate: TranslateService) {

        translate.setDefaultLang('ar');
        translate.use('ar');
        this.context.loginPageFollower.subscribe(value => { this.isLoginPage = value });
    }


    ngOnInit() {
        this.listenToUserLogin();
        this.listenToUserTypeLogin();
    }

    listenToUserLogin() {
        this.myApi.onUserLoggedIn().subscribe(x => {
            this.isLoggedIn = x;
        });
    }
    listenToUserTypeLogin() {
        this.myApi.onUserTypeLoggedIn().subscribe(userType => {
            console.log('adding user type ',userType);
            this.isAgent = userType === "cSupplier"
        });
    }
    logOut() {
        this.myApi.logOut()
    }


}
