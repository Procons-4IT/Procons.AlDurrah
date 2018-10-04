import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ContextService } from './ContextService';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Injectable, OnDestroy } from '@angular/core';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class UserService {
    constructor(private http: Http, private cxtService: ContextService) {

    }

    public AddUser(requestBody: string, serviceUrl: string): Observable<Response> {
        let header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        let options = new RequestOptions({ headers: header });
        return this.http.post(serviceUrl, requestBody, options);
    }

    public GetUserRole() {
        let header = this.GetAuthorizationHeader();
        header.append('Content-Type', 'application/json');
        let options = new RequestOptions({ headers: header });
        return this.http.get("https://rdsh.proconscloud.com:4545", options);
        //return this.http.get("http://localhost:59822", options);
        //https://rdsh.proconscloud.com:4545
        //http://localhost:59822/api/roles/GetCurrentRole
    }

    public DeleteUser(requestHeaders: RequestOptions, serviceUrl: string): Observable<Response> {
        return this.http.delete(serviceUrl);
    }

    public GetUsers(serviceUrl: string): Observable<Response> {
        let options = new RequestOptions({ headers: this.GetAuthorizationHeader() });
        return this.http.get(serviceUrl, options);
    }

    public GetSecurityToken(): string {
        return this.cxtService.GetSecurityToken();
    }

    private GetAuthorizationHeader() {
        let header = new Headers();
        header.append("Authorization", this.GetSecurityToken());
        return header;
    }



}
