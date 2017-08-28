import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ActivatedRoute } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import { Injectable, OnDestroy } from '@angular/core';
import { PaymentRedirectParams,KnetPayment } from '../Models/ApiRequestType';
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
    public knetPaymentRedirect(paymentInformation: PaymentRedirectParams): Observable<string> {

        return this.httpPostHelper(this.config.knetUrl, paymentInformation).map(response => {
            if (response.status = 200) {
                var knetPortalUrl: string = response.json();
                if (knetPortalUrl) {
                    window.location.href = knetPortalUrl;
                }
            }
            console.error('knetPayment failed to get valid response ', response);
            return '';

        });
    }
    public createIncomingPayment(payment: KnetPayment): Observable<String> {
        return this.httpPostHelper(this.config.incomingPaymentUrl, payment)
            .map(response => {
                return response.json();
            });
    }
    public getAllWorkers(): Observable<Worker[]> {
        console.log('Getting All the Workers');
        var actualData = this.httpGetHelper(this.config.getWorkersUrl)
        .map(response=>{
            var data :any[]= response.json();
            console.log('[server-worker data] ',data);
            data.map(x=>{x.image = x.photo;x.video= "https://www.youtube.com/embed/o_XCxBbuaJE?rel=0&amp;showinfo=0"});
            return data;
        });
        return actualData;
    }
    public httpPostHelper(url: string, body: any): Observable<Response> {
        // let headers = new Headers();
        // headers.append("Content-Type", 'application/x-www-form-urlencoded');

        return this.http.post(this.config.baseUrl + url, body);
    }
    public httpGetHelper(url: string): Observable<Response> {
        let headers = new Headers();
        headers.append("Content-Type", 'application/json');
        let options: RequestOptions = new RequestOptions({ headers: headers });
        
        return this.http.get(this.config.baseUrl + url,options);
    }
    public GetSecurityToken(): string {
        let securityToken: string = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken
    }

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
}
