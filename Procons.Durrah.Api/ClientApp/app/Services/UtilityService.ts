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
                PaymentID: x.paymentid,
                Postdate: x.postdate,
                Result: x.result,
                TranID: x.tranid,
                Auth: x.auth
            }
        });
    }
    public getResetPasswordUrlProperties(activeRoute: ActivatedRoute) {
        return activeRoute.queryParams.map(x => {
            return {
                Email: x.email,
                ValidationId: x.validationid
            }
        });
    }

    public redirectToUrl(url: string) {
        window.location.href = url;
    }

}
