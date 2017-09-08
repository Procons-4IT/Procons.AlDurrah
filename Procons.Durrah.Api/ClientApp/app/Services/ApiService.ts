import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';

import { Injectable, OnDestroy } from '@angular/core';
import { WorkerFilterParams, ConfirmEmailParams, PaymentRedirectParams, KnetPayment, SearchCriteriaParams, ResetPasswordParams, CreateNewUserParams } from '../Models/ApiRequestType';
import { Worker } from '../Models/Worker';
@Injectable()
export class ApiService {

    private config = require('../../config.json');
    private language: string;
    private userLoggedIn = new Subject<boolean>();

    constructor(private http: Http) {
        this.language = navigator.language;
    }

    public logOut() {
        this.RemoveSecurityToken();
        this.alertListenersUserLoggedIn(false);
    }
    
    public login(userName: string, password: string): Observable<boolean> {
        let requestBody = `grant_type=password&username=${userName}&password=${password}`;

        return this.httpPostHelper(this.config.loginUrl, requestBody).map(response => {
            if (response.status == 200) {
                var token = response.json();
                console.log(token);
                sessionStorage.setItem("SecurityToken", token.access_token);
                sessionStorage.setItem("SecurityTokenExpiryDate", token['expires_in']);
                return true;
            } else
                return false;
        }).do(x => { this.alertListenersUserLoggedIn(true) });
    }
    public forgotPassword(email: string): Observable<any> {
        var requestBody = { EmailAddress: email };;
        return this.httpPostHelper(this.config.forgotPasswordUrl, requestBody);

    }
    public resetPassword(param: ResetPasswordParams) {
        return this.httpPostHelper(this.config.resetPasswordUrl, param);
    }
    public confirmEmail(param: ConfirmEmailParams): Observable<boolean> {
        return this.httpPostHelper(this.config.emailValidationUrl, param)
            .map(response => {
                return response.status === 200;
            });
    }
    public createNewUser(param: CreateNewUserParams) {
        return this.httpPostHelper(this.config.createNewUserUrl, param)
            .map(response => {
                return response.json();
            });
    }
    public knetPaymentRedirectUrl(paymentInformation: PaymentRedirectParams): Observable<string> {
        return this.httpPostHelper(this.config.knetUrl, paymentInformation)
            .map(response => {
                return response.json();
            });
    }


    public createIncomingPayment(payment: KnetPayment): Observable<string> {
        return this.httpPostHelper(this.config.incomingPaymentUrl, payment)
            .map(response => {
                return response.json();
            });
    }
    public getSearchCriteriaParameters(): Observable<SearchCriteriaParams> {
        return this.httpGetHelper(this.config.getSearchCriteriaUrl)
            .map(response => { return response.json(); })
    }
    public getAllWorkers(optionalFilterCritera: WorkerFilterParams | object): Observable<Worker[]> {
        console.log('Getting All the Workers');
        var actualData = this.httpPostHelper(this.config.getWorkersUrl, optionalFilterCritera)
            .map(response => {
                var data: any[] = response.json();
                console.log('[server-worker data] ', data);
                return data;
            });
        return actualData;
    }
    public httpPostHelper(url: string, body: any): Observable<Response> {
        let headers = new Headers();
        headers.append("accept-language", this.language);
        let token = this.GetSecurityToken();
        if (token) {
            headers.append('Authorization', `bearer ${token}`);
        }
        let options: RequestOptions = new RequestOptions({ headers: headers });
        return this.http.post(this.config.baseUrl + url, body, options);
        // .catch(tempCatch => {
        //     var fakeObject = {
        //         json: () => [1, 2, 3]
        //     };
        //     return Observable.of(<Response>fakeObject);
        // });
    }
    public httpGetHelper(url: string): Observable<Response> {
        let headers = new Headers();
        headers.append("Content-Type", 'application/json');
        headers.append("accept-language", this.language);
        let options: RequestOptions = new RequestOptions({ headers: headers });

        return this.http.get(this.config.baseUrl + url, options)
        // .catch(tempCatch => {
        //     var fakeObject = {
        //         json: () => [1, 2, 3]
        //     };
        //     return Observable.of(<Response>fakeObject);

        // });
    }
    public GetSecurityToken(): string {
        // let securityToken: string = '';
        // securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        // return securityToken
        return sessionStorage.getItem('SecurityToken');
    }
    public RemoveSecurityToken(): void {
        sessionStorage.removeItem('SecurityToken');
    }

    public alertListenersUserLoggedIn(isLoggedIn: boolean) {
        this.userLoggedIn.next(isLoggedIn);
    }
    public onUserLoggedIn(): Subject<boolean> {
        return this.userLoggedIn;
    }
}
