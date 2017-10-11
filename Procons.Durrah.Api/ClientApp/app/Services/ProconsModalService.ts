import { Injectable, OnDestroy } from '@angular/core';
import { Modal } from 'ngx-modialog/plugins/bootstrap';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class ProconsModalSerivce {
    constructor(private modal: Modal, public translate: TranslateService) {

    }

    showErrorModal(optionalMessage: string = "error.default", isTranslateKey = true) {
        if (isTranslateKey) {
            this.translate.get([optionalMessage, 'ok']).subscribe(errorMessages => {
                this.createErrorModalTemplate(errorMessages[optionalMessage], errorMessages['ok']);
            });
        }else{
            this.translate.get(['ok']).subscribe(errorMessages => {
                this.createErrorModalTemplate(optionalMessage, errorMessages['ok']);
            });
        }

    }

    private createErrorModalTemplate(errorMessage: string, buttonMessage: string) {
        this.modal.alert()
            .showClose(true)
            .body(`
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>${errorMessage}</small></p>
                        </div>`
            )
            .okBtn(buttonMessage)
            .open();


    }

    showHTMLModal(html: string) {
        this.modal.alert()
            .showClose(true)
            .body(html)
            .open();
    }
}
