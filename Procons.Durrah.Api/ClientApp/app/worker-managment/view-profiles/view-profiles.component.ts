import { Component, Input, EventEmitter, Output } from '@angular/core';

import { Worker } from "../../Models/Worker";

@Component({
    selector: "view-profiles",
    templateUrl: "./view-profiles.component.html",
    styles: ["./view-profiles.component.css"]
})
export class ViewProfilesComponent {
    @Input() workers: Worker[]
    @Output() onBack = new EventEmitter<any>();

    constructor() {}

    back() {
        this.onBack.emit();
    }
}