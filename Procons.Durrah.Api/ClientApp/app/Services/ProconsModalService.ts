import { Injectable, OnDestroy } from '@angular/core';
import { Modal } from 'ngx-modialog/plugins/bootstrap';


@Injectable()
export class ProconsModalSerivce {
    constructor(private modal: Modal) {

    }

    showErrorModal(optionalMessage: string = "Network Error") {
        this.modal.alert()
            .showClose(true)
            .body(`
                    <div class="modal-body">
                        <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                        <p><small>${optionalMessage}</small></p>
                    </div>`
            )
            .okBtn('Ok')
            .open();

    }
    showHTMLModal(html: string) {
        this.modal.alert()
            .showClose(true)
            .body(html)
            .open();
    }
}
