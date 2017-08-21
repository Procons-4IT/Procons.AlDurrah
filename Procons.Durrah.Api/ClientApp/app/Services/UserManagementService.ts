import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Injectable, OnDestroy } from '@angular/core';

@Injectable()
export class UserManagementService {
    constructor(private http: Http) {

    }
    public CreateUser(requestHeaders: RequestOptions, requestBody: string, serviceUrl: string): Observable<Response> {
        return this.http.post(serviceUrl, requestBody, requestHeaders);
    }

    public DeleteUser(requestHeaders: RequestOptions, serviceUrl: string): Observable<Response> {
        return this.http.delete(serviceUrl);
    }

    public GetUsers(serviceUrl: string): Observable<Response> {
        let headers = new Headers();
        return this.http.get(serviceUrl);
    }

}
