import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';
import { Worker } from "../../Models/Worker";

@Component({
    selector: "add-profile",
    templateUrl: "./add-profile.component.html",
    styles: ["./add-profile.component.css"]
})
export class AddProfileComponent implements OnInit {
    @Input() worker;
    @Output() onBack = new EventEmitter<any>();

    state: any = {
        isAddMode: true,
        title: "Add Profile",
        worker: {}
    }
    constructor() {
    }

    ngOnInit(): void {
        console.log('add-profile: ', this.worker);
        if (this.worker) {
            this.state.isAddMode = false;
            this.state.title = "Edit Profile"
            this.state.worker = Object.assign({}, this.worker);
        }
    }

    back() {
        this.onBack.emit();
    }
}