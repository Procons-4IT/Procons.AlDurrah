import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Subscription } from 'rxjs/Subscription';
import { Subject } from 'rxjs/Subject';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/mergeMap';
import { ComponentBase } from '../app.ComponentBase';
import { Overlay } from 'angular2-modal';
import { overlayConfigFactory } from 'angular2-modal';
import { Modal } from 'angular2-modal/plugins/bootstrap';
import { ApiService } from '../Services/ApiService';

@Component({
    selector: '[app-paymentConfirmation]',
    templateUrl: './paymentConfirmation.component.html',
    styleUrls: ['./paymentConfirmation.component.css'],
    providers: [Modal, Overlay]
})
export class PaymentConfirmationComponent extends ComponentBase<any> implements OnInit {
    constructor(private myApiService: ApiService, private modal: Modal, private route: ActivatedRoute) {
        super();
    }

    ngOnInit() {
        this.myApiService.getKnetUrlProperties()
        .map(x=>this.myApiService.createIncomingPayment(x))
        .mergeMap(x=>x)
        .subscribe(onSuccess=>{
            console.log('Payment Attempted to Post: [server-response] ',onSuccess);
            this.OpenModal();
        });

    }



    OpenModal() {
        this.modal.alert()
            .showClose(true)
            .body(`
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>Payment Confirmed! </small></p>
                        </div>`)
            .open();
    }
}
