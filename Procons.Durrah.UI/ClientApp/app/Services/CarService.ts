import { Injectable, OnDestroy } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { Observable } from 'rxjs/observable';


@Injectable()
export class CarService {
    token: string;
    constructor(private http: Http) { } name

    public httpPost(requestHeaders: [any], requestBody: string, serviceUrl: string): Observable<Response> {

        let headers = new Headers();
        requestHeaders.forEach(element => {
            headers.append(element.key, element.value);
        });

        let body = requestBody;
        return this.http.post(serviceUrl, body, headers);
    }

    getCarsMedium() {

    }

    isLogedIn(): boolean {

        if (!sessionStorage)
            return false;
        else {

            let expirationDate: Date = new Date(sessionStorage.getItem("SecurityTokenExpiryDate"));
            let currentDate: Date = new Date();
            if (currentDate >= expirationDate) {
                return false;
            }
            else {
                return true;
            }
        }
    }

    ngOnDestroy() {

    }
}