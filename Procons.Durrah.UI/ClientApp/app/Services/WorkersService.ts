import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { ContextService } from './ContextService';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Injectable, OnDestroy } from '@angular/core';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class WorkersService {
    constructor(private http: Http, private cxtService: ContextService) {

    }

    public GetLookups(serviceUrl: string): Observable<Response> {
        return this.http.get(serviceUrl);
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
