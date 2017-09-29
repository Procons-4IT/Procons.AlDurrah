import { Component } from '@angular/core';
import { Worker } from "../Models/Worker";

@Component({
    selector: "worker-managment",
    templateUrl: "./worker-managment.component.html",
    styles: ["./worker-managment.component.css"]
})
export class WorkerMangmentComponent {

    state = {
        showProfiles: false,
        showWorkerManagment: false,
        showAddProfile: true,
        workers: Array(7)
    }

    constructor() {
        console.log('render the page!');
    }

    showProfiles() {
        this.state.showWorkerManagment = false;
        this.state.showProfiles = true;
    }

    showAddWorker() {
        console.log('do some state managment logic');
    }
}