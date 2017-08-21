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
import {Overlay} from 'angular2-modal';
import { overlayConfigFactory } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';

@Component({
    selector: '[app-login]',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    providers: [Modal, Overlay]
})
export class LoginComponent extends ComponentBase<any> implements OnInit {
    subscriptions: Subscription[];
    private ngUnsubscribe: Subject<void> = new Subject<void>();
    userName: string;
    password: string;
    displayError: boolean;
    errorMessage: string = "";
    constructor(private carService: CarService, private router: Router, private context: ContextService, public modal: Modal) {
        super();
    }

    ngOnInit() {

    }

    Login() {
        let header: Array<any> = new Array<any>();
        let requestBody = "userName=" + this.userName + "&password=" + this.password + "&grant_type=password";
        let serviceUrl: string = '/oauth/token';
        header.push({ key: "Content-Type", value: 'application/x-www-form-urlencoded' });

        this.context.httpPost(header, requestBody, serviceUrl)
            .takeUntil(this.ngUnsubscribe).subscribe(response => {
                if (response.status == 200) {
                    sessionStorage.setItem("SecurityToken", response.json().access_token);
                    sessionStorage.setItem("SecurityTokenExpiryDate", response.json()['.expires']);
                    this.OpenModal();
                    this.router.navigateByUrl("/Home");
                } else {

                }
            }, (error) => {
                this.errorMessage = "";
                this.errorMessage = error.json().error_description
                this.displayError = true;
            }, () => {
                this.userName = null;
                this.password = null;
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
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/eldurra-master/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>You are logged in</small></p>
                            <h4>Create your own password</h4>
                            <a href="#tabNewPass" data-toggle="tab" data-dismiss="modal">Continue</a>
                        </div>`)
            .open();
    }
}
//<div class="modal fade" tabindex= "-1" role= "dialog" id= "modalLogin" #modalLogin >
//    <div class="modal-dialog" role= "document" >
//        <div class="modal-content" >
//            <div class="modal-header" >
//                <button type="button" class="close" data- dismiss="modal" aria- label="Close" > <span aria- hidden="true" >&times; </span></button>
//                    </div>
//                    < div class="modal-body" >
//                        <div class="modal-icon" > <img src="/eldurra-master/src/app/images/icon_lock.png" class="icon" /></div>
//                            < p > <small>You are logged in </small></p>
//                                <h4>Create your own password< /h4>
//                                    < a href= "#tabNewPass" data- toggle="tab" data- dismiss="modal" > Continue < /a>
//                                        < /div>
//                                        < /div>
//                                        < /div>
//                                        < /div>