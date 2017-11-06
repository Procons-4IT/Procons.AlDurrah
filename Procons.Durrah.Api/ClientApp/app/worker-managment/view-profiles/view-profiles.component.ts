import { Component, Input, EventEmitter, Output } from '@angular/core';

import { Worker } from "../../Models/Worker";
import * as moment from 'moment';
import { MomentDatePipe } from '../../moment-date.pipe';
import { LanguageConvertPipe } from '../../language-convert.pipe';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: "view-profiles",
    templateUrl: "./view-profiles.component.html",
    styles: ["./view-profiles.component.css"],
    providers: [MomentDatePipe,LanguageConvertPipe]
})
export class ViewProfilesComponent {
    @Input() workers: Worker[]
    @Output() onBack = new EventEmitter<any>();

    videoUrl;
    showVideoModal = false;

    languages: string = "";
    constructor(public sanitizer: DomSanitizer) { }

    back() {
        this.onBack.emit();
    }
    public openRequestedPopup(url) {
        
        this.videoUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        this.showVideoModal = true;
      }



}