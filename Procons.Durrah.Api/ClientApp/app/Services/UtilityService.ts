import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class UtilityService {

    constructor() { }

    //Reusable Helper Methods
    public getKnetUrlProperties(activeRoute: ActivatedRoute) {
        return activeRoute.queryParams.map(x => {
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
