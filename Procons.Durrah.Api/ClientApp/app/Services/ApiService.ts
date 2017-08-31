import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ActivatedRoute } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import { Injectable, OnDestroy } from '@angular/core';
import { PaymentRedirectParams, KnetPayment, SearchCriteriaParams } from '../Models/ApiRequestType';
import { Worker } from '../Models/Worker';
@Injectable()
export class ApiService {

    private config = require('../../config.json');

    constructor(private http: Http, private activeRoute: ActivatedRoute) { }

    public login(userName: string, password: string): Observable<boolean> {
        let requestBody = "userName=" + userName + "&password=" + password + "&grant_type=password";

        return this.httpPostHelper(this.config.loginUrl, requestBody).map(response => {
            if (response.status == 200) {
                var token = response.json();
                console.log(token);
                sessionStorage.setItem("SecurityToken", token.access_token);
                sessionStorage.setItem("SecurityTokenExpiryDate", token['expires_in']);
                return true;
            } else
                return false;
        });
    }
    public knetPaymentRedirectUrl(paymentInformation: PaymentRedirectParams): Observable<string> {
        return this.httpPostHelper(this.config.knetUrl, paymentInformation)
            .map(response => {
                return response.json();
            });
    }


    public createIncomingPayment(payment: KnetPayment): Observable<String> {
        return this.httpPostHelper(this.config.incomingPaymentUrl, payment)
            .map(response => {
                return response.json();
            });
    }
    public getSearchCriteriaParameters(): Observable<SearchCriteriaParams> {
        return this.httpGetHelper(this.config.getSearchCriteriaUrl)
            .map(response => { return response.json(); })
    }
    public getAllWorkers(optionalFilterCritera: Worker): Observable<Worker[]> {
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
        // let headers = new Headers();
        // headers.append("Content-Type", 'application/x-www-form-urlencoded');

        return this.http.post(this.config.baseUrl + url, body)
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
        let securityToken: string = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken
    }

    //Reusable Helper Methods
    public getKnetUrlProperties() {
        return this.activeRoute.queryParams.map(x => {
            return {
                PaymentID: x.PaymentID,
                Postdate: x.Postdate,
                Result: x.Result,
                TranID: x.TranID,
                Auth: x.Auth
            }
        });
    }
    public redirectToUrl(url: string) {
        window.location.href = url;
    }

}
