import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { MenuItem } from 'primeng/primeng';
import { ContextService } from './Services/ContextService'
import { UserService } from './Services/UserService'
import { Router } from '@angular/router'
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    public items: MenuItem[];
    public isLoginPage: boolean;
    public isAdmin: boolean;
    public menuNode = {};
    constructor(private context: ContextService, private router: Router, private userService: UserService) {
        this.context.loginPageFollower.subscribe(value => { this.isLoginPage = value });
    }
  

    ngOnInit() {
        debugger;


    }

}
