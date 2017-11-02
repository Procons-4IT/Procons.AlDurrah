import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class UtilityService {

    constructor() { }
    // http://localhost:4200/paymentconfirmation?paymentid=7051302202373050&result=captured&postdate=1102&tranid=2544329212373050&auth=703383&ref=730523915761&trackid=5941653&udf2=cc004&udf3=dw00002&udf4=541254785&udf5=45            Udf1: CardCode---Udf2: ItemCode---Udf3 Worker Code----Udf5:Amount
    //Reusable Helper Methods
    public getKnetUrlProperties(activeRoute: ActivatedRoute) {
        return activeRoute.queryParams.map(x => {
            return {
                PaymentID: x.paymentid, //someNumber
                Postdate: x.postdate,//date
                Result: x.result, //captured
                TranID: x.tranid, //number
                Auth: x.auth, // number
                Ref: x.ref,//number
                TrackID: x.trackid,//number
                CardCode: x.udf1,
                ItemCode: x.udf2,
                WorkerCode: x.udf3,
                Amount: x.udf5

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
