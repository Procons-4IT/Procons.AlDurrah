import { Component, OnInit, OnDestroy, Input} from '@angular/core';
import { CarService } from '../Services/CarService'
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/takeUntil';
import { ContextService } from '../Services/ContextService'
import { ComponentBase } from '../app.ComponentBase';
import { Overlay } from 'ngx-modialog';
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import { ApiService } from '../Services/ApiService';

@Component({
    selector: '[app-login]',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [Modal, Overlay]
})
export class LoginComponent extends ComponentBase<any> implements OnInit {
    subscriptions: Subscription[];
    userName: string;
    password: string;
    displayError: boolean;
    errorMessage: string = "";
    constructor(private myApiService: ApiService, private carService: CarService, private router: Router, private context: ContextService, public modal: Modal) {
        super();
    }

    ngOnInit() {

    }

    Login(userName: string, password: string) {
        console.log('Logging in with userName: ',userName,' password: ','####');
        this.myApiService.login(userName, password).subscribe(isLoggedIn => {
            if (isLoggedIn) {
                this.OpenModal();
                this.router.navigateByUrl("/Home");
            } else {

            }
        }, (error) => {
            this.errorMessage = "";
            this.errorMessage = error.json().error_description
            this.displayError = true;
        }, () => {
            // this.userName = null;
            // this.password = null;
        });

    }

    Redirect() {
        window.location.href = "http://knet.testyourprojects.co.in/";
    }

    OnDestroy() {

    }


    OpenModal() {
        this.modal.alert()
            .showClose(true)
            .body(`
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>You are logged in</small></p>
                            <h4>Create your own password</h4>
                            <a href="#tabNewPass" data-toggle="tab" data-dismiss="modal">Continue</a>
                        </div>`)
            .open();
    }
}
