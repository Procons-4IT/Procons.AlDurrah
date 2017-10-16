import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Injectable, OnDestroy } from '@angular/core';

@Injectable()
export class ContextService {

    public loginPageFollower: Subject<boolean> = new Subject<boolean>();
    constructor(private http: Http) {
        this.loginPageFollower.next(false);
    }

    setLoginPage(islogin: boolean) {
        this.loginPageFollower.next(islogin);
    }

    public httpPost(requestHeaders: Array<any>, requestBody: string, serviceUrl: string): Observable<Response> {
        let headers = new Headers();
        requestHeaders.forEach(element => {
            debugger;
            headers.append(element.valueOf().key, element.valueOf().value);
        });

        let body = requestBody;
        return this.http.post(serviceUrl, body, headers);
    }

    public GetSecurityToken(): string {
        let securityToken: string = '';
        securityToken = 'bearer ' + sessionStorage.getItem('SecurityToken');
        return securityToken
    }
}