import { Injectable, OnDestroy } from '@angular/core';
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class ProconsModalSerivce {
    constructor(private modal: Modal, public translate: TranslateService) {

    }

    showErrorModal(optionalMessage: string = "error.default") {
        this.translate.get([optionalMessage, 'ok']).subscribe(errorMessages => {
            this.modal.alert()
                .showClose(true)
                .body(`
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>${errorMessages[optionalMessage]}</small></p>
                        </div>`
                )
                .okBtn(errorMessages['ok'])
                .open();


        });
    }

    showHTMLModal(html: string) {
        this.modal.alert()
            .showClose(true)
            .body(html)
            .open();
    }
}
