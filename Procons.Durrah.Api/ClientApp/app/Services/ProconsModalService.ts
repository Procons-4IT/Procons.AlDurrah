import { Injectable, OnDestroy } from '@angular/core';
import { Modal } from 'ngx-modialog/plugins/bootstrap';


@Injectable()
export class ProconsModalSerivce {
    constructor(private modal: Modal) {

    }
    showErrorModal(){
        this.modal.alert()
        .showClose(true)
        .body(`
                    <div class="modal-body">
                        <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                        <p><small>Network Error</small></p>
                        <h4>please try again</h4>
                    </div>`
                )
        .okBtn('Ok')
        .open();

    }
}
