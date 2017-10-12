"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
let ProconsModalSerivce = class ProconsModalSerivce {
    constructor(modal, translate) {
        this.modal = modal;
        this.translate = translate;
    }
    showErrorModal(optionalMessage = "error.default", isTranslateKey = true) {
        if (isTranslateKey) {
            this.translate.get([optionalMessage, 'ok']).subscribe(errorMessages => {
                this.createErrorModalTemplate(errorMessages[optionalMessage], errorMessages['ok']);
            });
        }
        else {
            this.translate.get(['ok']).subscribe(errorMessages => {
                this.createErrorModalTemplate(optionalMessage, errorMessages['ok']);
            });
        }
    }
    createErrorModalTemplate(errorMessage, buttonMessage) {
        this.modal.alert()
            .showClose(true)
            .body(`
                        <div class="modal-body">
                            <div class="modal-icon"><img src="/Assets/src/app/images/icon_lock.png" class="icon" /></div>
                            <p><small>${errorMessage}</small></p>
                        </div>`)
            .okBtn(buttonMessage)
            .open();
    }
    showHTMLModal(html) {
        this.modal.alert()
            .showClose(true)
            .body(html)
            .open();
    }
};
ProconsModalSerivce = __decorate([
    core_1.Injectable()
], ProconsModalSerivce);
exports.ProconsModalSerivce = ProconsModalSerivce;
//# sourceMappingURL=ProconsModalService.js.map