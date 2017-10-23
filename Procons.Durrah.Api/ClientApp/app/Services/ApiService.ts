import { Observable } from 'rxjs/Observable';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/zip';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';


import { Injectable, OnDestroy } from '@angular/core';
import { WorkerFilterParams, ConfirmEmailParams, PaymentRedirectParams, KnetPayment, SearchCriteriaParams, ResetPasswordParams, CreateNewUserParams } from '../Models/ApiRequestType';
import { Worker, WorkerManagementData } from '../Models/Worker';
@Injectable()
export class ApiService {
    KEYS = {
        SecurityToken: "SecurityToken",
        SecurityTokenExpiryDate: "SecurityTokenExpiryDate",
        UserType: "UserType"
    }

    private config = require('../../config.json');
    private language: string;
    private userLoggedIn: BehaviorSubject<boolean>;
    private userTypeLogIn: BehaviorSubject<string>;


    public resetParams: ResetPasswordParams = {
        EmailAddress: "",
        Password: "",
        ValidationId: ""
    };

    constructor(private http: Http) {
        this.language = navigator.language;
        this.userLoggedIn = new BehaviorSubject(this.isLoggedIn());
        this.userTypeLogIn = (this.userLoggedIn as any).filter(x => x).map(x => {
            return this.GetUserType();
        });
    }

    public logOut() {
        this.RemoveSecurityToken();
        this.alertListenersUserLoggedIn(this.isLoggedIn());
    }

    public login(userName: string, password: string): Observable<any> {
        let requestBody = `grant_type=password&username=${userName}&password=${password}`;

        return this.httpPostHelper(this.config.loginUrl, requestBody).map(response => {
            console.log('I was called!  ');
            if (response.status == 200) {
                var token = response.json();

                sessionStorage.setItem(this.KEYS.SecurityToken, token.access_token);
                sessionStorage.setItem(this.KEYS.SecurityTokenExpiryDate, token['expires_in']);
                sessionStorage.setItem(this.KEYS.UserType, token.UserType);

                return true;
            } else
                return false;
        })
            .catch(errorResponse => {
                return Observable.throw(this.getErrorMessage(errorResponse));
            })
            .do(x => { this.alertListenersUserLoggedIn(true) });

    }
    private getErrorMessage(errorResponse): string {
        debugger;
        let errorObject: any = errorResponse.json && errorResponse.json();
        let errorMessage: string = errorObject.error_description || errorObject.message || "something went wrong";
        return errorMessage;
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
            }).catch(errorResponse => {
                return Observable.throw(this.getErrorMessage(errorResponse));
            });
    }
    public knetPaymentRedirectUrl(paymentInformation: PaymentRedirectParams): Observable<string> {
        return this.httpPostHelper(this.config.knetUrl, paymentInformation)
            .map(response => {
                return response.json();
            });
    }


    public createIncomingPayment(payment: KnetPayment): Observable<any> {
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

        var actualData = this.httpPostHelper(this.config.getWorkersUrl, optionalFilterCritera)
            .map(response => {
                var data: any[] = response.json();

                return data;
            });
        return actualData;
    }
    public getAllAgentWorkers(optionalFilterCritera: WorkerFilterParams | object): Observable<any[]> {

        var actualData = this.httpPostHelper(this.config.getAgentWorkerUrl, optionalFilterCritera)
            .map(response => {
                var data: any[] = response.json();
                return data;
            });
        return actualData;
    }

    public getWorkerManagmentData(): Observable<WorkerManagementData> {
        let $workerKeyData = this.getAllAgentWorkers({});
        let $searchCritera = this.getSearchCriteriaParameters();

        let $workerManagmentData = Observable.zip($workerKeyData, $searchCritera, (workerData: Worker[], searchCriterParams: SearchCriteriaParams) => {
            return { w: workerData, s: searchCriterParams };
        }).map(data => this.convertToWorkerWorkerManagementData(data.w, data.s));

        return $workerManagmentData
    }
    public convertToWorkerWorkerManagementData(workerServerData: Worker[], searchCriterParams: SearchCriteriaParams): WorkerManagementData {
        //fix performance! language -> languages mismatch, consider lodash, change server or hardcoding 
        let workerManagmentData = new WorkerManagementData();
        workerManagmentData.searchCriteria = searchCriterParams;
        workerManagmentData.workerServerData = workerServerData;
        let workerDisplayData = [];
        let tempWorker;
        let searchProperties: string[] = Object.getOwnPropertyNames(workerManagmentData.searchCriteria);

        for (let i = 0; i < workerManagmentData.workerServerData.length; i++) {
            tempWorker = workerManagmentData.workerServerData[i];
            workerDisplayData.push(this.convertWorkerValues(tempWorker, workerManagmentData.searchCriteria, searchProperties));
        }
        workerManagmentData.workerDisplayData = workerDisplayData;
        return workerManagmentData;

    }
    public convertWorkerValues(workerServerData: Worker, searchCriteria, searchProperties) {
        console.log('help!');
        //s['religion'][w1['religion']].name
        var workerDisplayData = Object.assign({}, workerServerData);
        var searchProperty;
        for (var searchIndex = 0; searchIndex < searchProperties.length; searchIndex++) {
            searchProperty = searchProperties[searchIndex];
            if (workerDisplayData.hasOwnProperty(searchProperty)) {
                var workerKey = workerDisplayData[searchProperty];
                var propertyValue = searchCriteria[searchProperty];
                let name = this.getNameFromProperty(workerKey, propertyValue);
                workerDisplayData[searchProperty] = name;
            }
        }
        console.log(workerDisplayData);
        return workerDisplayData;
    }
    public getNameFromProperty(key: string, propertyValue: any[]): string {
        //[{name,value}] 
        let keyValuePair;
        for (let i = 0; i < propertyValue.length; i++) {
            keyValuePair = propertyValue[i];
            if (keyValuePair.value === key) {
                return keyValuePair.name;
            }
        }
        return null;
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
    public uploadFile(formData) {
        let url = 'http://localhost:59822/api/Workers/AddWorker';
        let headers = new Headers();

        // headers.append('Content-Type', 'application/x-www-form-urlencoded');
        // headers.append("Content-Type", 'application/json');
        // headers.append("accept-language", this.language);
        // let options: RequestOptions = new RequestOptions({ headers: headers });
        return this.http.post(url, formData);

    }
    public updateWorker(worker: Worker): Observable<any> {
        return this.httpPostHelper(this.config.updateWorkerUrl, worker);
    }
    public addWorker(worker: Worker): Observable<any> {
        return this.httpPostHelper(this.config.addWorkerUrl, worker);
    }
    public deleteWorker(workerCode: String): Observable<any> {
        return this.httpPostHelper(this.config.deleteWorkerUrl, { "WorkerCode": workerCode });
    }

    public isLoggedIn(): boolean {
        return !!this.GetSecurityToken();
    }
    public GetUserType(): string {
        return sessionStorage.getItem(this.KEYS.UserType);
    }
    public GetSecurityToken(): string {
        return sessionStorage.getItem(this.KEYS.SecurityToken);
    }
    public RemoveSecurityToken(): void {
        sessionStorage.removeItem(this.KEYS.SecurityToken);
    }

    public alertListenersUserLoggedIn(isLoggedIn: boolean) {
        this.userLoggedIn.next(isLoggedIn);
    }
    public onUserLoggedIn(): BehaviorSubject<boolean> {
        return this.userLoggedIn;
    }
    public onUserTypeLoggedIn(): BehaviorSubject<string> {
        return this.userTypeLogIn;
    }
}
